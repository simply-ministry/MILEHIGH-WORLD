using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = "";
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized metadata meets business constraints.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            // SECURITY: Input validation for environment string length (DoS mitigation)
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: Environment name exceeds 128 characters.");
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
        /// 🛡️ Sentinel: Security validation for character profile data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Implement resource exhaustion protection (DoS prevention)
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] Character name is invalid or too long.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] Character role exceeds 128 characters.");
                return false;
            }
            if (behaviorScript != null && behaviorScript.Length > 128)
            {
                Debug.LogError("[Security] Character behaviorScript exceeds 128 characters.");
                return false;
            }
            if (traits != null && traits.Length > 20)
            {
                Debug.LogError("[Security] Character traits count exceeds limit.");
                return false;
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

        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        /// <summary>
        /// 🛡️ Sentinel: Security validation for object interaction data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction objectId is invalid or too long.");
                return false;
            }
            if (action != null && action.Length > 128)
            {
                Debug.LogError("[Security] Interaction action exceeds 128 characters.");
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
        /// 🛡️ Sentinel: Security validation for dialogue data.
        /// </summary>
        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 128)
            {
                Debug.LogError("[Security] Dialogue speaker name exceeds 128 characters.");
                return false;
            }
            if (string.IsNullOrEmpty(text) || text.Length > 2048)
            {
                Debug.LogError("[Security] Dialogue text is empty or exceeds 2048 characters.");
                return false;
            }
            if (trigger != null && trigger.Length > 128)
            {
                Debug.LogError("[Security] Dialogue trigger exceeds 128 characters.");
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
        /// 🛡️ Sentinel: Security validation for scenario data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] ScenarioId is invalid or too long.");
                return false;
            }
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
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId = "";
        public Metadata metadata = new Metadata();
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is invalid or too long.");
                return false;
            }
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Character count is invalid.");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Scenario count is invalid.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            return true;
        }
    }
}
