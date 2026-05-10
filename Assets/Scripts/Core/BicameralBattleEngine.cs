using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Core
{
    public class BicameralBattleEngine : MonoBehaviour
    {
        public bool isDualProcessingActive = false;

        public void ProcessDualIntent(string protagonistIntent, string antagonistIntent)
        {
            isDualProcessingActive = true;
            Debug.Log($"Bicameral Engine: Processing {protagonistIntent} vs {antagonistIntent}");

            // Phase 2 Logic: Resolve conflicts between protagonist and antagonist
            bool success = Random.value > 0.3f;
            if (success)
            {
                Debug.Log("Bicameral Engine: Protagonist intent succeeded.");
            }
            else
            {
                Debug.Log("Bicameral Engine: Antagonist intent prevailed.");
            }
        }

        public void FinalizeTurn()
        {
            isDualProcessingActive = false;
            Debug.Log("Bicameral Engine: Turn finalized.");
        }
    }
}
