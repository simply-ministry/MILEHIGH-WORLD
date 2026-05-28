using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MilehighWorld.Core;
using MilehighWorld.Simulation;

namespace MilehighWorld.CombatSystems
{
    public class EndGameMultiFrontOrchestrator : MonoBehaviour
    {
        private const int TARGET_PARITY_NODE = 999;
        private readonly double eventHorizonLimit = 1.4446678659d;

        [Header("Global Material Overrides")]
        [SerializeField] private Renderer platformRenderer;
        [SerializeField] private GameObject onalymNexusGateway;

        private static MaterialPropertyBlock? _propBlock;

        // ⚡ Bolt: Pre-cache shader property IDs to eliminate string hashing overhead in the hot loop.
        private static readonly int VoidPulseRateId = Shader.PropertyToID("_VoidPulseRate");
        private static readonly int EmissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");

        public async Task CoordinateFinalNexusLockAsync(EncounterDirector director, LatticeSynchronizer synchronizer)
        {
            Debug.Log("<color=#E0BBE4>[SYSTEM]: multi_front_battle_loop initiated. Synchronizing thread data...</color>");

            // 1. Unpack entity targets from registry
            var micahBulwark = director.GetAlly("Micah");
            var skyIxVanguard = director.GetAlly("Sky.ix");
            var aeronGuardian = director.GetAlly("Aeron");
            var cirrusDragon = director.GetAlly("Cirrus");
            var kingCyrusBoss = director.GetEnemy("KingCyrus");

            // ⚡ Bolt: Hoist constant lookups and component fetches outside the hot loop.
            var reverieAlly = director.GetAlly("Reverie");
            Rigidbody? squadMassOverride = (micahBulwark?.PrefabReference != null)
                ? micahBulwark.PrefabReference.GetComponent<Rigidbody>()
                : null;

            // ⚡ Bolt: Set constant property values once outside the loop.
            if (squadMassOverride != null) squadMassOverride.mass = 900.0f;

            float voidVarianceDelta = 0.99f;
            float combinedTraumaModifier = 0.85f; // Clamped index based on Micah + Cirrus profiles

            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            // 2. Main multi-threaded evaluation loop for the convergence
            while (voidVarianceDelta > 0.0f)
            {
                // Verify background tracking integrity to ensure client stability
                if (kingCyrusBoss == null || micahBulwark == null)
                {
                    Debug.LogError("[CRITICAL ERROR]: Primary combat node dereferenced. Reality deallocated.");
                    return;
                }

                // ⚡ Bolt: Removed redundant GetComponent and mass assignment from loop.

                // Process the 1000 Fox Parade / Arcane Symphony visual degradation tracking
                // ⚡ Bolt: Using cached ally reference to avoid repeated O(1) dictionary lookups.
                if (reverieAlly != null) reverieAlly.UseAbility("Arcane Symphony");
                if (skyIxVanguard != null) skyIxVanguard.UseAbility("Void Step");

                // Decrement global variance based on local structural shard completion
                voidVarianceDelta -= 0.11f;

                // Real-time update to HDRP custom material instances via property IDs
                // ⚡ Bolt: Using cached Renderer and Property IDs for O(1) shader updates.
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
                synchronizer.SynchronizeShard(TARGET_PARITY_NODE, combinedTraumaModifier);

                // Toggle active rendering state on the Onalym gateway to seal the sector
                onalymNexusGateway.SetActive(false);
                Time.timeScale = 1.0f; // Restore baseline standard simulation time execution

                Debug.Log("<color=#00FF00>[SYSTEM]: Save Everyone Protocol success. Parity resonance locked at True Monad (1.0). Millenia online.</color>");
            }
        }
    }
}
