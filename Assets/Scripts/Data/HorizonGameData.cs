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
        public LightingState lighting;
        public string environment = null!;
        public int systemParity;
        public float voidSaturationLevel;

        public bool IsValid()
        {
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (string.IsNullOrEmpty(environment))
            {
                Debug.LogError("[Security] Metadata validation failed: environment is missing.");
                return false;
            }
            if (environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: Environment name exceeds 128 characters.");
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
    public class SceneScenario
    {
        public string name = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (interactiveObjects == null) return false;
            foreach (var interaction in interactiveObjects)
            {
                if (interaction == null || !interaction.IsValid()) return false;
            }
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
            if (characters == null) return false;
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }
            if (scenarios == null) return false;
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }
            return true;
        }
    }
}
