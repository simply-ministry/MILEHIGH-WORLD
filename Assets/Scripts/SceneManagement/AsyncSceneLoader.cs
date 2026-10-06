using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        private static readonly Regex SafeSceneNameRegex = new Regex(@"^[a-zA-Z0-9_\-/]+$", RegexOptions.Compiled);
        private const int MaxSceneNameLength = 128;

        public void LoadScene(string sceneName)
        {
            // 🛡️ Sentinel: Validate sceneName parameter against null or whitespace.
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[Security] Attempted to load scene with null or whitespace sceneName.");
                return;
            }

            // 🛡️ Sentinel: Restrict sceneName length to mitigate resource exhaustion or buffer overflow risks.
            if (sceneName.Length > MaxSceneNameLength)
            {
                Debug.LogError($"[Security] Scene name exceeds maximum allowed length ({MaxSceneNameLength} chars).");
                return;
            }

            // 🛡️ Sentinel: Whitelist scene name characters to prevent path traversal or parameter injection attacks.
            if (!SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError($"[Security] Blocked unauthorized or malformed scene name load request: '{sceneName}'");
                return;
            }

            StartCoroutine(LoadAsync(sceneName));
        }

        private IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            if (asyncLoad == null)
            {
                Debug.LogError($"[Security] Unable to initialize async operation for scene '{sceneName}'.");
                yield break;
            }

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
