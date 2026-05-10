using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = null!;
        public int systemParity;
        public float voidSaturationLevel;

        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
        public bool IsValid()
        {
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f) return false;
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128) return false;
        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment string is too long.");
                return false;
            }
            return true;
       }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;

        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual character data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Implement resource exhaustion protection (DoS prevention)
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        public string objectId = null!;
        public string action = null!;

        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(text)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;
        public List<Dialogue> dialogue = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;
            if (interactiveObjects != null)
            {
                foreach (var i in interactiveObjects) if (i == null || !i.IsValid()) return false;
            }
            if (dialogue != null)
            {
                foreach (var d in dialogue) if (d == null || !d.IsValid()) return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual scenarios.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            return true;
        }
        public string scenarioId = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;
        public List<Dialogue> dialogue = null!;
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = null!;
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = null!;
        public List<SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0 || characters.Count > 50) return false;
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100) return false;
            foreach (var c in characters) if (c == null || !c.IsValid()) return false;
            foreach (var s in scenarios) if (s == null || !s.IsValid()) return false;
        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// Validates the deserialized game data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = null!;
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = null!;
        public List<SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0) return false;
            if (scenarios == null || scenarios.Count == 0) return false;
            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is missing.");
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }

            if (scenarios == null)
            {
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            return true;
        }
    }
}