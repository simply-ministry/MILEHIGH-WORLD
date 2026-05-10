using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject shadowClonePrefab = null!;

        public override void ExecuteBehavior()
        {
            BossPhase();
        }

        public void BossPhase()
        {
            SpawnShadowClones();
            CastSicklyGreenBlackVoidfire();
        }

        private void SpawnShadowClones()
        {
            UnityEngine.Debug.Log("Delilah: Spawning shadow clones...");
            if (shadowClonePrefab != null)
            {
                UnityEngine.Object.Instantiate(shadowClonePrefab, transform.position + UnityEngine.Random.insideUnitSphere * 5f, UnityEngine.Quaternion.identity);
                // SECURITY: Ensure we use the UnityEngine.Random to avoid ambiguity and ensure correct behavior.
                Instantiate(shadowClonePrefab, transform.position + UnityEngine.Random.insideUnitSphere * 5f, Quaternion.identity);
            }
        }

        private void CastSicklyGreenBlackVoidfire()
        {
            UnityEngine.Debug.Log("Delilah: Casting sickly green-black Voidfire!");
        }
    }
}
