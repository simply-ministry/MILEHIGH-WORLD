namespace UnityEngine
{
    public class MonoBehaviour {
        public Coroutine StartCoroutine(System.Collections.IEnumerator routine) => null;
        public void StopCoroutine(Coroutine routine) {}
        public GameObject gameObject => new GameObject();
        public Transform transform => new Transform();
    }
    public class GameObject {
        public string name { get; set; }
        public bool activeInHierarchy => true;
        public Transform transform => new Transform();
        public void SetActive(bool active) {}
        public T GetComponent<T>() where T : class => null;
    }
    public class Transform {
        public Vector3 localScale { get; set; }
        public Vector3 position { get; set; }
        public Transform Find(string name) => null;
        public T GetComponent<T>() where T : class => null;
    }
    public struct Vector3 {
        public float x, y, z;
        public static Vector3 one => new Vector3 { x = 1, y = 1, z = 1 };
        public static Vector3 operator *(Vector3 a, float b) => a;
    }
    public class AudioSource {
        public void Play() {}
    }
    public class CanvasGroup {
        public float alpha { get; set; }
    }
    public class Coroutine {}
    public class WaitForSeconds {
        public WaitForSeconds(float f) {}
    }
    public class Animator {
        public void SetTrigger(string name) {}
    }
    public class Debug {
        public static void Log(string msg) {}
        public static void LogError(string msg) {}
    }
    public class TooltipAttribute : System.Attribute {
        public TooltipAttribute(string s) {}
    }
    public class HeaderAttribute : System.Attribute {
        public HeaderAttribute(string s) {}
    }
    public class Color {
        public Color(float r, float g, float b) {}
        public static Color white => new Color(1,1,1);
        public static Color cyan => new Color(0,1,1);
        public static Color black => new Color(0,0,0);
    }
    public static class ColorUtility {
        public static string ToHtmlStringRGB(Color c) => "";
    }
    public static class Mathf {
        public static int RoundToInt(float f) => (int)f;
        public static float Lerp(float a, float b, float t) => a;
        public static float Sin(float f) => 0;
        public const float PI = 3.14159f;
    }
    public static class Time {
        public static float deltaTime => 0.016f;
    }
    public static class Input {
        public static bool anyKeyDown => false;
    }
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour {
        public string text { get; set; }
        public int maxVisibleCharacters { get; set; }
        public UnityEngine.Color color { get; set; }
        public TMP_FontAsset fontMaterial { get; set; }
        public void ForceMeshUpdate() {}
        public TMP_TextInfo textInfo => new TMP_TextInfo();
    }
    public class TMP_FontAsset {
        public void SetFloat(int id, float f) {}
        public void SetColor(int id, UnityEngine.Color c) {}
    }
    public static class ShaderUtilities {
        public static int ID_OutlineWidth = 0;
        public static int ID_OutlineColor = 1;
    }
    public class TMP_TextInfo {
        public int characterCount => 0;
        public TMP_CharacterInfo[] characterInfo = new TMP_CharacterInfo[0];
    }
    public struct TMP_CharacterInfo {
        public char character;
    }
}
