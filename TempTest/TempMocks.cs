using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object {
        public string name = "";
        public static void Destroy(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
        public static bool operator ==(Object? x, Object? y) => true;
        public static bool operator !=(Object? x, Object? y) => false;
        public override bool Equals(object? obj) => true;
        public override int GetHashCode() => 0;
    }
    public class GameObject : Object {
        public Transform transform { get; } = new Transform();
        public bool activeSelf { get; set; }
        public void SetActive(bool active) { activeSelf = active; }
        public T GetComponent<T>() where T : Component, new() => new T();
        public static GameObject Find(string name) => new GameObject();
        public static GameObject[] FindObjectsByType<T>(System.Enum sortMode) => new GameObject[0];
    }
    public class Component : Object {
        public GameObject gameObject { get; } = new GameObject();
        public Transform transform => gameObject.transform;
    }
    public class MonoBehaviour : Component {
        public Coroutine StartCoroutine(System.Collections.IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine? routine) {}
        public void StopCoroutine(System.Collections.IEnumerator routine) {}
    }
    public class Transform : Component {
        public Vector3 localScale { get; set; } = Vector3.one;
        public Vector3 position { get; set; } = new Vector3();
        public Transform Find(string name) => this;
    }
    public struct Vector3 {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a;
    }
    public class AudioSource : Component {
        public void Play() {}
    }
    public class Coroutine {}
    public class WaitForSeconds { public WaitForSeconds(float f) {} }
    public class Debug {
        public static void Log(object m) {}
        public static void LogError(object m) {}
        public static void LogWarning(object m) {}
    }
    public class Input {
        public static bool anyKeyDown => false;
    }
    public class Time {
        public static float deltaTime => 0.02f;
    }
    public class Color {
        public float r, g, b, a;
        public Color(float r, float g, float b, float a = 1) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static Color white => new Color(1, 1, 1);
        public static Color cyan => new Color(0, 1, 1);
    }
    public class HeaderAttribute : System.Attribute { public HeaderAttribute(string header) {} }
    public class TooltipAttribute : System.Attribute { public TooltipAttribute(string tooltip) {} }
    public class ScriptableObject : Object {
        public static T CreateInstance<T>() where T : ScriptableObject, new() => new T();
    }
    public class Animator : Component {
        public void SetTrigger(string name) {}
    }
    public enum FindObjectsSortMode { None }
}

namespace UnityEngine.Serialization {
    public class FormerlySerializedAsAttribute : System.Attribute {
        public FormerlySerializedAsAttribute(string name) {}
    }
}

namespace TMPro {
    public class TextMeshProUGUI : UnityEngine.Component {
        public string text { get; set; } = "";
        public int maxVisibleCharacters { get; set; }
        public UnityEngine.Color color { get; set; } = UnityEngine.Color.white;
        public TMP_TextInfo textInfo { get; } = new TMP_TextInfo();
        public void ForceMeshUpdate() {}
    }
    public class TMP_TextInfo {
        public int characterCount { get; set; }
        public TMP_CharacterInfo[] characterInfo { get; set; } = new TMP_CharacterInfo[0];
    }
    public struct TMP_CharacterInfo {
        public char character { get; set; }
    }
}

namespace UnityEditor {
    public class AssetDatabase {
        public static void CreateAsset(UnityEngine.Object asset, string path) {}
        public static void SaveAssets() {}
        public static T? LoadAssetAtPath<T>(string path) where T : UnityEngine.Object => null;
    }
}
