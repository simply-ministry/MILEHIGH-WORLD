using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs; // Assign in Inspector
        public Transform characterSpawnRoot;

        // BOLT: Unified caching system to replace multiple redundant/conflicting declarations.
        // Uses O(1) lookups to eliminate expensive O(N) scene traversals and linear list searches.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private void Start()
        {
            InitializePrefabCache();

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear scene-specific caches to avoid stale references and memory leaks.
            _objectCache.Clear();
            _controllerCache.Clear();

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

            foreach (var interaction in scenario.interactiveObjects)
            {
                ApplyInteraction(interaction);
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: O(1) prefab lookup via dictionary
                if (_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                var controller = GetCachedController(characterObj);
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

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Handle Unity's native object lifetime (fake nulls)
                if (obj != null) return obj;
                _objectCache.Remove(objectName);
            }

            GameObject foundObj = GameObject.Find(objectName);
            if (foundObj != null) _objectCache[objectName] = foundObj;

            return foundObj;
        }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            int instanceID = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(instanceID, out CharacterControllerBase controller))
            {
                if (controller != null) return controller;
                _controllerCache.Remove(instanceID);
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[instanceID] = controller;

            return controller;
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            GameObject target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                Debug.Log($"Applying {interaction.action} to {interaction.objectId}");
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
