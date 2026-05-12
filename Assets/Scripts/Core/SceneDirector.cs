using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform characterSpawnRoot = null!;

        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private static readonly Regex _nameValidator = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\_\.\/\[\]]+$", RegexOptions.Compiled);

        private void Start()
        {
            InitializePrefabCache();
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs == null) return;
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null)
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName) || objectName.Length > 128 || !_nameValidator.IsMatch(objectName))
            {
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out var cachedObj))
            {
                if (System.Object.ReferenceEquals(cachedObj, null)) return null;
                if (cachedObj != null) return cachedObj;
        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and linear searches.
        // We use nullable types to support negative caching (storing a true null for missing objects).
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel: Regex for whitelisting safe object names to prevent DoS via expensive GameObject.Find operations.
        private static readonly Regex _nameWhitelist = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\.\/\[\]]+$", RegexOptions.Compiled);

        private void Start()
        {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }

            var obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null || !scenario.IsValid()) return;

            foreach (var interaction in scenario.interactiveObjects)
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear scene-specific caches at start of setup to avoid stale references.
            _objectCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
                }
            }
        }

        public void SpawnCharacter(CharacterProfile profile)
        {
            if (profile == null || !profile.IsValid()) return;
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (_prefabCache.TryGetValue(profile.name, out var prefab) && prefab != null)
            {
                GameObject instance = Instantiate(prefab, characterSpawnRoot);
                instance.name = profile.name;

                var controller = instance.GetComponent<CharacterControllerBase>();
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
                    _controllerCache[instance.GetInstanceID()] = controller;
                }
            }
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            // 🛡️ Sentinel: Block manipulation of core managers via IDOR
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked interaction with core manager: {interaction.objectId}");
                return;
            }

            var target = GetCachedObject(interaction.objectId);
            if (target == null) return;

            switch (interaction.action)
            {
                case "SetPosition":
                    if (interaction.isVector) target.transform.position = interaction.GetVectorValue();
                    break;
                case "SetActive":
                    target.SetActive(interaction.floatValue > 0);
                    break;
            }
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference) tampering with core systems.
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on core system: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);

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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: DoS Mitigation - Enforce length limit and character whitelist on object names.
            if (objectName.Length > 128 || !_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching using ReferenceEquals.
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                if (ReferenceEquals(prefab, null)) return null;
                if (prefab != null) return prefab;
            }

            prefab = characterPrefabs?.Find(p => p != null && (p.name == profileName || p.name.Contains(profileName)));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            int id = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(id, out CharacterControllerBase? controller))
            {
                return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[id] = controller;
            return controller;
        }
    }
}
