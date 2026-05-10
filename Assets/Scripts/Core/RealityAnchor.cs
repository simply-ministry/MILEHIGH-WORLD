using UnityEngine;

namespace Milehigh.Core
{
    public class RealityAnchor : MonoBehaviour
    {
        public float anchorStability = 1.0f;

        public void StabilizeReality(float amount)
        {
            anchorStability = Mathf.Clamp01(anchorStability + amount);
            Debug.Log($"Reality Anchor: Stability at {anchorStability}");
        }
    }
}
