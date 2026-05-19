// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Core
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;

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

            // Apply Narrative Rule: Arcane/Justice deals bonus damage to Void-Corrupted targets
            if (target.IsVoidCorrupted && (attacker.TechAlignment == "Arcane" || attacker.TechAlignment == "Hybrid"))
            {
                finalDamage *= 1.5f; // 50% bonus damage
                Debug.Log($"<color=orange>[COMBAT]: {attacker.Id} triggered Arcane Resonance against Void Corruption!</color>");
            }

            // Apply Global Reality Integrity modifier (weaker reality = less physical damage)
            if (GlobalResonanceManager.Instance != null)
            {
                finalDamage *= GlobalResonanceManager.Instance.GetIntegrityMultiplier();
            }

            return finalDamage;
        }
    }
}
