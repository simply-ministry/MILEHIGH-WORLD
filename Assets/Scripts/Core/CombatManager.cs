using UnityEngine;

namespace Milehigh.World.Core
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        public float CalculateVanguardDamage(CharacterData attacker, float baseDamage)
        {
            float integrityMult = GlobalResonanceManager.Instance.GetIntegrityMultiplier();
            // Formula: $D_{total} = (D_{base} \cdot M_{vanguard}) \cdot I_{multiplier}$
            return (baseDamage * attacker.vanguardMultiplier) * integrityMult;
        }

        public void TriggerEnemyGlitch(GameObject target)
        {
            if (target.TryGetComponent<Renderer>(out Renderer ren))
            {
                // Error 14 Fix: Null check handled via TryGetComponent
                ren.material.SetFloat("_GlitchIntensity", 1.0f);
            }
        }
    }
}
