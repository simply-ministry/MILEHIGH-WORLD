using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object {
        public string name;
    }
    public class GameObject : Object {
        public Transform transform = new Transform();
        public T GetComponent<T>() where T : class => null;
        public static GameObject Find(string name) => null;
        public GameObject(string name) { this.name = name; }
        public GameObject() {}
        public T AddComponent<T>() where T : class => null;
    }
    public class Transform : Object {
        public Vector3 position;
        public Vector3 localScale;
    }
    public struct Vector3 {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one = new Vector3(1,1,1);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x*b, a.y*b, a.z*b);
    }
    public class MonoBehaviour : Object {
        public GameObject gameObject = new GameObject();
        public Transform transform => gameObject.transform;
        public static void DontDestroyOnLoad(Object obj) {}
        public static void Destroy(Object obj) {}
        public static T FindObjectOfType<T>() where T : class => null;
        public static T Instantiate<T>(T original, Transform parent) where T : Object => original;
    }
    public class ScriptableObject : Object {
        public static T CreateInstance<T>() where T : ScriptableObject, new() => new T();
    }
    public class Debug {
        public static void Log(object message) {}
        public static void LogError(object message) {}
    }
    public class JsonUtility {
        public static T FromJson<T>(string json) => default;
    }
    public class Application {
        public static string dataPath = "";
        public static string streamingAssetsPath = "";
    }
    public class Mathf {
        public static float Clamp01(float value) => value;
    }
    public class TextAreaAttribute : Attribute {
        public TextAreaAttribute(int min, int max) {}
    }
    public class SerializableAttribute : Attribute {}
    public class CreateAssetMenuAttribute : Attribute {
        public string fileName;
        public string menuName;
    }
}

namespace UnityEditor
{
    using UnityEngine;
    public class EditorWindow : MonoBehaviour {}
    public class MenuItemAttribute : Attribute {
        public MenuItemAttribute(string path) {}
    }
    public class AssetDatabase {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(Object asset, string path) {}
        public static void SaveAssets() {}
        public static void Refresh() {}
    }
}
