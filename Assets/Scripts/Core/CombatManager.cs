// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Core
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public float CalculateVanguardDamage(RuntimeCharacterData attacker, RuntimeCharacterData target, float baseWeaponDamage)
        {
            float finalDamage = baseWeaponDamage;

            if (target.IsVoidCorrupted && (attacker.TechAlignment == "Arcane" || attacker.TechAlignment == "Hybrid"))
            {
                finalDamage *= 1.5f;
                Debug.Log($"<color=orange>[COMBAT]: {attacker.Id} triggered Arcane Resonance against Void Corruption!</color>");
            }

            if (GlobalResonanceManager.Instance != null)
            {
                finalDamage *= GlobalResonanceManager.Instance.GetIntegrityMultiplier();
            }

            return finalDamage;
        }

        private static readonly int GlitchIntensityId = Shader.PropertyToID("_GlitchIntensity");
        private static MaterialPropertyBlock? _glitchPropertyBlock;

        public void TriggerEnemyGlitch(GameObject target)
        {
            if (target.TryGetComponent<Renderer>(out Renderer ren))
            {
                if (_glitchPropertyBlock == null) _glitchPropertyBlock = new MaterialPropertyBlock();
                ren.GetPropertyBlock(_glitchPropertyBlock);
                _glitchPropertyBlock.SetFloat(GlitchIntensityId, 1.0f);
                ren.SetPropertyBlock(_glitchPropertyBlock);
            }
        }
    }
}
