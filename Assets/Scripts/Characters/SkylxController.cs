// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Core;

namespace MilehighWorld.Characters
{
    public class SkylxController : MonoBehaviour
    {
        [Header("Combat Visuals")]
        [SerializeField] private Transform muzzlePoint;
        [SerializeField] private GameObject beamFX; // The visual representation of the Void Blast

        public void ExecuteFireLogic()
        {
            if (beamFX != null && muzzlePoint != null)
            {
                // Visual Parity: Instantiate the BeamFX at the muzzle point
                GameObject beam = Instantiate(beamFX, muzzlePoint.position, muzzlePoint.rotation);

                // If it's a particle system, play it. If it's a prefab, it might already play on awake.
                ParticleSystem ps = beam.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }

                Destroy(beam, 0.5f); // Clean up the effect

                Debug.Log("<color=cyan>[Skylx]: Firing Void Conduit Gauntlet.</color>");

                // Trigger parity logic (e.g., checking if the shot synchronizes with the lattice)
                FireLogicParity();
            }
        }

        private void FireLogicParity()
        {
            // Backend math for the 9-bit recursive loop
            if (GlobalResonanceManager.Instance != null)
            {
                // Deduct void variance slightly on successful fire
            }
        }
    }
}
