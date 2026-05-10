using UnityEditor;
using UnityEditor.SceneManagement;

namespace Milehigh.Editor
{
    public class MultiSceneSetup : EditorWindow
    public class MultiSceneSetup
    {
        [MenuItem("Milehigh/Setup Multi-Scene")]
        public static void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }
    }
}
