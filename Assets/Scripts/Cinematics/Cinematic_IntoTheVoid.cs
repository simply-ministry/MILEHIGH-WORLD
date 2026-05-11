using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// Manages the cinematic sequence "Into the Void", handling dialogue, character animations, and rhythmic text reveal.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Character References")]
        public GameObject Skyix_Character = null!;
        public AudioSource Skyix_VoiceSource = null!;
        public GameObject Kai_Character = null!;
        public AudioSource Kai_VoiceSource = null!;
        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI? SkipHintText;

        [Header("UX Settings")]
        [Tooltip("Base delay in seconds between each character being revealed.")]
        public float baseTypingSpeed = 0.03f;
        [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
        public float kaiSpeedMultiplier = 3.0f;
        [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? popScaleCoroutine;
        private float currentTypingSpeed;
        private string currentSpeakerHex = "FFFFFF";
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private Vector3 originalSpeakerScale;

        // BOLT: Cache for WaitForSeconds to eliminate GC allocations during coroutine execution.
        // Using int key (milliseconds) to prevent float imprecision cache misses.
        private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

        private WaitForSeconds GetWait(float time)
        {
            int ms = Mathf.RoundToInt(time * 1000f);
            if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(time);
                _waitForSecondsCache[ms] = wait;
            }
            return wait;
        }

        private void Start()
        {
            // 🛡️ Sentinel: Defensive programming - Ensure UI components are assigned.
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic. Aborting.");
                return;
            }

            originalSpeakerScale = SpeakerNameText.transform.localScale;

            // Palette: Programmatically locate SkipHint if not assigned.
            if (SkipHintText == null && DialogueBox != null)
            {
                Transform hintTransform = DialogueBox.transform.Find("SkipHint");
                if (hintTransform != null) SkipHintText = hintTransform.GetComponent<TextMeshProUGUI>();
            }

            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

            // Palette: Accessibility - Text outline for better contrast in dark scenes.
            SpeakerNameText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
            SpeakerNameText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
            DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            // ⚡ Bolt: Precise skip detection for refined UX.
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0f;
                if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
            }

            // UX Enhancement: Show skip hint after 2 seconds of idleness during dialogue.
            if (DialogueBox != null && DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHintText != null)
                {
                    SkipHintText.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Starts the typewriter reveal.
        /// </summary>
        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            // UX Enhancement: Trigger a subtle "Pop" animation when the speaker changes.
            if (SpeakerNameText.text != speaker)
            {
                if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
                popScaleCoroutine = StartCoroutine(PopScaleEffect());
            }

            SpeakerNameText.text = speaker;

            // Apply speaker-specific speed multipliers and colors.
            float multiplier = 1.0f;
            Color speakerColor = Color.white;

            switch (speaker)
            {
                case "Sky.ix":
                    multiplier = skyixSpeedMultiplier;
                    speakerColor = Color.cyan;
                    break;
                case "Kai":
                    multiplier = kaiSpeedMultiplier;
                    speakerColor = new Color(1f, 0.84f, 0f); // Gold
                    break;
                case "Delilah":
                    speakerColor = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                    break;
            }

            SpeakerNameText.color = speakerColor;
            currentSpeakerHex = ColorUtility.ToHtmlStringRGB(speakerColor);
            currentTypingSpeed = baseTypingSpeed * multiplier;
            skipRequested = false;

            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator PopScaleEffect()
        {
            float elapsed = 0f;
            float duration = 0.2f;
            float targetScale = 1.15f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float percent = elapsed / duration;
                float curve = Mathf.Sin(percent * Mathf.PI);
                SpeakerNameText.transform.localScale = originalSpeakerScale * (1f + (targetScale - 1f) * curve);
                yield return null;
            }

            SpeakerNameText.transform.localScale = originalSpeakerScale;
            popScaleCoroutine = null;
        }

        private IEnumerator TypeDialogue(string message)
        {
            // UX Enhancement: Color-coded completion cue that matches speaker theme.
            // We append it upfront to prevent layout shifts.
            DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            TMP_TextInfo textInfo = DialogueText.textInfo;
            int totalCharacters = textInfo.characterCount;
            // The cue character is at the end. We reveal characters up to the cue.
            int mainMessageLength = totalCharacters - 1;

            for (int i = 0; i <= mainMessageLength; i++)
            {
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= mainMessageLength)
                {
                    char c = textInfo.characterInfo[i - 1].character;
                    float delay = currentTypingSpeed;

                    // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Look-ahead to avoid sentence-end delays for mid-word periods (e.g., Sky.ix).
                        bool isEndOfSentence = true;
                        if (i < mainMessageLength)
                        {
                            char nextChar = textInfo.characterInfo[i].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence)
                        {
                            // Ellipsis detection: dot with dot neighbors.
                            bool isEllipsis = (i > 1 && textInfo.characterInfo[i - 2].character == '.') ||
                                             (i < mainMessageLength && textInfo.characterInfo[i].character == '.');

                            delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                        }
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }

                    yield return GetWait(delay);
                }
            }

            DialogueText.maxVisibleCharacters = totalCharacters;
            skipRequested = false;
            typingCoroutine = null;
        }

        private IEnumerator WaitForSecondsOrSkip(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && !skipRequested)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            skipRequested = false;
        }

        /// <summary>
        /// Unified helper to play a dialogue line and wait for it to finish reveal plus a reading pause.
        /// </summary>
        private IEnumerator PlayDialogueLine(string speaker, string message, float readingPause)
        {
            ShowDialogue(speaker, message);
            // Wait for typing to complete (or be skipped)
            while (typingCoroutine != null) yield return null;
            // Now wait for the reading pause (also skippable)
            yield return WaitForSecondsOrSkip(readingPause);
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return WaitForSecondsOrSkip(1.0f);

            // --- Dialogue Line 1: Delilah ---
            yield return WaitForSecondsOrSkip(1.5f);
            yield return PlayDialogueLine("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.", 2.5f);

            // --- Dialogue Line 2: Sky.ix ---
            yield return WaitForSecondsOrSkip(0.5f);
            yield return PlayDialogueLine("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.", 1.5f);

            // --- Dialogue Line 3: Kai ---
            yield return WaitForSecondsOrSkip(0.7f);
            yield return PlayDialogueLine("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!", 2.0f);

            // --- Dialogue Line 4: Delilah ---
            yield return WaitForSecondsOrSkip(1.2f);
            yield return PlayDialogueLine("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.", 2.0f);

            // --- Dialogue Line 5: Sky.ix ---
            yield return WaitForSecondsOrSkip(0.8f);
            yield return PlayDialogueLine("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!", 1.0f);

            // --- ACTION: Sky.ix dashes towards the conduit ---
            yield return WaitForSecondsOrSkip(2.0f);

            // --- Dialogue Line 6: Kai ---
            yield return WaitForSecondsOrSkip(0.5f);
            yield return PlayDialogueLine("Kai", "The energy spike is massive! Your shields won't hold for long!", 1.0f);

            // --- Dialogue Line 7: Delilah ---
            yield return WaitForSecondsOrSkip(1.5f);
            yield return PlayDialogueLine("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.", 1.5f);

            // --- Dialogue Line 8: Sky.ix ---
            yield return WaitForSecondsOrSkip(1.0f);
            yield return PlayDialogueLine("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.", 3.0f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
        }
    }
}
