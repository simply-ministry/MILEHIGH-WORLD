using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        [Header("Setup")]
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform characterSpawnRoot = null!;

        // 🛡️ Sentinel: Hardened blocklist to prevent Insecure Direct Object Reference (IDOR) attacks on critical system managers.
        // Uses OrdinalIgnoreCase for defense-in-depth against case-insensitive bypass attempts.
        private static readonly HashSet<string> ProtectedSystemObjects = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
        {
            "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager",
            "CombatManager", "GlobalResonanceManager", "BicameralBattleEngine",
            "SkyIxController", "CinematicController", "TimelineSimulationEngine",
            "AsyncSceneLoader", "OtisTerminal", "EndGameMultiFrontOrchestrator",
            "EndGameOrchestrationBridge", "LatticeSynchronizer", "RealityAnchor",
            "EventSystem", "Main Camera"
        };

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_ \t\(\)\-\.\[\]]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
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
        {
            var data = CampaignManager.Instance.currentCampaignData;
            if (data != null && data.scenarios != null && data.scenarios.Count > 0)
            {
                SetupScene(data.scenarios[0]);
            }
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;

            _objectCache.Clear();
            // ⚡ Bolt: Use FindObjectsByType with FindObjectsSortMode.None (Unity 2021.3+).
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
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
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    characterObj = UnityEngine.Object.Instantiate(prefab, characterSpawnRoot);
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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                return null;
            }

            // ⚡ Bolt: Fixed negative caching anti-pattern. Using TryGetValue and checking existence
            // even for null values prevents redundant GameObject.Find calls every frame.
            // ⚡ Bolt: Use TryGetValue to support negative caching (explicitly storing null in the cache).
            // This eliminates redundant expensive GameObject.Find calls for objects that are known to be missing.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // System.Object.ReferenceEquals is required here because Unity's '== null' check returns true
                // for destroyed native objects. A true null in the cache means we've searched and found nothing.
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If the object exists but its native representation is destroyed, we should re-find it.
                if (obj != null) return obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller) && controller != null)
            {
                return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            return controller;
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) by blocking critical system managers.
            // 🛡️ Sentinel: Consolidate security validation into a single, linear pipeline.
            if (interaction == null || string.IsNullOrWhiteSpace(interaction.objectId)) return;

            string cleanId = interaction.objectId.Trim();

            // IDOR check 1: Input name
            if (ProtectedSystemObjects.Contains(cleanId))
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) by blocking critical system managers.
            // Consolidate security validation into a single, linear pipeline to prevent NullReferenceException
            // (information disclosure) and IDOR attacks.
            if (interaction == null || string.IsNullOrWhiteSpace(interaction.objectId)) return;

            string objectId = interaction.objectId.Trim();
            if (ProtectedSystemObjects.Contains(objectId))
            {
                Debug.LogError($"[Security] Blocked unauthorized interaction attempt to system object: {objectId}");
                return;
            }

            GameObject? target = GetCachedObject(objectId);
            if (target != null)
            {
                // IDOR check 2: Resolved object name (Defense-in-depth)
                string targetName = target.name.Trim();
                if (ProtectedSystemObjects.Contains(targetName))
                {
                // 🛡️ Sentinel: Double validation - check the resolved object name against the blocklist
                // to prevent potential bypasses if the object was retrieved via a different alias or path
                // or if it resides in a nested hierarchy (defense-in-depth).
                string targetName = target.name.Trim();
                if (ProtectedSystemObjects.Contains(targetName))
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt to resolved system object: {targetName}");
                if (ProtectedSystemObjects.Contains(target.name.Trim()))
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt to resolved system object: {target.name}");
            // 🛡️ Sentinel: Consolidate security validation into a single, linear pipeline.
            // Prevents NullReferenceException (information disclosure) and IDOR attacks.
            if (interaction == null || string.IsNullOrWhiteSpace(interaction.objectId)) return;

            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) by sanitizing untrusted external object IDs.
            // Block critical system managers and architectural singletons from being manipulated via external data.
            // Trim input to thwart bypasses using leading/trailing whitespace.
            string cleanId = interaction.objectId.Trim();
            if (ProtectedSystemObjects.Contains(cleanId))
            {
                Debug.LogError($"[Security] Blocked unauthorized interaction attempt to system object: {cleanId}");
                return;
            }

            GameObject? target = GetCachedObject(cleanId);
            if (target != null)
            {
                // 🛡️ Sentinel: Double validation - check the resolved object name against the blocklist
                if (ProtectedSystemObjects.Contains(target.name.Trim()))
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt to resolved system object: {target.name}");
                // as defense-in-depth against path-based or hierarchy-based bypasses (e.g. "/CampaignManager").
                string targetName = target.name.Trim();
                if (ProtectedSystemObjects.Contains(targetName))
                {
                    Debug.LogError($"[Security] Blocked resolved interaction to protected system object: {targetName}");
                    return;
                }

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
    }
}
