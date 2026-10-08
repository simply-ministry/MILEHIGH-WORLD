using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        // 🛡️ Sentinel: Whitelist for safe scene names to prevent path traversal or parameter injection
        private static readonly Regex SafeSceneNameRegex = new Regex(@"^[a-zA-Z0-9_\-/]+$", RegexOptions.Compiled);
        private const int MaxSceneNameLength = 128;

        public void LoadScene(string sceneName)
        {
            // 🛡️ Sentinel: Input validation against path traversal and null/excessive input injection
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[Security] LoadScene rejected: Scene name cannot be null or whitespace.");
                return;
            }

            if (sceneName.Length > MaxSceneNameLength)
            {
                Debug.LogError("[Security] LoadScene rejected: Scene name exceeds maximum allowed length.");
                return;
            }

            if (!SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError($"[Security] LoadScene rejected: Invalid characters in scene name '{sceneName}'.");
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
