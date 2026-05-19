// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilehighWorld.Data
{
    public static class ValidationUtility
    {
        public static bool ValidateString(string fieldName, string value, int maxLength, bool required = true)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (required)
                {
                    Debug.LogError($"[Security] Validation failed: {fieldName} is missing.");
                    return false;
                }
                return true;
            }

            if (value.Length > maxLength)
            {
                Debug.LogError($"[Security] Validation failed: {fieldName} length {value.Length} exceeds {maxLength} characters.");
                return false;
            }

            return true;
        }

        public static bool ValidateFloat(string fieldName, float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < min || value > max)
            {
                Debug.LogError($"[Security] Validation failed: {fieldName} {value} is invalid or out of range [{min}, {max}]");
                return false;
            }
            return true;
        }
    }

    public enum LightingState { Day, Night, Dynamic }

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = "";
        public int systemParity;
        [UnityEngine.Range(0.0f, 1.0f)]
        public float voidSaturationLevel;

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("environment", environment, 128)) return false;
            if (!ValidationUtility.ValidateFloat("voidSaturationLevel", voidSaturationLevel, 0.0f, 1.0f)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        [UnityEngine.TextArea(3, 10)]
        public string behaviorScript = null!;

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("CharacterProfile.name", name, 128)) return false;
            if (!ValidationUtility.ValidateString("CharacterProfile.role", role, 128, false)) return false;

            if (traits != null)
            {
                if (traits.Length > 50)
                {
                    Debug.LogError($"[Security] Validation failed: Too many traits ({traits.Length}).");
                    return false;
                }
                foreach (var trait in traits)
                {
                    if (!ValidationUtility.ValidateString("trait", trait, 128, false)) return false;
                }
            }

            if (!ValidationUtility.ValidateString("behaviorScript", behaviorScript, 2048, false)) return false;
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

        public UnityEngine.Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("objectId", objectId, 128)) return false;
            if (!ValidationUtility.ValidateString("action", action, 128, false)) return false;

            if (float.IsNaN(floatValue) || float.IsInfinity(floatValue) ||
                float.IsNaN(x) || float.IsInfinity(x) ||
                float.IsNaN(y) || float.IsInfinity(y) ||
                float.IsNaN(z) || float.IsInfinity(z))
            {
                Debug.LogError($"[Security] Validation failed for '{objectId}': Numeric parameters contain NaN or Infinity.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker = null!;
        [UnityEngine.TextArea(2, 5)]
        public string text = null!;
        public string trigger = null!;

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("Dialogue.speaker", speaker, 128, false)) return false;
            if (!ValidationUtility.ValidateString("Dialogue.text", text, 2048, false)) return false;
            if (!ValidationUtility.ValidateString("Dialogue.trigger", trigger, 128, false)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId = null!;
        [UnityEngine.TextArea(2, 5)]
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("scenarioId", scenarioId, 128)) return false;
            if (!ValidationUtility.ValidateString("description", description, 1024, false)) return false;

            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 50)
                {
                    Debug.LogError($"[Security] Validation failed: too many interactive objects ({interactiveObjects.Count}).");
                    return false;
                }
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                if (dialogue.Count > 50)
                {
                    Debug.LogError($"[Security] Validation failed: too many dialogue lines ({dialogue.Count}).");
                    return false;
                }
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
        public string sceneId = null!;
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        public bool IsValid()
        {
            if (!ValidationUtility.ValidateString("sceneId", sceneId, 128)) return false;

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }
            if (characters.Count > 50)
            {
                Debug.LogError($"[Security] Game data validation failed: character count {characters.Count} exceeds 50.");
                return false;
            }
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (scenarios.Count > 50)
            {
                Debug.LogError($"[Security] Game data validation failed: scenario count {scenarios.Count} exceeds 50.");
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
