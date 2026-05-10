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

        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and linear searches
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Consolidated caches for GameObjects, prefabs, and controllers to prevent expensive searches and GetComponent calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
1        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
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
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Handle "fake nulls" (destroyed native objects) and negative caching.
                // If the reference is not null but Unity says it is, the object was destroyed.
                if (obj == null && !ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj; // Return cached object or legitimate null (negative cache hit)
                }
            // Note: ReferenceEquals(obj, null) checks if the managed reference is null,
            // while obj == null (Unity's override) checks if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
1            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Check if the cached reference is a destroyed Unity object (fake null)
                // vs a legitimate negative cache entry (real null).
                if (obj == null && !ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                if (!ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }
                _objectCache.Remove(objectName); // Clean up stale/destroyed references
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);
            // BOLT: Implement negative caching by storing null results.
            if (obj != null)
            {
                _objectCache[objectName] = obj;
            }
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject GetPrefab(string profileName)
        {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            // BOLT: Clear lookup caches at start of setup to avoid stale references and memory leaks across scenes.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();
            // Clear object and controller caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
            {
                SpawnOrUpdateCharacter(charProfile);
            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[objId] = controller;
            return controller;
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during any scene setup
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
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
                // BOLT: Optimized prefab lookup using O(1) dictionary cache instead of O(P) linear search
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                // BOLT: Optimized O(1) prefab lookup via dictionary
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact name not in cache
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                // BOLT: O(1) prefab lookup instead of O(P) list search.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                // BOLT: Use O(1) prefab cache helper
                GameObject prefab = GetPrefab(profile.name);
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
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object to prevent redundant searches
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Avoid expensive GetComponent calls by caching character controllers using InstanceID
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller))
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    _controllerCache[instanceId] = controller;
                }

                // BOLT: Optimized component access using GetInstanceID() to avoid redundant GetComponent calls
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller) || controller == null)
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    if (controller != null) _controllerCache[instanceId] = controller;
                }

                // BOLT: O(1) controller lookup instead of O(N) GetComponent search.
                int id = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(id, out CharacterControllerBase controller))
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    if (controller != null) _controllerCache[id] = controller;
                }

                // BOLT: Use O(1) controller cache to avoid redundant GetComponent
                var controller = GetCharacterController(characterObj);
                if (controller != null)
                {
                    // Create a dummy CharacterData for runtime initialization
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
