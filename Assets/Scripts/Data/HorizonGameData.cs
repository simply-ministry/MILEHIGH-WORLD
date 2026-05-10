using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = "";
        public string environment = null!;
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Input validation for environment string length (DoS mitigation)
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: Environment name exceeds 128 characters.");
                return false;
            }

            // Void saturation must be within a safe 0.0 to 1.0 range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // 🛡️ Sentinel: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
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
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Invalid voidSaturationLevel detected: {voidSaturationLevel}. Must be between 0.0 and 1.0.");
                return false;
            }

            if (environment != null && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata environment string exceeds 128 characters.");
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
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
        /// 🛡️ Sentinel: Security validation for character profile data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Enforce string length limits to prevent memory exhaustion/DoS
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (role != null && role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
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
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 128) return false;
        public string name = "";
        public string role = "";
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = "";
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
        public string objectId = "";
        public string action = "";
        public string objectId = null!;
        public string action = null!;

        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public UnityEngine.Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(x, y, z);
        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId)) return false;
            return true;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            return true;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction objectId is null, empty or exceeds 128 characters.");
                return false;
            }
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
            if (speaker != null && speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
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
        public string scenarioId = "";
        public string description = "";
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();
        public List<ObjectInteraction> interactiveObjects = new();
        public List<Dialogue> dialogue = new();
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        /// <summary>
        /// 🛡️ Sentinel: Security validation for scenario data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Enforce string length limits
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            return true;
        }
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
        public string sceneId = "";
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();
        public List<CharacterProfile> characters = new();
        public List<SceneScenario> scenarios = new();
        public Metadata? metadata;
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();
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
            // SECURITY: Enforce global campaign limits to prevent resource exhaustion attacks
            if (sceneId != null && sceneId.Length > 128) return false;
            if (characters != null && characters.Count > 50) return false;
            if (scenarios != null && scenarios.Count > 100) return false;
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is null, empty or exceeds 128 characters.");
                return false;
            }

            if (metadata == null)
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
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata is missing.");
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

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
                Debug.LogError("[Security] Game data validation failed: Character profiles count invalid.");
                return false;
            }

            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios count invalid.");
                return false;
            }

            if (characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Too many characters defined (>50).");
                return false;
            }

            foreach (var charProfile in characters)
            {
                if (charProfile == null || !charProfile.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Too many scenarios defined (>100).");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }
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