using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform O(1) lookup
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Return cached reference (Unity's == operator handles destruction check)
                if (obj != null || System.Object.ReferenceEquals(obj, null))
                {
                    return obj;
                }

                // If Unity object was destroyed but entry isn't a negative cache entry, remove it
                _objectCache.Remove(objectName);
            }

            // Fallback to scene traversal
            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // Fallback for partial matches if not in cache
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                _prefabLookupCache[profileName] = prefab;
            }

            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            // BOLT: Using InstanceID (int) for cache key to avoid string allocations
            if (_controllerCache.TryGetValue(objId, out var controller) && controller != null)
                return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            return controller;
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache for O(1) lookups
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches to avoid stale references across scenarios
            _objectCache.Clear();
            _controllerCache.Clear();

            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                var controller = GetCharacterController(characterObj);
                if (controller != null)
                {
                    CharacterData data = ScriptableObject.CreateInstance<CharacterData>();
                    data.characterName = profile.name;
                    data.role = profile.role;
                    data.traits = profile.traits;
                    data.behaviorScript = profile.behaviorScript;

                    controller.Initialize(data);
                }
            }
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            GameObject? target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                if (interaction.isVector)
                {
                    target.transform.position = interaction.GetVectorValue();
                }
                else
                {
                    target.transform.localScale = Vector3.one * interaction.floatValue;
                }
            }
        }
    }
}
