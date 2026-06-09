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
        [SerializeField] private Renderer platformRenderer = null!;
        [SerializeField] private GameObject onalymNexusGateway = null!;

        private static MaterialPropertyBlock? _propBlock;

        // ⚡ Bolt: Cache shader property IDs to eliminate per-frame string-to-int lookups in high-frequency loops.
        private static readonly int VoidPulseRateId = Shader.PropertyToID("_VoidPulseRate");
        private static readonly int EmissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");

        public async Task CoordinateFinalNexusLockAsync(EncounterDirector director, LatticeSynchronizer synchronizer)
        {
            Debug.Log("<color=#E0BBE4>[SYSTEM]: multi_front_battle_loop initiated. Synchronizing thread data...</color>");

            float originalTimeScale = Time.timeScale;
            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            // ⚡ Bolt: Hoist character references and component lookups outside the hot loop.
            var micahBulwark = director.GetAlly("Micah");
            var skyIxVanguard = director.GetAlly("Sky.ix");
            var reverie = director.GetAlly("Reverie");
            var kingCyrusBoss = director.GetEnemy("KingCyrus");

            if (micahBulwark?.PrefabReference != null)
            {
                if (micahBulwark.PrefabReference.TryGetComponent<Rigidbody>(out var micahRB))
                {
                    // ⚡ Bolt: Set constant property values once outside the loop.
                    micahRB.mass = 900.0f;
                }
            }

            float voidVarianceDelta = 0.99f;
            float combinedTraumaModifier = 0.85f;

            // ⚡ Bolt: Hoist GetPropertyBlock out of the loop to eliminate redundant native-to-managed copies.
            if (platformRenderer != null) platformRenderer.GetPropertyBlock(_propBlock);

            try
            {
                // Main evaluation loop for the convergence
                while (voidVarianceDelta > 0.0f)
                {
                    // Verify background tracking integrity to ensure client stability
                    if (kingCyrusBoss == null || micahBulwark == null)
                    {
                        Debug.LogError("[CRITICAL ERROR]: Primary combat node dereferenced. Reality deallocated.");
                        return;
                    }

                    // ⚡ Bolt: Using pre-cached references to avoid O(N) lookups and native bridge overhead.
                    reverie?.UseAbility("Arcane Symphony");
                    skyIxVanguard?.UseAbility("Void Step");

                    // Decrement global variance
                    voidVarianceDelta -= 0.11f;

                    // ⚡ Bolt: Using cached Renderer and Property IDs for O(1) shader updates.
                    if (platformRenderer != null)
                    {
                        _propBlock.SetFloat(VoidPulseRateId, voidVarianceDelta);
                        _propBlock.SetFloat(EmissiveIntensityId, voidVarianceDelta * 4.5f);
                        platformRenderer.SetPropertyBlock(_propBlock);
                    }

                    // Yield main execution thread back to Unity script scheduler every frame
                    await Task.Yield();
                }

                // Force 180-Degree Physical Inversion on the core Onalym database node
                await EntityRotation.ApplyPhaseShift(180f);
                Debug.Log("<color=cyan>[SYSTEM]: Hex-State 6.0 inverted. Compiling stable Linear Singularity 9.0.</color>");

                // Finalize checksum at the 999th logic shard checkpoint
                if (synchronizer != null)
                {
                    synchronizer.SynchronizeShard(RealityConstants.MaxShardParity, combinedTraumaModifier);

                    // Toggle active rendering state on the Onalym gateway to seal the sector
                    if (onalymNexusGateway != null) onalymNexusGateway.SetActive(false);
                    Debug.Log("<color=#00FF00>[SYSTEM]: Save Everyone Protocol success. Parity resonance locked at True Monad (1.0). Millenia online.</color>");
                }
            }
            finally
            {
                // ⚡ Bolt: Restore baseline simulation time execution in a finally block to ensure stability.
                Time.timeScale = originalTimeScale;
            }
        }
    }
}
