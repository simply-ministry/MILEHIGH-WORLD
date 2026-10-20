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
            // 🛡️ Sentinel: Validate sceneName parameter to prevent path traversal and scene injection vulnerabilities
            if (string.IsNullOrWhiteSpace(sceneName) || sceneName.Length > 128 || !SafeSceneNameRegex.IsMatch(sceneName))
            {
                Debug.LogError($"[Security] Invalid scene name parameter provided to AsyncSceneLoader: '{sceneName}'");
                return;
            }

            StartCoroutine(LoadAsync(sceneName));
        }

        private IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            if (asyncLoad == null)
            {
                Debug.LogError($"[AsyncSceneLoader] Failed to initiate async load for scene: '{sceneName}'");
                yield break;
            }

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
