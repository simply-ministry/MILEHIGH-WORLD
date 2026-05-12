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
            }

            var obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null || !scenario.IsValid()) return;

            foreach (var interaction in scenario.interactiveObjects)
            {
                ApplyInteraction(interaction);
            }
        }

        public void SpawnCharacter(CharacterProfile profile)
        {
            if (profile == null || !profile.IsValid()) return;

            if (_prefabCache.TryGetValue(profile.name, out var prefab) && prefab != null)
            {
                GameObject instance = Instantiate(prefab, characterSpawnRoot);
                instance.name = profile.name;

                var controller = instance.GetComponent<CharacterControllerBase>();
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
        }
    }
}
