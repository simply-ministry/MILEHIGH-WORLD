using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState
    {
        Day,
        Night,
        Dynamic
    }

    [Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment;
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// Validates metadata integrity and safety bounds.
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints and prevents DoS.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment name is missing or too long (max 128).");
                return false;
            }

        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // Void saturation must be within a safe 0.0 to 1.0 range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class CharacterProfile
    {
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;

        /// <summary>
        /// 🛡️ Sentinel: Perform input validation on character data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64)
            {
                Debug.LogError("[Security] Character validation failed: Name is missing or exceeds 64 characters.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] Character validation failed: Role exceeds 128 characters.");
                return false;
            }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 64) return false;
            if (behaviorScript != null && behaviorScript.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            return true;
        }
    }

    [Serializable]
    public class ObjectInteraction
    {
        public string objectId;
        public string action;

        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker;
        public string text;
        public string trigger;

        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            return true;
        }
    }

    [Serializable]
    public class SceneScenario
    {
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;

            if (interactiveObjects != null)
            {
                foreach (var obj in interactiveObjects) if (!obj.IsValid()) return false;
            }
            if (dialogue != null)
            {
                foreach (var d in dialogue) if (!d.IsValid()) return false;
            }

            return true;
        }
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId;
        public Metadata metadata;
        public List<CharacterProfile> characters;
        public List<SceneScenario> scenarios;

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// Validates the deserialized game data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (!metadata.IsValid()) return false;

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            // SECURITY: Prevent resource exhaustion attacks by limiting the number of characters and scenarios.
            if (characters == null || characters.Count == 0 || characters.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character profile count (must be between 1 and 100).");
                return false;
            }

            foreach (var character in characters)
            {
                if (!character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count (must be between 1 and 50).");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Character profiles count out of bounds.");
                return false;
            }

            foreach (var character in characters)
            {
                if (!character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios count out of bounds.");
                return false;
            }

            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            foreach (var scenario in scenarios)
            {
                if (!scenario.IsValid()) return false;
            }

            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
                return false;
            }

            foreach (var charProfile in characters)
            {
                if (charProfile == null || !charProfile.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid character profile detected.");
                    return false;
                }
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid scenario detected.");
                    return false;
                }
            }

            return true;
        }
    }
}
