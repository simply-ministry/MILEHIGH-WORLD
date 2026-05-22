// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using MilehighWorld.Core;
using MilehighWorld.Backend;

namespace MilehighWorld.Cinematics
{
    /// <summary>
    /// Manages the asynchronous execution of the "Into the Void" cinematic climax.
    /// Drives HDRP shader manipulation, Base-9 parity alignment, and lexical pacing.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Entity References")]
        [SerializeField] private GameObject skyixPrefab = null!;
        [SerializeField] private GameObject reveriePrefab = null!;
        [SerializeField] private GameObject kingCyrusPrefab = null!;

        [Header("UI & Lexical Systems")]
        [SerializeField] private TextMeshProUGUI speakerNameText = null!;
        [SerializeField] private TextMeshProUGUI dialogueText = null!;
        [SerializeField] private GameObject dialogueCanvas = null!;

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;
        private const float IteratedSanctuary = 0.0777777777f;

        // Palette: Visual Polish
        private Vector3 originalSpeakerScale;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            // Palette: Accessibility - High-contrast outlines for better readability
            ApplyHighContrastOutline(speakerNameText);
            ApplyHighContrastOutline(dialogueText);
            if (speakerNameText != null) originalSpeakerScale = speakerNameText.transform.localScale;

            _ = ExecuteConvergenceSequenceAsync();
        }

        private void ApplyHighContrastOutline(TextMeshProUGUI text)
        {
            if (text == null) return;
            text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
            text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            // 1. Force the local coordinate space into a Linear Omen (6.0) Hex-State
            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

            // 2. Transfinite Data Load: Initialize entities from object pools (Disable vs Destroy SOP)
            skyixPrefab.SetActive(true);
            reveriePrefab.SetActive(true);
            kingCyrusPrefab.SetActive(true);

            // 3. Asynchronous Lexical Pacing
            dialogueCanvas.SetActive(true);
            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", 0.04f);
            await Task.Delay(500);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. We are at 998 shards. Engaging Void Conduit.", 0.03f);

            // 4. Parity Verification via OMEGA.ONE Fulcrum
            LogNarrativeTelemetry("Executing BackendSyncService Call: Validating Parity Resonance...");
            var resolution = await BackendSyncService.Instance.RequestAIResolutionAsync(
                stateHash: 998,
                parityResonance: 0.999f,
                activeReality: "Void",
                zoneId: "LOC_001_LINQ"
            );

            if (resolution.WasActionSuccessful)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. Severing the loop... now!", 0.03f);
                await ExecuteSaveEveryoneProtocolAsync();
            }
            else
            {
                LogNarrativeTelemetry("WARNING: Parity Lock Failed. Initiating Fallback.");
            }

            dialogueCanvas.SetActive(false);
        }

        /// <summary>
        /// Mathematically tweens the HDRP shader's emissive intensity to simulate Void corruption.
        /// </summary>
        private async Task TweenShaderEntropyAsync(float targetIntensity, float duration)
        {
            float startIntensity = hyperrealisticPlatformShader.GetFloat(emissiveIntensityId);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                hyperrealisticPlatformShader.SetFloat(emissiveIntensityId, currentIntensity);

                // Processor Choking Prevention: Yield execution
                await Task.Yield();
            }
        }

        /// <summary>
        /// Executes the final visual and logical reset to the True Monad (1.0).
        /// </summary>
        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            LogNarrativeTelemetry("PROTOCOL_SAVE_EVERYONE Initiated. Physics re-aligning.");

            // Fade out Cyrus using Object Pooling SOP (Alpha Decay)
            await TweenAlphaDecayAsync(kingCyrusPrefab.GetComponentInChildren<Renderer>().material, 1.5f);
            kingCyrusPrefab.SetActive(false); // Return to pool

            // Clamp environmental delta changes instantly upon loop completion
            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);

            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        private async Task TweenAlphaDecayAsync(Material mat, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                mat.SetFloat(baseColorAlphaId, alpha);
                await Task.Yield();
            }
        }

        /// <summary>
        /// High-polish rhythmic typewriter effect with layout stability and speaker-specific themes.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string colorHex = GetSpeakerColorHex(speaker);

            // Palette: Trigger "Pop" effect and color speaker name if it changes
            if (speakerNameText.text != $"<color={colorHex}>[{speaker}]</color>")
            {
                speakerNameText.text = $"<color={colorHex}>[{speaker}]</color>";
                _ = PopScaleAsync(speakerNameText.transform);
            }

            // Palette: Pre-calculate layout with completion cue to prevent shifts
            dialogueText.text = content + $" <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;

            for (int i = 0; i <= totalCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                // Palette: Rhythmic Pacing - Apply delays based on punctuation
                if (i > 0 && i < totalCharacters)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float multiplier = 1f;

                    // Look-ahead for "Sky.ix" and abbreviations (no pause if followed by alphanumeric)
                    bool isTechnicalPeriod = false;
                    if (c == '.' && i < content.Length)
                    {
                        char next = content[i];
                        if (!char.IsWhiteSpace(next)) isTechnicalPeriod = true;
                    }

                    if (!isTechnicalPeriod)
                    {
                        if (c == '.' || c == '?' || c == '!') multiplier = 15f;
                        else if (c == ',' || c == ':' || c == ';') multiplier = 8f;

                        // Palette: Ellipsis check (reduced multiplier for consecutive dots)
                        if (c == '.' && i > 1 && content[i - 2] == '.') multiplier = 5f;
                    }

                    if (multiplier > 1f)
                    {
                        await Task.Delay(Mathf.RoundToInt(charDelay * multiplier * 1000));
                    }
                }

                await Task.Delay(Mathf.RoundToInt(charDelay * 1000));
            }
        }

        private async Task PopScaleAsync(Transform target)
        {
            if (target == null) return;
            float duration = 0.2f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float s = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.1f;
                target.localScale = originalSpeakerScale * s;
                await Task.Yield();
            }
            target.localScale = originalSpeakerScale;
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => "#00FFFF", // Cyan
                "King Cyrus" => "#FFFF00", // Yellow
                "Reverie" => "#FF00FF", // Magenta
                _ => "#FFFFFF" // White
            };
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
