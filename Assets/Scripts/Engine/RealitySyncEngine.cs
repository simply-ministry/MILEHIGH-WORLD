// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Core;

namespace MilehighWorld.Engine
{
    /// <summary>
    /// Handles the deterministic synchronization of reality states (Void vs. Now).
    /// Adjusts world-space constants and entity behavior multipliers based on Void Variance.
    /// </summary>
    public class RealitySyncEngine : MonoBehaviour
    {
        public static RealitySyncEngine Instance { get; private set; } = null!;

        [Header("Reality Parameters")]
        [SerializeField, Range(0, 1)] private float voidVariance = 0.5f;
        [SerializeField] private float stabilityThreshold = 0.0777777777f;

        public float VoidVariance => voidVariance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // Update GlobalResonanceManager with deterministic Base-9 aligned values
            if (GlobalResonanceManager.Instance != null)
            {
                // Ensure synchronization at a 1.0Hz Love Frequency equivalent
                if (Time.frameCount % 9 == 0)
                {
                    GlobalResonanceManager.Instance.UpdateResonance(voidVariance);
                }
            }
        }

        /// <summary>
        /// Reconciles the local reality state with a target variance.
        /// BOLT: Uses a non-linear interpolation to simulate reality "snapping".
        /// </summary>
        public void ReconcileState(float targetVariance)
        {
            voidVariance = Mathf.Lerp(voidVariance, targetVariance, stabilityThreshold);

            // Log state transition if threshold is crossed
            if (Mathf.Abs(voidVariance - targetVariance) < 0.001f)
            {
                Debug.Log($"[RealitySync]: Reality stabilized at variance {voidVariance}");
            }
        }

        /// <summary>
        /// Calculates a combat or physics multiplier based on the current Void depth.
        /// </summary>
        public float GetRealityMultiplier()
        {
            // Base-9 Parity Curve
            return 1.0f + (voidVariance * 0.09f);
        }
    }
}
