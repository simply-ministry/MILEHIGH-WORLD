// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using System.Collections;

namespace MilehighWorld.Characters
{
    public class NefariousAI : MonoBehaviour
    {
        public void TriggerEnemyGlitch()
        {
            Renderer enemyRenderer = GetComponent<Renderer>();

            // Null check as requested to prevent ReferenceExceptions
            if (enemyRenderer == null) return;

            // Logic: Toggle a 'Glitch' property on the material
            enemyRenderer.material.SetFloat("_GlitchIntensity", 1.0f);

            // Reset after a brief moment
            StartCoroutine(ResetGlitch(enemyRenderer));
        }

        private IEnumerator ResetGlitch(Renderer r)
        {
            yield return new WaitForSeconds(0.2f);
            if (r != null)
            {
                r.material.SetFloat("_GlitchIntensity", 0f);
            }
        }
    }
}
