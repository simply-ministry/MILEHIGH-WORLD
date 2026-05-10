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

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();


        // BOLT: Cache for character prefabs to turn O(P) list searches into O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for character controllers to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private void EnsurePrefabCache()
        {
            if (_prefabCache.Count > 0 || characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        private GameObject GetCachedObject(string objectName)
        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                if (System.Object.ReferenceEquals(obj, null)) return null;

                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null)
            {
                _prefabCache[profileName] = prefab;
            }
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller)) return controller;

            // NRT Pattern: Explicitly mark component as nullable before caching
            CharacterControllerBase? newController = characterObj.GetComponent<CharacterControllerBase>();
            if (newController != null)
            {
                _controllerCache[objId] = newController;
            }
            return newController;
            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null)
            {
                _controllerCache[objId] = controller;
            }
            return controller;
            // BOLT: Perform an O(1) dictionary lookup first.
            // Note: ReferenceEquals(obj, null) checks if the managed reference is null,
            // while obj == null (Unity's override) checks if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                if (!ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }
                _objectCache.Remove(objectName); // Clean up stale/destroyed references
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null) return null;

                return obj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result even if null (negative caching) to prevent repeated searches.
            _objectCache[objectName] = obj;

            return obj;
        }

        private void Start()
        {
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            // NRT Pattern: Capture singleton property in local variable before null check
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            _objectCache.Clear();
            _controllerCache.Clear();

            // NRT Pattern: Capture singleton property in local variable
            if (CampaignManager.Instance.currentCampaignData != null)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();
            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabLookupCache.Clear();

            // BOLT: Pre-populate prefab cache for O(1) lookup during spawning
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        // We use the full name as the key. Character names usually match or contain the prefab name.
                        if (!_prefabLookupCache.ContainsKey(prefab.name))
                        {
                            _prefabLookupCache[prefab.name] = prefab;
                        }
                    }
                }
            }

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                // Instantiate characters if not already in scene
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    if (interaction != null) ApplyInteraction(interaction);
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Use O(1) prefab cache helper
                GameObject? prefab = GetPrefab(profile.name);
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Optimized O(1) prefab lookup via dictionary
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact name not in cache
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                // instead of characterPrefabs.Find (O(P))
                GameObject? prefab = null;

                // Try exact match first
                if (!_prefabLookupCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact match fails (legacy support)
                    foreach (var kvp in _prefabLookupCache)
                    {
                        if (kvp.Key != null && kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
                }

                if (prefab != null)
                {
                    // Unity Performance Pattern: Use generic Instantiate for type safety
                    characterObj = Instantiate<GameObject>(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                var controller = GetCharacterController(characterObj);
                // BOLT: Optimized component access using GetInstanceID() to avoid redundant GetComponent calls
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller) || controller == null)
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    if (controller != null) _controllerCache[instanceId] = controller;
                }

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

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}