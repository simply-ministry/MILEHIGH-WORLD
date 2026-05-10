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
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefab lookups to avoid O(P) linear searches
        private Dictionary<string, GameObject> _prefabLookupCache = new Dictionary<string, GameObject>();

        // ⚡ Bolt: Cache game objects to prevent expensive O(N) GameObject.Find() calls.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabLookupCache = null;

        private void InitializePrefabCache()
        {
            if (_prefabLookupCache != null) return;
            _prefabLookupCache = new Dictionary<string, GameObject>();

        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and O(P) list searches.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Consolidated caches to prevent expensive lookups and redundant GetComponent calls.
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // 🛡️ Sentinel: Regex for validating object names to prevent expensive GameObject.Find on malicious strings.
        private static readonly Regex _nameValidator = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\_\.\/\[\]]+$", RegexOptions.Compiled);
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();


        // BOLT: Cache for character prefabs to turn O(N) searches into O(1) lookups
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();

        private GameObject? GetCachedObject(string objectName)
        public Transform? characterSpawnRoot;
        public Transform characterSpawnRoot = null!;

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Component cache to avoid redundant GetComponent calls.
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel: Regex for whitelisting safe object names to prevent DoS via expensive GameObject.Find operations.
        private static readonly Regex _nameWhitelist = new Regex(@"^[a-zA-Z0-9_\s\-.\[\]\(\)\$]+$", RegexOptions.Compiled);
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private static readonly System.Text.RegularExpressions.Regex _nameWhitelist = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_\s\(\)\-$\.\/\[\]]+$", System.Text.RegularExpressions.RegexOptions.Compiled);
        // Note: Using GameObject? to support negative caching (storing confirmed nulls)
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // BOLT: Component cache to avoid redundant GetComponent calls.
        // Note: Using CharacterControllerBase? to support negative caching.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // Uses GameObject? for negative caching (null for confirmed missing objects)
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches.
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        public List<GameObject> characterPrefabs; // Assign in Inspector
        public Transform characterSpawnRoot;

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
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefabs to avoid linear searches in the characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            // BOLT: Pre-index prefabs for O(1) lookups during character spawning
        // BOLT: Prefab cache to avoid O(M) linear searches through the characterPrefabs list
        // BOLT: Cache for prefab lookups to avoid O(P) list searches during spawning
        private Dictionary<string, GameObject> _prefabLookupCache = new Dictionary<string, GameObject>();

        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent memory leaks
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
        private readonly Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefabs to avoid repeated string matching in the list
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Unified caching system to replace multiple redundant/conflicting declarations.
        // Uses O(1) lookups to eliminate expensive O(N) scene traversals and linear list searches.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();


        // 🛡️ Sentinel: Whitelist regex for object names to prevent malicious input.
        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\[\]\.]+$", RegexOptions.Compiled);
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls.
        // Performance Insight: Dictionary.TryGetValue is O(1).
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
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
        // BOLT: Cache for character prefabs to prevent expensive O(N) List.Find calls
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefab lookups to avoid O(N) list searches
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Component cache to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            // 🛡️ Sentinel: Implement input length limits and validation to prevent DoS via expensive Find operations.
            if (string.IsNullOrEmpty(objectName) || objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                return null;
            }

            // 🛡️ Sentinel: Validate input string length and content to mitigate DoS via expensive Find operations.
            if (objectName.Length > 128 || !_nameValidator.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
                return null;
            }

            // BOLT: O(1) dictionary lookup.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Note: Unity overrides the == operator. If obj is a destroyed Unity object, it will evaluate to null.
                // We use ReferenceEquals to check for 'true' null entries (negative caching).
                if (System.Object.ReferenceEquals(obj, null)) return null;
                if (obj == null)
            // 🛡️ Sentinel: DoS Mitigation - Enforce length limit and character whitelist on object names.
            if (objectName.Length > 128 || !_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
            // 🛡️ Sentinel: DoS Mitigation - Limit string length and validate against whitelist
            // to prevent expensive GameObject.Find from being triggered by malicious or oversized input.
            if (objectName.Length > 128 || !_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            GameObject? obj;
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            if (_objectCache.TryGetValue(objectName, out obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            if (_objectCache.TryGetValue(objectName, out obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // by removing it from cache and falling through to GameObject.Find.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // BOLT: Perform O(1) lookup
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Return cached reference (Unity's == operator handles destruction check)
                if (obj != null || System.Object.ReferenceEquals(obj, null))
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
        // BOLT: Cache for character prefabs to turn O(P) list searches into O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // 🛡️ Sentinel: Regex for validating object names to prevent DoS via malicious GameObject.Find queries.
        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\_\.\\[\]\/]+$", RegexOptions.Compiled);
        private void Start()
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
                    if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }
        }
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

        // BOLT: Cache for prefab lookups to avoid O(N) string comparisons in loops
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Prefab cache to replace O(M) list searching with O(1) dictionary lookup
        private Dictionary<string, GameObject> _prefabCache;

        // BOLT: Prefab cache to replace O(M) linear search through characterPrefabs list with O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            // BOLT: Pre-populate prefab cache for O(1) lookups during spawning
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null)
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        // BOLT: Cache for prefabs to prevent expensive O(N) List.Find string matching inside loops
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for prefab lookups to prevent O(N) list searches with string comparisons
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Prefab cache to avoid O(M) linear searches through characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            if (characterPrefabs == null) return;
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        // BOLT: Cache for character prefabs to replace O(M) linear searches with O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Security validation for object names from untrusted data
            // SECURITY: Mitigate DoS via excessive GameObject.Find calls with long/malicious strings.
            if (objectName.Length > 128) return null;

            // SECURITY: Whitelist allowed characters to prevent exploitation of Find's path-like syntax.
            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && !"_ ()-.[ ]".Contains(c))
                {
                    return null;
                }
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                if (obj != null) return obj;

                // BOLT: Robust negative caching check.
                // If obj == null but ReferenceEquals is false, the object was destroyed (fake null).
                if (System.Object.ReferenceEquals(obj, null))
                {
                    return null; // Negative cache hit (real null).
                }

                // Object was destroyed; remove stale reference from cache.
                _objectCache.Remove(objectName);
            // 🛡️ Sentinel: Hardening against Denial of Service (DoS) attacks
            // Limit object name length and restrict to safe characters to prevent expensive Find operations.
            if (objectName.Length > 128 || !System.Text.RegularExpressions.Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]\/]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
                return null;
            }

            // 🛡️ Sentinel: DoS Hardening - prevent expensive Find operations with malicious or oversized strings
            // SECURITY: Whitelist regex ensures only safe characters are passed to GameObject.Find
            if (objectName.Length > 128 || !Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\$\.\\[\]\/]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject: Blocked potentially malicious object name: {objectName}");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // BOLT: Check if the cached reference is a destroyed Unity object (fake null)
                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                if (System.Object.ReferenceEquals(obj, null)) return null;
                if (obj == null)
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we remove it to allow re-finding.
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // 🛡️ Sentinel: Mitigate DoS via complex GameObject.Find queries.
            // Restrict name length and characters to prevent expensive scene traversals with complex patterns.
            if (objectName.Length > 128) return null;

            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != ' ' && c != '(' && c != ')')
                {
                    return null;
                }
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // ReferenceEquals(obj, null) is true for real nulls (including negative cache hits).
                if (ReferenceEquals(obj, null))
                {
                    return null; // Negative cache hit
                }

                if (obj != null)
                {
                    return obj;
                }
                // Object was destroyed but still in cache, remove and fallback
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                // BOLT: Unity's custom == operator checks if the native object was destroyed.
                // We use ReferenceEquals to check for literal nulls (negative cache hits).
                if (obj != null) return obj;
                if (ReferenceEquals(obj, null)) return null; // Genuine negative cache hit

                // If obj == null but ReferenceEquals is false, it means the object was destroyed.
                // We remove it and fall back to Find.
                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);
            // BOLT: Cache the result, including nulls (negative caching) to prevent repeated O(N) traversals
            _objectCache[objectName] = obj;
            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Implement negative caching to prevent repeated expensive searches for non-existent objects
            if (obj != null)
            {
                _objectCache[objectName] = obj;
            }
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Robust negative caching - use ReferenceEquals to check for real nulls
                // vs fake nulls (destroyed Unity objects).
                if (ReferenceEquals(obj, null)) return null; // Negative cache hit

                if (obj != null) return obj; // Valid cached object
            {
                // BOLT: Performance Learning - Robust negative caching requires checking both Unity null (destroyed) and C# null (not found)
                // If ReferenceEquals is true, it's a legitimate negative cache hit (real null).
                // If ReferenceEquals is false but obj == null is true, the object was destroyed (fake null).
                if (ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;
            // Performance Insight: We use negative caching (storing 'null' for missing objects)
            // to avoid repeated O(N) scene traversals for objects that don't exist.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity overrides the == operator for GameObjects. If 'obj' was destroyed, it returns null.
                // We must check if the reference is actually valid.
                if (ReferenceEquals(obj, null))
                {
                    // This is a legitimate negative cache hit (real null).
                    return null;
                }

                if (obj == null)
                {
                    // Object was destroyed (fake null). Remove from cache and continue.
                    _objectCache.Remove(objectName);
                }
                else
                {
                    // Valid object found.
                    return obj;
                }
            }

            obj = GameObject.Find(objectName);
            if (obj != null) _objectCache[objectName] = obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);

            // Performance Insight: Even if obj is null, we store it to enable negative caching.
            _objectCache[objectName] = obj;

            return obj;
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

        /// <summary>
        /// Retrieves a GameObject by name, using a cache to avoid O(N) GameObject.Find calls.
        /// </summary>
        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: O(1) dictionary lookup.
            // Unity overrides the == operator to check if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                return obj;
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

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals.
            // We store the result in the dictionary. If Find returns null, it's a 'true' null.
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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
                // If Unity object was destroyed but entry isn't a negative cache entry, remove it
                _objectCache.Remove(objectName);
            }

            // 🛡️ Sentinel: Input validation to prevent DoS attacks using extremely long or malformed strings in GameObject.Find.
            if (objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null)
            // Fallback to scene traversal
            obj = GameObject.Find(objectName);
            // Note: ReferenceEquals(obj, null) checks if the managed reference is null,
            // while obj == null (Unity's override) checks if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
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

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
            }

            GameObject foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj; // negative cache if not found
            return foundObj;
        }

            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            GameObject? foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
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

            // BOLT: Cache the result, including nulls for negative caching
            _objectCache[objectName] = obj;

            return obj;
            // BOLT: Negative caching - store the result even if it is null to avoid repeated O(N) traversals
            _objectCache[objectName] = obj;

            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName))!;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Security validation of input to prevent DoS via GameObject.Find
            // Limit character name length and restrict characters to alphanumeric/safe symbols
            if (objectName.Length > 128)
            {
                Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName.Substring(0, 10)}...' exceeds 128 character limit.");
                return null;
            }

            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != ' ' && c != '(' && c != ')')
                {
                    Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName}' contains invalid characters.");
                    return null;
                }
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Use ReferenceEquals to distinguish between a 'true' null and a 'Unity' null.
                if (System.Object.ReferenceEquals(obj, null)) return null;
                if (obj == null) return null;
                return obj;
            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));

            GameObject? prefab;
            if (_prefabCache.TryGetValue(profileName, out prefab)) return prefab;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
            if (string.IsNullOrEmpty(profileName)) return null;

        // 🛡️ Sentinel: Exact-match blacklist of critical system objects to prevent IDOR-like tampering
        private readonly HashSet<string> _restrictedSystemObjects = new HashSet<string>
        {
            "CampaignManager",
            "SceneDirector",
            "CameraManager",
            "AlliancePowerManager"
        };

        private GameObject GetCachedObject(string objectName)
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                if (prefab != null) return prefab;
            }

            // BOLT: Optimized prefab lookup using characterPrefabs list
            // We do a partial match search only if the name isn't already cached.
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

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
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            // We use ReferenceEquals to check if the managed reference exists in the cache,
            // then Unity's == null check to see if the native object was destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                if (obj != null) return obj;

                // BOLT: Negative caching hit (O(1)). If obj is null but existed in dictionary,
                // it was either never found or destroyed.
                return null;
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search only happens once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (string.IsNullOrEmpty(profileName)) return null;

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);
            // Cache the result (including null if not found) for future O(1) retrieval.
            _objectCache[objectName] = obj!;
            return obj;
        }

        private void OnDestroy()
        {
            // BOLT: Ensure references are released to prevent memory leaks, especially with negative caching.
            _objectCache.Clear();
            _prefabCache.Clear();

            // BOLT: Cache the result, even if it's null (negative caching), to prevent repeated O(N) lookups.
            _objectCache[objectName] = obj;

            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // Fallback to searching the list if not pre-cached
            if (characterPrefabs != null)
            {
                GameObject? foundPrefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (foundPrefab != null)
                {
                    _prefabCache[profileName] = foundPrefab;
                    return foundPrefab;
                }
            }
            return null;
        }

            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p.name.Contains(profileName));

            // BOLT: Cache result (including null for negative caching) to ensure O(1) subsequent lookups
            if (prefab != null)
            {
                _prefabCache[profileName] = prefab;
            }
            return prefab;
        }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            var foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            return prefab;
        }

        // BOLT: Cache for character prefabs to avoid repeated O(M) searches in characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private GameObject GetCachedObject(string objectName)
            if (prefab != null) _prefabCache[profileName] = prefab;
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
        private CharacterControllerBase? GetCharacterController(GameObject? characterObj)
            // BOLT: O(P) search only happens once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
            prefab = characterPrefabs.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
        private void InitializePrefabCache()
        {
            if (_prefabCache != null) return;

            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
            // BOLT: Cache the result even if null to prevent repeated failed lookups (negative caching).
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            // SECURITY: Using non-nullable reference directly to avoid CS8604 if null-conditional is redundant
            prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));

            // BOLT: O(P) search happens only once per profile name
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // Fallback for partial matches if not in cache
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                _prefabLookupCache[profileName] = prefab;
            }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            InitializePrefabCache();

            GameObject? obj;

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

            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) to critical system singletons
            if (objectName == "CampaignManager" || objectName == "SceneDirector" ||
                objectName == "CameraManager" || objectName == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Access to protected core manager '{objectName}' is denied.");
                return null;
            }

            // 🛡️ Sentinel: Prevent IDOR tampering with core architectural singletons
            // SECURITY: Exact match blocklist prevents unauthorized data-driven manipulation
            if (objectName == "CampaignManager" ||
                objectName == "SceneDirector" ||
                objectName == "CameraManager" ||
                objectName == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized access attempt to critical system object: {objectName}");
                return null;
            }

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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

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
            // 🛡️ Sentinel: Hardening against potential DoS via expensive GameObject.Find.
            // Limit the length of the name and validate allowed characters.
            if (objectName.Length > 128 || !System.Text.RegularExpressions.Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]\/]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious or oversized object name: {objectName}");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // SECURITY: Negative caching - if we already searched and found nothing, return null immediately.
                // Note: ReferenceEquals(obj, null) is true for a legitimate negative cache hit.
                // Unity's == null check is true if the object was destroyed (fake null).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;

                // Object was destroyed, remove from cache and fall through to find it again (or not)
                _objectCache.Remove(objectName);
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity's == operator checks if the underlying native object is destroyed.
                // If it was cached as non-null but is now 'null' (destroyed), we must re-search.
                if (obj != null) return obj;

                // If it was explicitly cached as null, we treat it as "not found" to avoid O(N) re-search.
                // We use a custom check or just return the null if the reference itself is null.
                if (ReferenceEquals(obj, null)) return null;
            }

        // 🛡️ Sentinel: Regex for white-listing safe characters in object names to prevent DoS via GameObject.Find
        private static readonly Regex _safeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\.\-\[\]]+$");
        private const int MAX_NAME_LENGTH = 128;

        private GameObject GetCachedObject(string objectName)
            GameObject? foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Sanitize input to mitigate DoS risks and ensure data integrity
            if (objectName.Length > MAX_NAME_LENGTH || !_safeNameRegex.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject: Rejected potentially unsafe object name '{objectName}'");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Robust negative caching check.
                // In Unity, == null is true for destroyed objects. ReferenceEquals check
                // distinguishes between a "legitimate" null (not found) and a "fake" null (destroyed).
                if (obj == null)
                {
                    if (ReferenceEquals(obj, null)) return null; // Negative cache hit (object definitely doesn't exist)

                    _objectCache.Remove(objectName); // Object was destroyed, remove from cache
                }
                else
                {
                    return obj;
                }
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            // SECURITY: Robust negative caching - store null explicitly if not found to prevent future traversals.
            _objectCache[objectName] = obj;

            // BOLT: Cache the result (including null) to prevent repeated O(N) searches for missing objects.
            _objectCache[objectName] = obj;

            return obj;
        }

        private void Start()
        {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            // BOLT: Fallback to O(N) scene traversal only if not cached or reference is stale.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result even if null to enable negative caching.
            _objectCache[objectName] = obj;

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result, including null to enable negative caching
            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result, including null (Negative Caching), to prevent repeated Find calls.
            _objectCache[objectName] = obj;

            if (obj != null)
            {
                _objectCache[objectName] = foundObj;
            }
            return foundObj;
            GameObject? foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals.
            _objectCache[objectName] = obj;
            return obj;
            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
            return prefab;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
        /// <summary>
        /// Retrieves a character prefab by name using an O(1) lookup.
        /// </summary>
        private GameObject? GetPrefab(string profileName)
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            if (string.IsNullOrEmpty(profileName)) return null;

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
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            // Fallback for partial matches (legacy support)
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (prefab != null)
                {
                    _prefabCache[profileName] = prefab;
                }
            }
            return prefab;
        }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or was a destroyed instance.
            obj = GameObject.Find(objectName);

            // BOLT: Negative caching - we store null if not found to avoid repeated O(N) searches for missing objects.
            _objectCache[objectName] = obj;

            return obj;
        /// <summary>
        /// Retrieves or caches the CharacterControllerBase for a given GameObject.
        /// </summary>
        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name if not pre-populated.

            // BOLT: Perform O(1) dictionary lookup
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                // Handle negative caching and destroyed objects
                if (ReferenceEquals(prefab, null)) return null;
                if (prefab != null) return prefab;
            }

            // BOLT: Fallback to O(P) linear search with partial match support
            GameObject? foundPrefab = characterPrefabs?.Find(p => p != null && (p.name == profileName || p.name.Contains(profileName)));

            // BOLT: Cache result (including null for negative caching)
            _prefabCache[profileName] = foundPrefab;
            return foundPrefab;
        }

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: Initial lookup in characterPrefabs list.
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches on destroy to release Unity engine object references.
            // This assists the garbage collector and prevents memory leaks or stale reference issues between scene loads.
            _objectCache?.Clear();
            _prefabCache?.Clear();
        }

        private void OnDestroy()
        {
            // BOLT: Clear caches to release Unity object references for GC
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            StartCoroutine(SetupSceneCoroutine(scenario));
        }
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out var controller) && controller != null)
            {
                return controller;
            }
            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();
            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller)) return controller;
            CharacterControllerBase? controller;
            if (_controllerCache.TryGetValue(objId, out controller)) return controller;
            if (_controllerCache.TryGetValue(objId, out var controller))
            {
                if (controller != null) return controller;
            }
            // BOLT: Use dictionary lookup with negative caching support
            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();

            // BOLT: Pre-populate object cache with a single O(N) pass to avoid multiple GameObject.Find calls
            // Using FindObjectsOfType for maximum compatibility across Unity versions.
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }
            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;

            // BOLT: We no longer clear the cache here to allow persistent surgical lazy-loading.
            // This provides performance benefits across multiple calls.
            // BOLT: Cache component reference to avoid redundant GetComponent calls
            // BOLT: Cache the result even if null (negative caching) to avoid redundant GetComponent calls
            _controllerCache[objId] = controller;
            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller)) return controller;

            // BOLT: Initialize prefab lookup cache once per setup
            InitializePrefabCache();

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

        private GameObject GetCachedGameObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // Check cache first; safely handle natively destroyed objects via Unity's overloaded == operator
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
        private void Start()
        {
            if (CampaignManager.Instance != null &&
                CampaignManager.Instance.currentCampaignData != null &&
                CampaignManager.Instance.currentCampaignData.scenarios != null &&
                CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during scene setup
            // BOLT: Pre-populate prefab cache
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups
            // BOLT: Pre-populate prefab cache for O(1) lookups
            if (characterPrefabs != null)
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }

            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Length > 0)
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            if (CampaignManager.Instance.currentCampaignData != null &&
                CampaignManager.Instance.currentCampaignData.scenarios != null &&
                CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
                    if (prefab != null) _prefabLookupCache[prefab.name] = prefab;
                }
            }

            GameObject foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            // NRT Pattern: Capture singleton property in local variable before null check
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
            // UNITY NRT Flow Analysis Pattern: Capture singleton property in local variable
            var campaignManager = CampaignManager.Instance;
            if (campaignManager != null)
            {
                var data = campaignManager.currentCampaignData;
                if (data != null && data.scenarios != null && data.scenarios.Count > 0)
                {
                    SetupScene(data.scenarios[0]);
                }
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            // 🛡️ Sentinel: Capture singleton property to local for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            // BOLT: Capture property in local variable for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                    if (prefab != null)
                    {
                         _prefabCache[prefab.name] = prefab;
                         _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
                    if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                    if (prefab != null) _prefabLookupCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null &&
                    CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Performance optimization - Pre-populate the cache with a single O(N) pass
            // instead of calling GameObject.Find (O(N)) repeatedly in loops (O(N*M)).
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
            GameObject characterObj = GetCachedGameObject(profile.name);

            if (characterObj == null)
            {
                // Try to find prefab
                GameObject prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj; // Cache newly spawned object
            if (scenario == null) return;
            Debug.Log($"⚡ Bolt: Setting up scenario: {scenario.scenarioId}");
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Performance Optimization - Pre-populate cache with a single O(N) pass
            // This eliminates redundant O(N) scene traversals for subsequent lookups.
            _objectCache.Clear();
            var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }
            // BOLT: Removed redundant _objectCache.Clear() to allow surgical lazy-loading to persist.
            // Unity's == null check in GetCachedObject handles destroyed objects safely.
            // BOLT: Removed redundant _objectCache.Clear() to allow surgical lazy-loading cache
            // to persist across multiple scenario updates for incremental performance.
            // BOLT: Clear dynamic caches at start of setup to avoid stale references.
            _objectCache.Clear();
            _controllerCache.Clear();

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

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            _objectCache.Clear();
            _controllerCache.Clear();

            _objectCache.Clear();
            _controllerCache.Clear();

            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Clear scene-specific caches to avoid stale references and memory leaks.
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Clear dynamic caches to avoid stale references across scenarios
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Pre-populate cache with a single O(N) pass to avoid repeated O(N) GameObject.Find calls.
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (go != null && !string.IsNullOrEmpty(go.name))
                {
                    // If multiple objects have the same name, the last one found wins,
                    // which matches the behavior of GameObject.Find.
                    _objectCache[go.name] = go;
                }
            }

            // Capture singleton instance to satisfy NRT flow analysis
            var campaignManager = CampaignManager.Instance;
            if (campaignManager != null)
            // NRT Pattern: Capture singleton property in local variable
            // Instantiate characters if not already in scene
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            if (CampaignManager.Instance.currentCampaignData?.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData?.characters != null)
            if (CampaignManager.Instance.currentCampaignData != null)
            var campaignManager = CampaignManager.Instance;
            if (campaignManager != null)
            {
                var data = campaignManager.currentCampaignData;
                if (data != null && data.characters != null)
                {
                    foreach (var charProfile in data.characters)
                    {
                        if (charProfile != null)
                        {
                            SpawnOrUpdateCharacter(charProfile);
                        }
            if (CampaignManager.Instance.currentCampaignData != null)
            // 🛡️ Sentinel: Capture singleton property to local for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            if (CampaignManager.Instance.currentCampaignData != null)
            if (CampaignManager.Instance?.currentCampaignData != null)
            _objectCache.Clear();
            _controllerCache.Clear();

            if (CampaignManager.Instance?.currentCampaignData?.characters != null)
            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                var campaignData = campaignManager.currentCampaignData;
                if (campaignData != null && campaignData.characters != null)
                {
                    foreach (var charProfile in campaignData.characters)
                    {
                        if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                    SpawnOrUpdateCharacter(charProfile);
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null)
                        SpawnOrUpdateCharacter(charProfile);
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                    if (charProfile != null)
                    {
                        SpawnOrUpdateCharacter(charProfile);
                    }
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
                    if (interaction != null) ApplyInteraction(interaction);
                    ApplyInteraction(interaction);
                ApplyInteraction(interaction);
                foreach (var interaction in scenario.interactiveObjects)
                {
                    if (interaction != null)
                        ApplyInteraction(interaction);
                    if (interaction != null) ApplyInteraction(interaction);
                    if (interaction != null)
                    {
                        ApplyInteraction(interaction);
                    }
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: O(1) exact match lookup from prefab cache.
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to O(N) partial match if exact name not found.
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Use O(1) prefab cache lookup instead of O(M) list search.
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                if (prefab == null && characterPrefabs != null)
                {
                    // Fallback to partial name matching if exact match not in cache.
                    prefab = characterPrefabs.Find(p => p.name.Contains(profile.name));
                }

                // BOLT: Optimized O(1) prefab lookup via dictionary
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to fuzzy search if exact name match isn't in dictionary
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                }

                // BOLT: Try O(1) prefab cache lookup first
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to O(M) linear search if not in dictionary (e.g. partial name match)
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab; // Cache the match
                }

                // BOLT: Use prefab lookup cache to avoid expensive O(P) list searches
                if (!_prefabLookupCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
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
                // BOLT: Optimized prefab lookup using O(1) dictionary with O(P) fallback for contains-logic
                GameObject prefab = null;
                if (_prefabLookupCache == null || !_prefabLookupCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                }

                // BOLT: O(1) prefab lookup
                // BOLT: Use prefab cache to avoid repeated string matching in the list
                GameObject prefab;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                }
                // BOLT: Use optimized prefab retrieval with O(1) cache lookup
                // BOLT: O(1) prefab lookup instead of O(M) list search
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
                {
                // BOLT: Optimized prefab lookup using O(1) dictionary instead of linear List.Find (O(M)).
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                if (prefab == null)
                {
                    // Fallback for partial name matches if exact match fails
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                }
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary instead of linear search
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback for partial name matching if exact match fails
                if (prefab == null && characterPrefabs != null)
                {
                    prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profile.name));
                }
                // BOLT: Optimized O(1) prefab lookup via dictionary cache
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback for partial name matches if exact match fails (maintaining existing behavior)
                // BOLT: Use O(1) prefab cache helper
                GameObject? prefab = GetPrefab(profile.name);

                GameObject prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                GameObject? prefab = null;
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Use O(1) prefab cache helper
                // BOLT: O(1) prefab lookup via dictionary
                if (_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                // BOLT: Use O(1) prefab cache lookup instead of O(M) linear Find
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback to partial name match only if exact match fails
                if (prefab == null && characterPrefabs != null)
                {
                    prefab = characterPrefabs.Find(p => p.name.Contains(profile.name));
                }
                // BOLT: Optimized prefab lookup using O(1) dictionary instead of O(M) List.Find
                GameObject prefab = null;

                // Try direct match first
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to fuzzy search if necessary, but keep it minimal
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
                GameObject prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                GameObject? prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object.
                    // BOLT: Immediately cache the newly instantiated object
                    // BOLT: Immediately cache the newly instantiated object to avoid subsequent searches
                    // BOLT: Immediately cache the newly instantiated object.
                    // BOLT: Immediately cache the newly instantiated object to prevent redundant scene searches
                    // BOLT: Immediately cache the newly instantiated object to resolve negative cache hits.
                    // BOLT: Immediately cache the newly instantiated object to prevent redundant searches
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
                // BOLT: O(1) controller lookup avoids redundant GetComponent calls
                var controller = GetCharacterController(characterObj);
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

            GameObject? target = GetCachedObject(interaction.objectId);
            // 🛡️ Sentinel: Prevent IDOR tampering with core architectural managers
            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked attempt to manipulate critical system object: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);
            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects using exact matching
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogError($"[Security] Blocked unauthorized interaction with system object: {interaction.objectId}");
                return;
            }

            GameObject target = GetCachedGameObject(interaction.objectId);
            // 🛡️ Sentinel: Security validation to prevent IDOR via external JSON mapping to core managers.
            string[] protectedManagers = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };
            if (System.Array.Exists(protectedManagers, m => m == interaction.objectId))
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt with protected manager: {interaction.objectId}");
                return;
            }

            // SECURITY: Prevent IDOR (Insecure Direct Object Reference) tampering with core systems.
            // Block direct interactions with architectural singletons.
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on core system: {interaction.objectId}");
                return;
            }

            if (interaction == null) return;
            GameObject? target = GetCachedObject(interaction.objectId);
            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked attempt to interact with critical system object: {interaction.objectId}");
                return;
            }

            // 🛡️ Sentinel: Validate that the requested object ID is not a restricted system object.
            // SECURITY: Prevents IDOR-like tampering where untrusted JSON data manipulates critical singletons.
            if (_restrictedSystemObjects.Contains(interaction.objectId))
            {
                Debug.LogWarning($"[Security] Attempted to apply interaction to restricted system object: {interaction.objectId}. Action blocked.");
                return;
            }

            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked interaction attempt on critical system object: {interaction.objectId}");
                return;
            }
            if (interaction == null) return;
            // 🛡️ Sentinel: Prevent IDOR vulnerabilities by blocking untrusted external data from targeting core architectural singletons.
            // SECURITY: Prevent IDOR tampering with core system managers via untrusted JSON data
            // 🛡️ Sentinel: Prevent IDOR by blocking manipulation of critical system managers
            if (interaction == null) return;
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR)
            // Block external data from manipulating core architectural managers.
            string[] protectedObjects = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };
            foreach (string protectedObj in protectedObjects)
            {
                if (interaction.objectId == protectedObj)
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                    return;
                }
            // SENTINEL: Prevent IDOR vulnerabilities by blocking untrusted external interactions
            // from manipulating core architectural singletons and managers.
            // SENTINEL: Prevent IDOR by blocking access to core architectural singletons.
            // External JSON data should not be able to manipulate these critical managers.
            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference)
            // Block external modification of core architectural singletons.
            // SECURITY: Prevent IDOR (Insecure Direct Object Reference). Block external JSON from manipulating core managers.
            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects.
            // External JSON could attempt to manipulate core managers by passing their exact names.
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogError($"[Security] Blocked unauthorized interaction with core system object: {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on protected system object: {interaction.objectId}");
                Debug.LogWarning("Security: Blocked unauthorized interaction with core system manager.");
                Debug.LogWarning($"[Security] Blocked unauthorized interaction with protected system object: {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized attempt to interact with protected core system: {interaction.objectId}");
                Debug.LogWarning($"Security Block: Unauthorized interaction attempt on protected object {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized attempt to manipulate protected object: {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on system object: {interaction.objectId}");
                return;
            }

            // SECURITY: Exact string matching to prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on critical system object: {interaction.objectId}");
                return;
            }

            // 🛡️ Sentinel: Security enhancement to prevent IDOR-like manipulation of core objects
            // Do not allow external JSON data to manipulate critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked attempt to manipulate protected object: {interaction.objectId}");
                return;
            }

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
            // BOLT: Clear caches on destroy to release Unity object references
            // BOLT: Clear all caches to release Unity engine object references
            // BOLT: Clear caches to release references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Explicitly clear the cache to release Unity engine object references
            if (_objectCache != null)
            {
                _objectCache.Clear();
            }
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}
