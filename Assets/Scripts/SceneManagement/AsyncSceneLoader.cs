using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        // 🛡️ Sentinel: Safe scene name regex whitelist to prevent Path Traversal and Scene Injection.
        private static readonly Regex SafeSceneNameRegex = new Regex(@"^[a-zA-Z0-9_\-/]+$", RegexOptions.Compiled);
        private const int MaxSceneNameLength = 128;

        public void LoadScene(string sceneName)
        {
            // 🛡️ Sentinel: Input validation to prevent path traversal / scene injection.
            if (string.IsNullOrWhiteSpace(sceneName) || sceneName.Length > MaxSceneNameLength)
            {
                Debug.LogError("[Security] Invalid scene loading request: name is null, empty, or exceeds maximum length.");
                return;
            }

            if (!SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError($"[Security] Blocked invalid scene load attempt with potentially unsafe characters: {sceneName}");
                return;
            }

            StartCoroutine(LoadAsync(sceneName));
        }

        private IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
