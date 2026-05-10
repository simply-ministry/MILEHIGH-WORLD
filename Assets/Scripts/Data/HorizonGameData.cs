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
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints and prevents resource exhaustion.
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// Validates metadata integrity and safety bounds.
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints and prevents DoS.
        /// </summary>
        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// Validates metadata integrity and safety bounds.
        public bool IsValid()
        {
            if (environment != null && environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: environment string exceeds 128 characters.");
            // SECURITY: Enforce resource limits on string lengths to prevent DoS
            if (environment != null && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment string exceeds 128 characters.");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: environment is null or exceeds 128 characters.");
                return false;
            }

            // Void saturation must be within a safe 0.0 to 1.0 range.
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
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
                Debug.LogError($"[Security] Invalid voidSaturationLevel detected: {voidSaturationLevel}. Must be between 0.0 and 1.0.");
                return false;
            }

            // SECURITY: Limit environment string length to prevent DoS via memory exhaustion
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata environment name exceeds maximum length (128).");
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
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;

        /// <summary>
        /// 🛡️ Sentinel: Validates character profile data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (role != null && role.Length > 64) return false;
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (role != null && role.Length > 64) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 64) return false;
            if (string.IsNullOrEmpty(behaviorScript) || behaviorScript.Length > 64) return false;
            if (traits == null || traits.Length > 10) return false;

            foreach (var trait in traits)
            {
                if (string.IsNullOrEmpty(trait) || trait.Length > 64) return false;
            }

            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            // SECURITY: behaviorScript can be long (it's code) but we still enforce a maximum for DoS prevention.
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 64) return false;

            // SECURITY: Limit traits collection size
            if (traits != null && traits.Length > 10) return false;

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

        /// <summary>
        /// 🛡️ Sentinel: Validates object interaction data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            if (action != null && action.Length > 64) return false;
        public bool IsValid()
        {
            if (objectId != null && objectId.Length > 64) return false;
            if (action != null && action.Length > 64) return false;
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 64) return false;
            if (string.IsNullOrEmpty(action) || action.Length > 64) return false;
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker;
        public string text;
        public string trigger;

        /// <summary>
        /// 🛡️ Sentinel: Validates dialogue data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 64) return false;
            if (text != null && text.Length > 1024) return false;
            if (trigger != null && trigger.Length > 64) return false;
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            if (trigger != null && trigger.Length > 64) return false;
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64) return false;
            if (!string.IsNullOrEmpty(text) && text.Length > 1024) return false;
            if (speaker != null && speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            return true;
        }
    }

    [Serializable]
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

        /// <summary>
        /// 🛡️ Sentinel: Validates scene scenario data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 50) return false;
                foreach (var obj in interactiveObjects) if (obj != null && !obj.IsValid()) return false;
        public bool IsValid()
        {
            if (scenarioId != null && scenarioId.Length > 128) return false;
            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 50) return false;
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects == null || interactiveObjects.Count > 50) return false;
            if (dialogue == null || dialogue.Count > 50) return false;

            foreach (var obj in interactiveObjects) if (obj == null || !obj.IsValid()) return false;
            foreach (var d in dialogue) if (d == null || !d.IsValid()) return false;

            // SECURITY: Limit collection sizes to prevent resource exhaustion
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;

            if (dialogue != null)
            {
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;

            if (interactiveObjects != null)
            {
                foreach (var obj in interactiveObjects) if (!obj.IsValid()) return false;
            }
            if (dialogue != null)
            {
                if (dialogue.Count > 50) return false;
                foreach (var d in dialogue) if (d != null && !d.IsValid()) return false;
            }
                foreach (var d in dialogue) if (!d.IsValid()) return false;
            }
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
        /// 🛡️ Sentinel: Performs hierarchical integrity and security validation on the entire dataset.
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// Validates the deserialized game data for security and integrity.
        /// </summary>
        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// Validates the deserialized game data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Characters count out of range (1-50).");
                Debug.LogError("[Security] Game data validation failed: Invalid character profile count.");
                return false;
            }
            foreach (var character in characters)
            {
                if (character != null && !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios count out of range (1-100).");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scene scenarios defined.");
                return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario != null && !scenario.IsValid()) return false;
            foreach (var character in characters)
            {
                if (!character.IsValid()) return false;
            }

            if (scenarios != null)
            {
                if (scenarios.Count > 100) return false;
                foreach (var scenario in scenarios)
                {
                    if (!scenario.IsValid()) return false;
                }
            }
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 64)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is missing or too long.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }

            // SECURITY: Enforce collection limits for DoS protection
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character count.");
                return false;
            }

            if (scenarios == null || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count.");
                return false;
            }

                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (!metadata.IsValid()) return false;

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character count.");
                return false;
            }

            foreach (var character in characters)
            {
                if (character == null || !character.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid character profile.");
                    return false;
                }
            // SECURITY: Prevent resource exhaustion attacks by limiting the number of characters and scenarios.
            if (characters == null || characters.Count == 0 || characters.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character profile count (must be between 1 and 100).");
                return false;
            }

            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is missing.");
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
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count.");
                return false;
            }

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
                    Debug.LogError($"[Security] Game data validation failed: Invalid character profile {charProfile?.name}");
                    Debug.LogError("[Security] Game data validation failed: Invalid character profile detected.");
                    return false;
                }
            }

            if (scenarios != null)
            {
                foreach (var scenario in scenarios)
                {
                    if (scenario == null || !scenario.IsValid())
                    {
                        Debug.LogError($"[Security] Game data validation failed: Invalid scenario {scenario?.scenarioId}");
                        return false;
                    }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid scene scenario.");
                    Debug.LogError("[Security] Game data validation failed: Invalid scenario detected.");
                    return false;
                }
            }

            return true;
        }
    }
}
