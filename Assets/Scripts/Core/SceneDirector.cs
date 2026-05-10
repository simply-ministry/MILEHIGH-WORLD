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
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab lookup cache to avoid O(P) linear searches
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();
        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and linear searches
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefab lookups to prevent O(N) list searches on instantiation
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Consolidated caches for GameObjects, prefabs, and controllers to prevent expensive searches and GetComponent calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
1        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Component cache to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
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

        // BOLT: Cache for prefabs to prevent O(N) list searches during instantiation
        private Dictionary<string, GameObject> _prefabCache;

        private void InitializePrefabCache()
        {
            if (_prefabCache != null) return;

            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        // BOLT: Cache to prevent O(N) list searches with string comparisons inside setup loops
        private Dictionary<string, GameObject> _prefabCache;

        // BOLT: Cache for character prefabs to prevent O(N) list searches during scene setup
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private GameObject GetCachedPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName) || characterPrefabs == null) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject prefab))
            {
                return prefab; // Will return null if it was cached as null
            }

            // Fallback to O(N) search and cache the result (even if null)
            prefab = characterPrefabs.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        // BOLT: Cache for prefab lookups to avoid O(N) list search with string comparisons
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for prefabs to replace O(M) list searching with O(1) dictionary lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            // BOLT: Pre-populate prefab cache for O(1) retrieval during character spawning
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Performance optimization - use ReferenceEquals for native null checks.
                // If both are true, it's a legitimate negative cache hit (real null).
                if (ReferenceEquals(obj, null))
                {
                    return null;
                }

                // If obj == null is true but ReferenceEquals is false, the object was destroyed (fake null)
                if (obj != null)
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Implement negative caching to prevent repeated O(N) lookups for missing objects
            _objectCache[objectName] = obj;

            return obj;
            {
                // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // If it's not null and not a "fake null" (destroyed), return it.
                if (obj != null) return obj;

                // BOLT: Implement negative caching and handle "fake nulls" (destroyed objects).
                // If obj == null but ReferenceEquals is false, it's a destroyed object; otherwise it's a legitimate null cache.
                if (System.Object.ReferenceEquals(obj, null)) return null; // Legitimate negative cache hit (real null)
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or cache was stale.
            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj; // Cache the result, even if it's null (negative caching)

            return obj;
            {
                // BOLT: Robust negative caching check.
                // Unity's '==' operator returns true for destroyed objects (fake nulls).
                // ReferenceEquals(obj, null) is only true for "real" C# nulls (negative cache hits).
                if (obj != null) return obj;
                if (System.Object.ReferenceEquals(obj, null)) return null;
                // If it's a "fake null", we fall through to re-find it in the scene.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
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
                // BOLT: Distinguish between negative cache (true null) and destroyed object (Unity null)
                if (ReferenceEquals(obj, null)) return null;
                if (obj == null) // Unity object was destroyed
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            GameObject foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj; // negative cache if not found
            return foundObj;
        }

            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            GameObject? foundObj = GameObject.Find(objectName);
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                if (!ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }
                _objectCache.Remove(objectName); // Clean up stale/destroyed references
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);
            // BOLT: Cache the result even if null to prevent repeated failed lookups (negative caching).
            _objectCache[objectName] = obj;
            return obj;
        }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            int instanceID = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(instanceID, out CharacterControllerBase controller) && controller != null)
            // BOLT: Implement negative caching by storing null results.
            if (obj != null)
            {
                return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null)
            {
                _controllerCache[instanceID] = controller;
            }
            return controller;
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private void InitializePrefabCache()
        {
            if (_prefabCache != null) return;
            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private void Start()
        private GameObject? GetPrefab(string profileName)
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            if (string.IsNullOrEmpty(profileName)) return null;
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (prefab != null) _prefabCache[profileName] = prefab;
            }
            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabLookupCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            InitializePrefabCache();

            // Clear cache at start of setup to avoid stale references across scenes
            // BOLT: Clear scene-specific caches to avoid stale references across scenarios/scenes.
            // Prefab cache is persisted as it remains valid across the session.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Clear lookup caches at start of setup to avoid stale references and memory leaks across scenes.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();
            // Clear object and controller caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                SpawnOrUpdateCharacter(charProfile);
            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            if (controller != null)
            {
                _controllerCache[objId] = controller;
            }
            return controller;
        }

        private void Start()
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabLookupCache[prefab.name] = prefab;
                }
            }

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            _objectCache.Clear();
            _controllerCache.Clear();

            if (CampaignManager.Instance?.currentCampaignData?.characters != null)
            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
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
            if (profile == null) return;
            InitializePrefabCache();
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using the pre-populated dictionary (O(1) vs O(M))
                // Try exact match first for O(1) performance
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to fuzzy match if exact match fails (O(M))
                    foreach (var kvp in _prefabCache)
                    {
                        if (kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
                }
                // Try to find prefab if not in scene
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                }
                if (_prefabCache.TryGetValue(profile.name, out GameObject cachedPrefab))
                {
                    prefab = cachedPrefab;
                GameObject prefab = GetCachedPrefab(profile.name);
                // BOLT: Replaced O(N) list search with O(1) dictionary lookup where possible
                GameObject prefab = null;
                if (_prefabCache != null)
                {
                    // Fast exact match
                    if (!_prefabCache.TryGetValue(profile.name, out prefab))
                    {
                        // Fallback to Contains if exact match fails, and cache the result for O(1) next time
                        prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                        if (prefab != null)
                        {
                            _prefabCache[profile.name] = prefab;
                        }
                    }
                }
                else
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                }
                // Try to find prefab if not in scene, first using O(1) lookup
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to O(N) contains if exact name match fails, then cache the result
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        _prefabCache[profile.name] = prefab;
                    }
                }
                // BOLT: O(1) prefab lookup via cache instead of O(M) list search.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                }
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
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object to resolve negative cache hits.
                    // BOLT: Immediately cache the newly instantiated object to prevent redundant searches
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Use GetCachedController to avoid expensive GetComponent calls.
                var controller = GetCachedController(characterObj);
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
            if (interaction == null) return;
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
