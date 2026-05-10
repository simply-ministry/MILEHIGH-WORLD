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
        public string environment = "";
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// Enforces resource limits on string lengths to prevent DoS.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Limit environment string length to prevent DoS via memory exhaustion
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment name is missing or too long (max 128).");
                return false;
            }

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
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
        public string name = "";
        public string role = "";
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = "";

        /// <summary>
        /// 🛡️ Sentinel: Validates character profile data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64)
            {
                Debug.LogError("[Security] Character validation failed: Name is missing or exceeds 64 characters.");
                return false;
            }
            if (string.IsNullOrEmpty(role) || role.Length > 64)
            {
                Debug.LogError("[Security] Character validation failed: Role is missing or exceeds 64 characters.");
                return false;
            }
            // SECURITY: behaviorScript can be long (it's code) but we still enforce a maximum for DoS prevention.
            if (string.IsNullOrEmpty(behaviorScript) || behaviorScript.Length > 2048)
            {
                Debug.LogError("[Security] Character validation failed: behaviorScript is missing or too long (max 2048).");
                return false;
            }

            // SECURITY: Limit traits collection size and individual trait lengths
            if (traits == null || traits.Length > 10)
            {
                Debug.LogError("[Security] Character validation failed: traits collection is null or exceeds 10 items.");
                return false;
            }
            foreach (var trait in traits)
            {
                if (string.IsNullOrEmpty(trait) || trait.Length > 64)
                {
                    Debug.LogError("[Security] Character validation failed: Individual trait is invalid or exceeds 64 characters.");
                    return false;
                }
            }

            return true;
        }
    }

    [Serializable]
    public class ObjectInteraction
    {
        public string objectId = "";
        public string action = "";
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
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction validation failed: objectId is missing or too long (max 128).");
                return false;
            }
            if (string.IsNullOrEmpty(action) || action.Length > 128)
            {
                Debug.LogError("[Security] Interaction validation failed: action is missing or too long (max 128).");
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker = "";
        public string text = "";
        public string trigger = "";

        /// <summary>
        /// 🛡️ Sentinel: Validates dialogue data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 64)
            {
                Debug.LogError("[Security] Dialogue validation failed: speaker is missing or too long (max 64).");
                return false;
            }
            if (string.IsNullOrEmpty(text) || text.Length > 1024)
            {
                Debug.LogError("[Security] Dialogue validation failed: text is missing or too long (max 1024).");
                return false;
            }
            if (trigger != null && trigger.Length > 128)
            {
                Debug.LogError("[Security] Dialogue validation failed: trigger exceeds 128 characters.");
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class SceneScenario
    {
        public string scenarioId = "";
        public string description = "";
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();

        /// <summary>
        /// 🛡️ Sentinel: Validates scene scenario data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] Scenario validation failed: scenarioId is missing or too long (max 128).");
                return false;
            }

            // SECURITY: Limit collection sizes to prevent resource exhaustion
            if (interactiveObjects == null || interactiveObjects.Count > 50)
            {
                Debug.LogError("[Security] Scenario validation failed: interactiveObjects collection is null or too large (max 50).");
                return false;
            }
            foreach (var obj in interactiveObjects)
            {
                if (obj == null || !obj.IsValid()) return false;
            }

            if (dialogue == null || dialogue.Count > 100)
            {
                Debug.LogError("[Security] Scenario validation failed: dialogue collection is null or too large (max 100).");
                return false;
            }
            foreach (var d in dialogue)
            {
                if (d == null || !d.IsValid()) return false;
            }

            return true;
        }
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId = "";
        public Metadata metadata = new Metadata();
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        /// <summary>
        /// 🛡️ Sentinel: Performs hierarchical integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is missing or too long (max 128).");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }

            // SECURITY: Enforce collection limits for DoS protection
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character count (must be between 1 and 50).");
                return false;
            }
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count (must be between 1 and 50).");
                return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            return true;
        }
    }
}
