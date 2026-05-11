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

    [System.Serializable]
    public class Metadata
    {
        [Tooltip("The lighting state for the scene.")]
        public LightingState lighting;
        [Tooltip("The name of the environment.")]
        public string environment = null!;
        [Tooltip("System parity level for synchronization.")]
        public int systemParity;
        [Tooltip("The saturation level of the Void effect (0 to 1).")]
        [Range(0.0f, 1.0f)]
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized metadata meets business constraints and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment is missing or too long.");
                return false;
            }

            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        [Tooltip("The name of the character.")]
        public string name = null!;
        [Tooltip("The role or class of the character.")]
        public string role = null!;
        [Tooltip("A list of traits defining the character's abilities or attributes.")]
        public string[] traits = null!;
        [Tooltip("The script defining the character's AI behavior.")]
        [TextArea(3, 10)]
        public string behaviorScript = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            // Palette: Increased behaviorScript limit to support richer AI descriptions while maintaining safety.
            // BOLT: Increased behaviorScript length to 2048 to support complex AI behaviors
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        [Tooltip("The ID of the object to interact with.")]
        public string objectId = null!;
        [Tooltip("The action to perform on the object.")]
        public string action = null!;

        [Tooltip("Whether the interaction uses a vector or a float value.")]
        public bool isVector;
        [Tooltip("The float value for the interaction.")]
        public float floatValue;
        [Tooltip("The X component of the vector value.")]
        public float x;
        [Tooltip("The Y component of the vector value.")]
        public float y;
        [Tooltip("The Z component of the vector value.")]
        public float z;

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            if (string.IsNullOrEmpty(action) || action.Length > 128) return false;
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        [Tooltip("The name of the speaker.")]
        public string speaker = null!;
        [Tooltip("The dialogue text content.")]
        [TextArea(2, 5)]
        public string text = null!;
        [Tooltip("The event trigger for this dialogue.")]
        public string trigger = null!;

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        [Tooltip("Unique ID for the scenario.")]
        public string scenarioId = null!;
        [Tooltip("Description of the scenario.")]
        [TextArea(2, 5)]
        public string description = null!;
        [Tooltip("List of interactive objects in this scenario.")]
        public List<ObjectInteraction> interactiveObjects = null!;
        [Tooltip("List of dialogue lines in this scenario.")]
        public List<Dialogue> dialogue = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;

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
    }

    [System.Serializable]
    public class HorizonGameData
    {
        [Tooltip("Unique ID for the scene.")]
        public string sceneId = null!;
        [Tooltip("Metadata for the scene.")]
        public Metadata metadata = null!;
        [Tooltip("List of character profiles in this campaign.")]
        public List<CharacterProfile> characters = null!;
        [Tooltip("List of scenarios in this campaign.")]
        public List<SceneScenario> scenarios = null!;

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset after deserialization.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined or too many scenarios.");
                return false;
            }

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
