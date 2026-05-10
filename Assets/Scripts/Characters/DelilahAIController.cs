using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Core;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject shadowClonePrefab = null!;

        // BOLT: Object pool to eliminate Instantiate/Destroy overhead
        private readonly Queue<GameObject> _clonePool = new Queue<GameObject>();
        private const int MAX_CLONES = 5;

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
            Debug.Log("Delilah: Spawning shadow clones...");
            if (shadowClonePrefab == null) return;

            GameObject? clone = null;

            // BOLT: Reuse from pool if available
            while (_clonePool.Count > 0)
            {
                clone = _clonePool.Dequeue();
                if (clone != null)
                {
                    clone.transform.position = transform.position + Random.insideUnitSphere * 5f;
                    clone.transform.rotation = Quaternion.identity;
                    clone.SetActive(true);
                    break;
                }
            }

            // BOLT: Instantiate if pool is empty or contained destroyed objects
            if (clone == null)
            {
                clone = Instantiate(shadowClonePrefab, transform.position + Random.insideUnitSphere * 5f, Quaternion.identity);
            }

            StartCoroutine(RecycleClone(clone));
        }

        private IEnumerator RecycleClone(GameObject? clone)
        {
            // BOLT: Use centralized WaitForSeconds caching
            yield return UnityUtils.GetWait(5.0f);

            if (clone != null)
            {
                clone.SetActive(false);
                // BOLT: Cap pool growth
                if (_clonePool.Count < MAX_CLONES)
                {
                    _clonePool.Enqueue(clone);
                }
                else
                {
                    Destroy(clone);
                }
            }
        }

        private void CastSicklyGreenBlackVoidfire()
        {
            Debug.Log("Delilah: Casting sickly green-black Voidfire!");
        public override void ExecuteBehavior()
        {
            Debug.Log("Delilah AI executing behavior...");
        }
    }
}
