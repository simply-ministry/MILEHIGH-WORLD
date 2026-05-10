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

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = null!;
        public int systemParity;
        public float voidSaturationLevel;

        private const int MAX_STRING_LENGTH = 128;

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// </summary>
        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure void saturation is within a safe 0.0 to 1.0 range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            // SECURITY: Prevent resource exhaustion by limiting string length
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment string is too long.");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            if (environment != null && environment.Length > MAX_STRING_LENGTH)
            {
                Debug.LogError($"[Security] Metadata validation failed: environment string exceeds {MAX_STRING_LENGTH} characters.");
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
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > MAX_STRING_LENGTH) return false;
            if (role != null && role.Length > MAX_STRING_LENGTH) return false;
            if (behaviorScript != null && behaviorScript.Length > MAX_STRING_LENGTH) return false;
            return true;
        }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] Character name is null, empty or exceeds 128 characters.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] Character role exceeds 128 characters.");
                return false;
            }
            if (behaviorScript != null && behaviorScript.Length > 1024) // Allowing more for script, but still limited
            {
                Debug.LogError("[Security] Character behaviorScript exceeds 1024 characters.");
                return false;
            }
            return true;
        }
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;
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

        public UnityEngine.Vector3 GetVectorValue()
        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > MAX_STRING_LENGTH) return false;
            if (action != null && action.Length > MAX_STRING_LENGTH) return false;
            return true;
        }

        public Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction objectId is null, empty or exceeds 128 characters.");
                return false;
            }
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker;
        public string text;
        public string trigger;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (speaker != null && speaker.Length > MAX_STRING_LENGTH) return false;
            if (trigger != null && trigger.Length > MAX_STRING_LENGTH) return false;
            // Dialogue text can be longer, but we might want to cap it too for DoS
            if (text != null && text.Length > 2048) return false;
            return true;
        }
        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
            return true;
        }
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
            return true;
        }
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;
        public List<Dialogue> dialogue = null!;
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > MAX_STRING_LENGTH) return false;

            if (interactiveObjects != null)
            {
                foreach (var obj in interactiveObjects) if (!obj.IsValid()) return false;
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] ScenarioId is null, empty or exceeds 128 characters.");
                return false;
            }

            if (interactiveObjects != null)
            {
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                foreach (var d in dialogue) if (!d.IsValid()) return false;
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
            }

            return true;
        }
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 100) return false;
                foreach (var io in interactiveObjects) if (io == null || !io.IsValid()) return false;
            }
            if (dialogue != null)
            {
                if (dialogue.Count > 200) return false;
                foreach (var d in dialogue) if (d == null || !d.IsValid()) return false;
            }
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

        private const int MAX_STRING_LENGTH = 128;
        private const int MAX_CHARACTERS = 50;
        private const int MAX_SCENARIOS = 100;

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > MAX_STRING_LENGTH)
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is null, empty or exceeds 128 characters.");
                return false;
            }

            if (metadata == null)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata is missing.");
                Debug.LogError("[Security] Game data validation failed: Invalid sceneId.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > MAX_CHARACTERS)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Scenarios list is missing.");
                return false;
            }
                Debug.LogError($"[Security] Game data validation failed: Character count {characters?.Count} out of bounds.");
                return false;
            }

            foreach (var character in characters)
            {
                if (!character.IsValid())
                {
                    Debug.LogError($"[Security] Game data validation failed: Invalid character profile {character.name}.");
                    return false;
                }
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > MAX_SCENARIOS)
            {
                Debug.LogError($"[Security] Game data validation failed: Scenario count {scenarios?.Count} out of bounds.");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (!scenario.IsValid())
                {
                    Debug.LogError($"[Security] Game data validation failed: Invalid scenario {scenario.scenarioId}.");
                    return false;
                }
            }
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Character profiles count invalid.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null) return false;

            return true;
        }
    }
}
