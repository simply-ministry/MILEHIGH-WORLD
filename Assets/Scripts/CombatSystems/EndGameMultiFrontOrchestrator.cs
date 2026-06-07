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

        // ⚡ Bolt: Cache shader property IDs to eliminate per-frame string-to-int lookups.
        // ⚡ Bolt: Cache shader property IDs to eliminate per-frame string-to-int lookups in high-frequency loops.
        private static readonly int VoidPulseRateId = Shader.PropertyToID("_VoidPulseRate");
        private static readonly int EmissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");

        public async Task CoordinateFinalNexusLockAsync(EncounterDirector director, LatticeSynchronizer synchronizer)
        {
            Debug.Log("<color=#E0BBE4>[SYSTEM]: multi_front_battle_loop initiated. Synchronizing thread data...</color>");

            // ⚡ Bolt: Pre-cache character references and components outside the hot loop to reduce CPU overhead.
            float originalTimeScale = Time.timeScale;
            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            try
            {
                // ⚡ Bolt: Hoist character references and component lookups outside the hot loop.
                var micahBulwark = director.GetAlly("Micah");
                var skyIxVanguard = director.GetAlly("Sky.ix");
                var kingCyrusBoss = director.GetEnemy("KingCyrus");
                var reverie = director.GetAlly("Reverie");

                if (micahBulwark != null && micahBulwark.PrefabReference != null)
            // ⚡ Bolt: Hoist character references and component lookups outside the hot loop.
            var micahBulwark = director.GetAlly("Micah");
            var skyIxVanguard = director.GetAlly("Sky.ix");
            var reverie = director.GetAlly("Reverie");
            var kingCyrusBoss = director.GetEnemy("KingCyrus");

            Rigidbody? micahRB = null;
            if (micahBulwark != null && micahBulwark.PrefabReference != null)
            // ⚡ Bolt: Cache components and set constant values once outside the loop to eliminate redundant native writes.
            if (micahBulwark?.PrefabReference != null)
            {
                micahRB = micahBulwark.PrefabReference.GetComponent<Rigidbody>();
                // ⚡ Bolt: Set constant property values once outside the loop.
                if (micahRB != null) micahRB.mass = 900.0f;
            }

            float voidVarianceDelta = 0.99f;
            float combinedTraumaModifier = 0.85f; // Clamped index based on Micah + Cirrus profiles

            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            // ⚡ Bolt: Hoist GetPropertyBlock out of the loop to eliminate redundant native-to-managed copies.
            if (platformRenderer != null) platformRenderer.GetPropertyBlock(_propBlock);

            try
            {
                // 2. Main evaluation loop for the convergence
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

                    // Decrement global variance based on local structural shard completion
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

                // 3. Force 180-Degree Physical Inversion on the core Onalym database node
                await EntityRotation.ApplyPhaseShift(180f);
                Debug.Log("<color=cyan>[SYSTEM]: Hex-State 6.0 inverted. Compiling stable Linear Singularity 9.0.</color>");

                // 4. Finalize checksum at the 999th logic shard checkpoint
                if (synchronizer != null)
                {
                    synchronizer.SynchronizeShard(RealityConstants.MaxShardParity, combinedTraumaModifier);

                    // Toggle active rendering state on the Onalym gateway to seal the sector
                    if (onalymNexusGateway != null) onalymNexusGateway.SetActive(false);

            // ⚡ Bolt: Pre-cache MaterialPropertyBlock once before the loop to save redundant native-to-managed copies every frame.
            if (platformRenderer != null)
            {
                platformRenderer.GetPropertyBlock(_propBlock);
            }

            // Main multi-threaded evaluation loop for the convergence
            while (voidVarianceDelta > 0.0f)
            {
                // Verify background tracking integrity to ensure client stability
                if (kingCyrusBoss == null || micahBulwark == null)
                {
                    if (micahBulwark.PrefabReference.TryGetComponent<Rigidbody>(out var micahRB))
                    {
                        // ⚡ Bolt: Set mass once outside the loop as it remains constant.
                        micahRB.mass = 900.0f;
                    }
                }

                float voidVarianceDelta = 0.99f;
                float combinedTraumaModifier = 0.85f;

                // ⚡ Bolt: Pre-cache MaterialPropertyBlock once before the loop.
                if (platformRenderer != null)
                {
                    platformRenderer.GetPropertyBlock(_propBlock);
                }

                // 2. Main multi-threaded evaluation loop for the convergence
                while (voidVarianceDelta > 0.0f)
                {
                    // Verify background tracking integrity
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

                    // ⚡ Bolt: Use cached Property IDs for O(1) shader updates.
                    if (platformRenderer != null)
                    {
                        _propBlock.SetFloat(VoidPulseRateId, voidVarianceDelta);
                        _propBlock.SetFloat(EmissiveIntensityId, voidVarianceDelta * 4.5f);
                        platformRenderer.SetPropertyBlock(_propBlock);
                    }

                    await Task.Yield();
                // ⚡ Bolt: Using pre-cached references to avoid O(N) lookups and native bridge overhead.
                reverie?.UseAbility("Arcane Symphony");
                skyIxVanguard?.UseAbility("Void Step");

                // Decrement global variance based on local structural shard completion
                voidVarianceDelta -= 0.11f;

                // Real-time update to HDRP custom material instances via property IDs
                // ⚡ Bolt: Using cached Renderer, Property IDs, and MaterialPropertyBlock for efficient shader updates.
                if (platformRenderer != null && _propBlock != null)
                {
                    _propBlock.SetFloat(VoidPulseRateId, voidVarianceDelta);
                    _propBlock.SetFloat(EmissiveIntensityId, voidVarianceDelta * 4.5f);
                    platformRenderer.SetPropertyBlock(_propBlock);
                }

                // 3. Force 180-Degree Physical Inversion on the core Onalym database node
                await EntityRotation.ApplyPhaseShift(180f);
                Debug.Log("<color=cyan>[SYSTEM]: Hex-State 6.0 inverted. Compiling stable Linear Singularity 9.0.</color>");

                // 4. Finalize checksum at the 999th logic shard checkpoint
                if (synchronizer != null)
                {
                    synchronizer.SynchronizeShard(RealityConstants.MaxShardParity, combinedTraumaModifier);

                    if (onalymNexusGateway != null) onalymNexusGateway.SetActive(false);
                    Debug.Log("<color=#00FF00>[SYSTEM]: Save Everyone Protocol success. Parity resonance locked at True Monad (1.0). Millenia online.</color>");
                }
            }
            finally
            {
                // ⚡ Bolt: Implement try-finally to guarantee time scale restoration.
                // ⚡ Bolt: Restore baseline simulation time execution in a finally block to ensure stability.
                Time.timeScale = 1.0f;
            }
        }
    }
}
