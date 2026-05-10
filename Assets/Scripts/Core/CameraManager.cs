using UnityEngine;

namespace Milehigh.Core
{
    public class CameraManager : MonoBehaviour
    {
        public Camera mainCamera = null!;

        public void SetCameraPosition(Vector3 position)
        {
            if (mainCamera != null)
            {
                mainCamera.transform.position = position;
            }
        public Camera mainCamera;

        public void SwitchCamera(Camera newCamera)
        {
            if (mainCamera != null) mainCamera.enabled = false;
            mainCamera = newCamera;
            if (mainCamera != null) mainCamera.enabled = true;
        }
    }
}
