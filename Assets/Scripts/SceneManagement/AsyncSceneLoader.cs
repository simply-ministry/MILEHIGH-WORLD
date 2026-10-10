using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        private static readonly Regex SafeSceneNameRegex = new Regex(@"^[a-zA-Z0-9_\-/]+$", RegexOptions.Compiled);

        public void LoadScene(string sceneName)
        {
            // 🛡️ Sentinel: Validate sceneName to prevent path traversal and scene parameter injection attacks.
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[Security] Attempted to load scene with null or empty name.");
                return;
            }

            if (sceneName.Length > 128)
            {
                Debug.LogError("[Security] Scene name exceeds maximum length limit.");
                return;
            }

            if (!SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError($"[Security] Invalid characters detected in scene name: '{sceneName}'.");
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
