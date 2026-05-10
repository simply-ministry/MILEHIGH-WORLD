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
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f) return false;
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128) return false;
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
            return true;
        }
    }
}