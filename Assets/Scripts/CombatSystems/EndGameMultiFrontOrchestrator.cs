using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MilehighWorld.Core;
using MilehighWorld.Simulation;
using Milehigh.World.CoreLogic;

namespace MilehighWorld.CombatSystems
{
    public class EndGameMultiFrontOrchestrator : MonoBehaviour
    {
        [Header("Global Material Overrides")]
        [SerializeField] private Renderer platformRenderer;
        [SerializeField] private GameObject onalymNexusGateway;

        private static MaterialPropertyBlock? _propBlock;

        // ⚡ Bolt: Cache shader property IDs to avoid string lookups in high-frequency loops.
        private static readonly int VoidPulseRateId = Shader.PropertyToID("_VoidPulseRate");
        private static readonly int EmissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");

        public async Task CoordinateFinalNexusLockAsync(EncounterDirector director, LatticeSynchronizer synchronizer)
        {
            Debug.Log("<color=#E0BBE4>[SYSTEM]: multi_front_battle_loop initiated. Synchronizing thread data...</color>");

            // 1. Unpack entity targets from registry
            var micahBulwark = director.GetAlly("Micah");
            var skyIxVanguard = director.GetAlly("Sky.ix");
            var reverieAlly = director.GetAlly("Reverie");
            var kingCyrusBoss = director.GetEnemy("KingCyrus");

            float voidVarianceDelta = 0.99f;
            float combinedTraumaModifier = 0.85f; // Clamped index based on Micah + Cirrus profiles

            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            // ⚡ Bolt: Pre-cache components outside the loop.
            Rigidbody? squadMassOverride = null;
            if (micahBulwark != null && micahBulwark.PrefabReference != null)
            {
                squadMassOverride = micahBulwark.PrefabReference.GetComponent<Rigidbody>();
                // ⚡ Bolt: Set mass once outside the loop as it remains constant during this phase.
                if (squadMassOverride != null) squadMassOverride.mass = 900.0f;
            }

            // 2. Main multi-threaded evaluation loop for the convergence
            while (voidVarianceDelta > 0.0f)
            {
                // Verify background tracking integrity to ensure client stability
                if (kingCyrusBoss == null || micahBulwark == null)
                {
                    Debug.LogError("[CRITICAL ERROR]: Primary combat node dereferenced. Reality deallocated.");
                    return;
                }

                // Process the 1000 Fox Parade / Arcane Symphony visual degradation tracking
                reverieAlly?.UseAbility("Arcane Symphony");
                skyIxVanguard?.UseAbility("Void Step");

                // Decrement global variance based on local structural shard completion
                voidVarianceDelta -= 0.11f;

                // Real-time update to material instances via property IDs
                if (platformRenderer != null)
                {
                    platformRenderer.GetPropertyBlock(_propBlock);
                    _propBlock.SetFloat(VoidPulseRateId, voidVarianceDelta);
                    _propBlock.SetFloat(EmissiveIntensityId, voidVarianceDelta * 4.5f);
                    platformRenderer.SetPropertyBlock(_propBlock);
                }

                // Yield main execution thread back to Unity script scheduler every frame
                await Task.Yield();
            }

            // 3. Force 180-Degree Physical Inversion on the core Onalym database node
            await EntityRotation.ApplyPhaseShift(180f);
            Debug.Log("<color=cyan>[SYSTEM]: Hex-State 6.0 inverted. Compiling stable Linear Singularity 9.0.</color>");

            // 4. Finalize checksum at the 999th logic shard checkpoint
            if (synchronizer != null)
            {
                synchronizer.SynchronizeShard(RealityConstants.MaxShardParity, combinedTraumaModifier);

                // Toggle active rendering state on the Onalym gateway to seal the sector
                if (onalymNexusGateway != null) onalymNexusGateway.SetActive(false);
                Time.timeScale = 1.0f; // Restore baseline standard simulation time execution

                Debug.Log("<color=#00FF00>[SYSTEM]: Save Everyone Protocol success. Parity resonance locked at True Monad (1.0). Millenia online.</color>");
            }
        }
    }
}
