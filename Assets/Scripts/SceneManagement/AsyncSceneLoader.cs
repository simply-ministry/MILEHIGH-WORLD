using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        private const int MaxSceneNameLength = 128;
        private static readonly Regex SafeSceneNameRegex = new Regex(@"^[a-zA-Z0-9_\-/]+$", RegexOptions.Compiled);

        public void LoadScene(string sceneName)
        {
            // 🛡️ Sentinel: Validate sceneName to prevent path traversal and scene injection
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[SECURITY] LoadScene rejected: Scene name cannot be null or empty.");
                return;
            }

            if (sceneName.Length > MaxSceneNameLength)
            {
                Debug.LogError("[SECURITY] LoadScene rejected: Scene name exceeds maximum length.");
                return;
            }

            if (!SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError("[SECURITY] LoadScene rejected: Scene name contains invalid characters.");
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
