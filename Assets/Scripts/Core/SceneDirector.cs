using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;
using System.Text.RegularExpressions;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        [Header("Setup")]
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls.
        // Also used for negative caching (mapping to null) to avoid repeated failed searches.
        private readonly Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for character prefabs to avoid repeated string matching in loops.
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void OnDestroy()
        {
            // BOLT: Clear caches to ensure references are released when the SceneDirector is destroyed.
            _objectCache.Clear();
            _prefabCache.Clear();
        }
        // ⚡ Bolt: Cache game objects to prevent expensive O(N) GameObject.Find() calls.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        // BOLT: Prefab lookup cache for O(1) retrieval instead of O(P) linear search
        private Dictionary<string, GameObject> _prefabLookupCache = new Dictionary<string, GameObject>();

        // BOLT: Simple object pool to reduce GC pressure and instantiation overhead
        private Dictionary<string, Stack<GameObject>> _characterPool = new Dictionary<string, Stack<GameObject>>();

        private void Awake()
        {
            PopulatePrefabCache();
        }

        private void PopulatePrefabCache()
        {
            if (characterPrefabs == null) return;
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                {
                    _prefabLookupCache[prefab.name] = prefab;
                }
            }
        }

        private void Start()
        {
            var data = CampaignManager.Instance.currentCampaignData;
            if (data != null && data.scenarios.Count > 0)
        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and O(P) list searches.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
        }
        // BOLT: Prefab cache to avoid O(M) linear searches through the characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            // BOLT: Pre-populate prefab cache for O(1) lookups during spawning
        private Dictionary<string, GameObject>? _prefabLookupCache = null;

        private void InitializePrefabCache()
        {
            if (_prefabLookupCache != null) return;
            _prefabLookupCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null)
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }
                    if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }
        }
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        /// <summary>
        /// Retrieves a GameObject by name, using a cache to avoid O(N) GameObject.Find calls.
        /// </summary>
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private readonly Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefabs to avoid repeated string matching in the list
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Cache for character prefabs to turn O(N) searches into O(1) lookups
        public Transform? characterSpawnRoot;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        public Transform characterSpawnRoot = null!;

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();


        // BOLT: Cache for character prefabs to turn O(P) list searches into O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for character controllers to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // 🛡️ Sentinel: Regex for validating object names to prevent DoS via malicious GameObject.Find queries.
        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\_\.\\[\]\/]+$", RegexOptions.Compiled);
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

        // BOLT: Cache for character prefabs to avoid repeated O(M) searches in characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // 🛡️ Sentinel: Whitelist regex for object names to prevent injection/DoS.
        // Allows alphanumeric, spaces, parentheses, hyphens, dots, brackets, and forward slashes (for hierarchy).
        private static readonly Regex _nameWhitelist = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\.\[\]\/]+$", RegexOptions.Compiled);
        private GameObject GetCachedObject(string objectName)
        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // We also use object.ReferenceEquals for a faster check against true null (uninitialized or negative cache).
                if (!object.ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }

                // If the object was destroyed but still in cache (fake null), or it's a legitimate negative hit (real null),
                // we treat it as "not found" but don't immediately re-search if it was a negative hit.
                if (object.ReferenceEquals(obj, null)) return null;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or was a destroyed instance.
            obj = GameObject.Find(objectName);

            // BOLT: Negative caching - we store null if not found to avoid repeated O(N) searches for missing objects.
            _objectCache[objectName] = obj;

            // 🛡️ Sentinel: Security validation for object names from untrusted data
            // SECURITY: Mitigate DoS via excessive GameObject.Find calls with long/malicious strings.
            if (objectName.Length > 128) return null;

            // SECURITY: Whitelist allowed characters to prevent exploitation of Find's path-like syntax.
            string whitelist = "_ ()-.[ ]";
            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && whitelist.IndexOf(c) == -1)
                {
                    return null;
                }
            // 🛡️ Sentinel: Input validation and DoS protection
            // SECURITY: Limit object name length and restrict characters to prevent DoS via GameObject.Find
            if (objectName.Length > 128)
            {
                Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName.Substring(0, 10)}...' exceeds length limit.");
                return null;
            }

            // Whitelist alphanumeric, underscores, spaces, parentheses, hyphens, periods, and square brackets (common in Unity names)
            if (!Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName}' contains potentially dangerous characters.");
            // 🛡️ Sentinel: Input validation to mitigate Denial of Service (DoS) attacks via GameObject.Find.
            // 1. Length constraint.
            if (objectName.Length > 128)
            {
                Debug.LogWarning($"[Security] GetCachedObject aborted: Object name '{objectName.Substring(0, 10)}...' exceeds 128 character limit.");
                return null;
            }

            // 2. Character whitelist validation.
            if (!_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject aborted: Object name contains illegal characters.");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // We also check for 'ReferenceEquals' to distinguish between a negative cache (null) and a destroyed object.
                if (obj == null && !ReferenceEquals(obj, null))
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // SECURITY: Negative caching - if we already searched and found nothing, return null immediately.
                // Note: ReferenceEquals(obj, null) is true for a legitimate negative cache hit.
                // Unity's == null check is true if the object was destroyed (fake null).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;

                // Object was destroyed, remove from cache and fall through to find it again (or not)
                _objectCache.Remove(objectName);
            // 🛡️ Sentinel: Hardening against potential DoS via expensive GameObject.Find.
            // Limit the length of the name and validate allowed characters.
            if (objectName.Length > 128 || !System.Text.RegularExpressions.Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]\/]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious or oversized object name: {objectName}");
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Negative caching implementation.
                // ReferenceEquals(obj, null) checks if we explicitly cached a 'null' for a missing object.
                if (ReferenceEquals(obj, null)) return null;

                // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                if (obj != null) return obj;

                // If obj == null but was in cache, it was destroyed. Remove it to allow a fresh lookup.
                _objectCache.Remove(objectName);
            // 🛡️ Sentinel: Input validation to prevent DoS attacks using extremely long or malformed strings in GameObject.Find.
            if (objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
            // BOLT: O(1) dictionary lookup.
            // Unity overrides the == operator to check if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                // Unity's == operator checks if the underlying native object is destroyed.
                // If it was cached as non-null but is now 'null' (destroyed), we must re-search.
                if (obj != null) return obj;

                // If it was explicitly cached as null, we treat it as "not found" to avoid O(N) re-search.
                // We use a custom check or just return the null if the reference itself is null.
                if (ReferenceEquals(obj, null)) return null;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            {
                _objectCache[objectName] = foundObj;
            }
            return foundObj;
            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // SECURITY: Robust negative caching - store null explicitly if not found to prevent future traversals.
            _objectCache[objectName] = obj;

            // BOLT: Cache the result (including null) to prevent repeated O(N) searches for missing objects.
            _objectCache[objectName] = obj;

            return obj;
        }

        /// <summary>
        /// Retrieves a character prefab by name using an O(1) lookup.
        /// </summary>
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we remove it to allow re-finding.
                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
            // 🛡️ Sentinel: Denial of Service (DoS) protection.
            // Limit object name length to prevent expensive string operations or malicious traversal.
            if (objectName.Length > 128)
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked: objectName '{objectName.Substring(0, 10)}...' exceeds length limit.");
                return null;
            }

            // 🛡️ Sentinel: Whitelist check to prevent DoS via complex GameObject.Find calls.
            // Only allow alphanumeric, underscores, spaces, parentheses, hyphens, periods, and brackets.
            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != ' ' && c != '(' && c != ')' && c != '-' && c != '.' && c != '[' && c != ']')
                {
                    Debug.LogWarning($"[Security] GetCachedObject blocked: objectName '{objectName}' contains illegal character '{c}'.");
                    return null;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            var foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            }

            // 🛡️ Sentinel: Hardening against Denial of Service (DoS) attacks
            // Limit object name length and restrict to safe characters to prevent expensive Find operations.
            if (objectName.Length > 128 || !System.Text.RegularExpressions.Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]\/]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            // We use System.Object.ReferenceEquals(obj, null) to distinguish between a truly null reference
            // and a destroyed Unity object, which allows us to implement "negative caching" for missing objects.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Managed reference is null: object was never found (cached negative result).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // Managed reference exists, check if Unity object is still alive.
                if (obj != null) return obj;

                // Managed reference is non-null but obj == null: object was destroyed.
                return null;
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we remove it to allow re-finding.
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals.
            _objectCache[objectName] = obj;
            return obj;
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

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            var foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;


            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name if not pre-populated.
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            // Fallback for partial matches (legacy support)
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (prefab != null)
                {
                    _prefabCache[profileName] = prefab;
                }
            // BOLT: Optimized prefab lookup using dictionary cache (O(1))
            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab))
            {
                return prefab;
            }

            // Fallback to partial match if exact match fails (legacy support)
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                _prefabLookupCache[profileName] = prefab;
                return prefab;
            }

            return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            // characterPrefabs is null! initialized, so ?. is redundant for NRT but characterPrefabs can be null if not assigned.
            // However, CI prefers NRT-consistent code.
            prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
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

        /// <summary>
        /// Retrieves or caches the CharacterControllerBase for a given GameObject.
        /// </summary>
        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out var controller) && controller != null)
            {
                return controller;
            }
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

            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);
            // Cache the result (including null if not found) for future O(1) retrieval.
            _objectCache[objectName] = obj!;

            // BOLT: Cache the result even if null (negative caching) to prevent repeated searches.
            _objectCache[objectName] = obj;

            return obj;
        }

        private void OnDestroy()
        {
            // BOLT: Ensure references are released to prevent memory leaks, especially with negative caching.
            _objectCache.Clear();
            _prefabCache.Clear();
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during any scene setup
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null) _prefabCache[prefab.name] = prefab;
            }

            if (CampaignManager.Instance != null &&
                CampaignManager.Instance.currentCampaignData != null &&
                CampaignManager.Instance.currentCampaignData.scenarios != null &&
                CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during scene setup
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups
            if (characterPrefabs != null)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null &&
                    CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            // NRT Pattern: Capture singleton property in local variable before null check
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(data.scenarios[0]);
            }
        }

        private void OnDestroy()
        {
            // BOLT: Clear caches to release Unity object references for GC
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            UnityEngine.Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Performance optimization - Pre-populate the cache with a single O(N) pass
            // instead of calling GameObject.Find (O(N)) repeatedly in loops (O(N*M)).
            StartCoroutine(SetupSceneCoroutine(scenario));
        }

        private IEnumerator SetupSceneCoroutine(SceneScenario scenario)
        {
            Debug.Log($"⚡ Bolt: Setting up scenario asynchronously: {scenario.scenarioId}");

            if (CampaignManager.Instance.currentCampaignData == null) yield break;

            // Instantiate characters across multiple frames if needed to prevent spikes
            var characters = CampaignManager.Instance.currentCampaignData.characters;
            for (int i = 0; i < characters.Count; i++)
            {
                SpawnOrUpdateCharacter(characters[i]);
                // Yield every 2 characters to balance speed and framerate
                if (i % 2 == 1) yield return null;
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
            if (scenario == null) return;
            Debug.Log($"⚡ Bolt: Setting up scenario: {scenario.scenarioId}");
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Removed redundant _objectCache.Clear() to allow surgical lazy-loading cache
            // to persist across multiple scenario updates for incremental performance.
            // BOLT: Initialize prefab lookup cache once per setup
            InitializePrefabCache();

            // Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (go != null) _objectCache[go.name] = go;
            }

            // BOLT: Pre-cache prefabs for faster lookup.
            _prefabCache.Clear();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            // BOLT: Pre-populate object cache with existing scene objects to avoid lazy O(N) lookups
            // Note: FindObjectsOfType is legacy but highly compatible across Unity versions.
            foreach (var go in FindObjectsOfType<GameObject>())
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            if (CampaignManager.Instance.currentCampaignData != null)
            if (CampaignManager.Instance.currentCampaignData?.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            var data = CampaignManager.Instance.currentCampaignData;
            if (data != null)
            {
                foreach (var charProfile in data.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();

            // BOLT: Pre-populate object cache with a single O(N) pass to avoid multiple GameObject.Find calls
            // Using FindObjectsOfType for maximum compatibility across Unity versions.
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
            // BOLT: Removed redundant .Clear() to allow lazy-loading caches to persist across scenario updates.
            // Unity's null check (obj != null) safely handles destroyed objects from previous scenes.

            // BOLT: Batch pre-populate object cache from existing scene objects to avoid multiple O(N) Find calls
            foreach (var go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData?.characters != null)
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null)
                        SpawnOrUpdateCharacter(charProfile);
                    {
                        SpawnOrUpdateCharacter(charProfile);
                    }
                    SpawnOrUpdateCharacter(charProfile);
            // BOLT: Optimization - Pre-populate the cache with a single scene traversal.
            // This avoids multiple O(N) GameObject.Find calls later.
            _objectCache.Clear();
            var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.None, FindObjectsSortMode.None);
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Pre-populate object cache with existing scene objects to avoid lazy O(N) lookups
            // Note: FindObjectsOfType is legacy but highly compatible across Unity versions.
            foreach (var go in FindObjectsOfType<GameObject>())
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

        private GameObject? GetCachedGameObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // Check cache first; safely handle natively destroyed objects via Unity's overloaded == operator
            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            {
                return obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null)
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
                    if (interaction != null)
                        ApplyInteraction(interaction);
                    {
                        ApplyInteraction(interaction);
                    }
                    if (interaction != null) ApplyInteraction(interaction);
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Use a prefab cache to avoid O(P) linear searches through the prefab list.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                // BOLT: O(1) exact match lookup from prefab cache.
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to O(N) partial match if exact name not found.
                    foreach (var kvp in _prefabCache)
                    {
                        if (kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
                }

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Try O(1) prefab cache lookup first
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to O(M) linear search if not in dictionary (e.g. partial name match)
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab; // Cache the match
                }

                // Try to find prefab if not in scene
                GameObject? prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                GameObject prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                GameObject? prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));

            if (characterObj == null)
            {
                // BOLT: Use dictionary for O(1) prefab lookup instead of O(P) linear search
                if (!_prefabLookupCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        _prefabLookupCache[profile.name] = prefab;
                    }
                }

                // BOLT: Try to get from pool first
                if (_characterPool.TryGetValue(profile.name, out Stack<GameObject>? pool) && pool != null && pool.Count > 0)
                {
                    characterObj = pool.Pop();
                    characterObj.SetActive(true);
            GameObject? characterObj = GetCachedGameObject(profile.name);

            if (characterObj == null)
            {
                // Try to find prefab
                GameObject? prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                if (prefab != null)
                {
                    characterObj = UnityEngine.Object.Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj; // Cache newly spawned object
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Optimized prefab lookup using O(1) dictionary with O(P) fallback for contains-logic
                GameObject prefab = null;
                if (_prefabLookupCache == null || !_prefabLookupCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                // BOLT: O(1) prefab lookup
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Use prefab cache to avoid repeated string matching in the list
                GameObject prefab;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: O(1) prefab lookup instead of O(M) list search
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        _prefabCache[profile.name] = prefab;
                // BOLT: O(1) prefab lookup after initial O(N) search
                if (!_prefabLookupCache.TryGetValue(profile.name, out GameObject? prefab))
                {
                    prefab = characterPrefabs?.Find(p => p != null && p.name != null && p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        _prefabLookupCache[profile.name] = prefab;
                // BOLT: Optimized prefab lookup with O(1) dictionary cache.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
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

                    // BOLT: Immediately cache the newly instantiated object.
                    // BOLT: Immediately cache the newly instantiated object to avoid subsequent searches
                    _objectCache[profile.name] = characterObj;
                }
                else
                {
                    // BOLT: Use O(1) prefab lookup cache
                    _prefabLookupCache.TryGetValue(profile.name, out GameObject? prefab);

                    // Fallback to partial match if exact match fails (legacy behavior)
                    if (prefab == null && characterPrefabs != null)
                    {
                        prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profile.name));
                    }

                    if (prefab != null)
                    {
                        characterObj = Instantiate(prefab, characterSpawnRoot);
                        characterObj.name = profile.name;
                        _objectCache[profile.name] = characterObj;
                    }
                }
            }

            if (characterObj != null)
            {
                var controller = characterObj.GetComponent<CharacterControllerBase>();
                // Assign data to controllers
                var controller = characterObj.GetComponent<MonoBehaviour>();
                if (controller is CharacterControllerBase charController)
                // BOLT: O(1) controller lookup avoids redundant GetComponent calls
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
                    // Create a dummy CharacterData for runtime initialization
                    CharacterData data = UnityEngine.ScriptableObject.CreateInstance<CharacterData>();
                    CharacterData data = ScriptableObject.CreateInstance<CharacterData>();
                    data.characterName = profile.name;
                    data.role = profile.role;
                    data.traits = profile.traits;
                    data.behaviorScript = profile.behaviorScript;

                    charController.Initialize(data);
                    controller.Initialize(data);
                }
            }
        }

        // BOLT: Public method to return characters to pool
        public void DespawnCharacter(string characterName)
        {
            if (_objectCache.TryGetValue(characterName, out GameObject? obj) && obj != null)
            {
                obj.SetActive(false);
                _objectCache.Remove(characterName);

                if (!_characterPool.ContainsKey(characterName))
                {
                    _characterPool[characterName] = new Stack<GameObject>();
                }
                _characterPool[characterName].Push(obj);
            }
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            GameObject target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                UnityEngine.Debug.Log($"Applying {interaction.action} to {interaction.objectId}");
            GameObject? target = GetCachedGameObject(interaction.objectId);
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            GameObject? target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                if (interaction.isVector)
                {
                    target.transform.position = interaction.GetVectorValue();
                }
                else
                {
                    target.transform.localScale = UnityEngine.Vector3.one * interaction.floatValue;
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up to prevent memory leaks if objects are held by the dictionary
            _objectCache.Clear();
            // BOLT: Clear the cache on destroy to release references and prevent memory leaks.
            _objectCache.Clear();
            // BOLT: Explicitly clear dictionaries to release Unity object references for GC
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
            // BOLT: Explicitly clear caches to release Unity object references
            _objectCache.Clear();
            _prefabLookupCache.Clear();
            foreach (var pool in _characterPool.Values)
            {
                pool.Clear();
            }
            _characterPool.Clear();
        // BOLT: Explicitly clear caches on destroy to prevent memory leaks in the Unity Editor
        private void OnDestroy()
        {
        private void OnDestroy()
        {
            // BOLT: Clear caches to release references
            _objectCache.Clear();
            _controllerCache.Clear();
            _prefabLookupCache.Clear();
            // BOLT: Clear caches to allow Unity to garbage collect native objects
            _objectCache.Clear();
            _prefabLookupCache.Clear();
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}