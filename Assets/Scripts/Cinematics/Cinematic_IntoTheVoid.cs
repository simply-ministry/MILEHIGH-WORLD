using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using Milehigh.Core;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of THE VOID, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'--a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        // Add an Action Delegate at the top of the class
        public event Action? OnCinematicComplete;

        // ====================================================================
        // CHARACTER ASSET & VOICE REFERENCE BLOCK
        // ====================================================================

        [Header("Characters")]
        // Protagonist: Sky.ix the The Bionic Goddess
        // Description: A 45-year-old Caucasian cyborg woman with short white hair. She has humanoid features but her face and body have visible cybernetic enhancements that allow her to traverse the Void. She was a brilliant xenolinguist who, along with her family, was part of the research team at the Onalym Nexus.
        // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/skyix.png
        // Ability Script: Ability_Skyix.cs
        /* VOICE PROFILE:
         * Pitch: Mid-Range Mezzo-Shorano
         * Tempo: Steady and Precise (130-140 WPM)
         * Texture & Effects: Clean, Clear, and Articulated. Subtle Digital/Synthetic Filter (low chorus).
         * Projection: Medium-High, Direct
         * Tone & Style: Driven, Loving, Determined. Underlying sorrow/weariness.
         * Keywords: Digital, Bionic, Precise, Loving, Clear Articulation, Subtle Filter.
        */
        public GameObject Skyix_Character = null!;
        public AudioSource Skyix_VoiceSource = null!;

        // Protagonist: Kai the The Child of Prophecy
        // Description: Sky.ix's child, lost and now found. Holds the key to the Prophecy.
        // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/kai.png
        // Ability Script: Ability_Kai.cs
        /* VOICE PROFILE:
         * Pitch: Gender Neutral/Mid-Range
         * Tempo: Slow and Paused (70-90 WPM)
         * Texture & Effects: Aged, Weathered, and Layered. Subtle Temporal Echo/Layering effect.
         * Projection: Soft, but Infinitely Resonant
         * Tone & Style: Cryptic, Calm, Profound, and Fatalistic. Speaks in metaphor.
         * Keywords: Ancient, Layered, Slow, Resonant, Cryptic, Contemplative.
        */
        public GameObject Kai_Character = null!;
        public AudioSource Kai_VoiceSource = null!;

        // Antagonist: Delilah the The Desolate
        // Description: A corrupted form of Ingris, wielding Voidfire.
        // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/antagonists/delilah.png
        // Ability Script: Ability_Delilah.cs
        /* VOICE PROFILE:
         * Not available.
        */
        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public CanvasGroup? DialogueCanvasGroup;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI? SkipHint;

        [Header("UX Settings")]
        [FormerlySerializedAs("typingSpeed")]
        [Tooltip("Base delay in seconds between each character being revealed.")]
        public float baseTypingSpeed = 0.03f;
        [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
        public float kaiSpeedMultiplier = 3.0f;
        [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? namePopCoroutine;
        private string lastSpeaker = "";
        private float currentTypingSpeed;
        private string currentSpeakerHex = "#FFFFFF";
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private Vector3 originalSpeakerScale;

        private void Awake()
        {
            if (SpeakerNameText != null)
                originalSpeakerScale = SpeakerNameText.transform.localScale;
        }

        private void Start()
        {
            // 🛡️ Sentinel: Security enhancement - Defensive programming
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }

            if (SkipHint != null) SkipHint.gameObject.SetActive(false);

            // Palette UX: Improve text contrast in the dark "Void" environment via outlines.
            DialogueText.fontMaterial.EnableKeyword("OUTLINE_ON");
            DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            // Poll for skip input to ensure responsiveness across multiple accessible inputs
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0;
                if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            }

            // UX Enhancement: Show skip hint after idleness
            if (DialogueBox != null && DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHint != null && !SkipHint.gameObject.activeSelf)
                {
                    SkipHint.text = "[Space] Skip";
                    SkipHint.gameObject.SetActive(true);
                }
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            // UX Enhancement: Pop animation when the speaker changes
            if (speaker != lastSpeaker)
            {
                if (namePopCoroutine != null) StopCoroutine(namePopCoroutine);
                namePopCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.15f, 1.2f));
                lastSpeaker = speaker;
            }

            SpeakerNameText.text = speaker;

            // Apply character-specific colors
            switch (speaker)
            {
                case "Sky.ix":
                    SpeakerNameText.color = Color.cyan;
                    currentSpeakerHex = "#00FFFF";
                    currentTypingSpeed = baseTypingSpeed * skyixSpeedMultiplier;
                    break;
                case "Kai":
                    SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                    currentSpeakerHex = "#FFD700";
                    currentTypingSpeed = baseTypingSpeed * kaiSpeedMultiplier;
                    break;
                case "Delilah":
                    SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                    currentSpeakerHex = "#991AE6";
                    currentTypingSpeed = baseTypingSpeed;
                    break;
                default:
                    SpeakerNameText.color = Color.white;
                    currentSpeakerHex = "#FFFFFF";
                    currentTypingSpeed = baseTypingSpeed;
                    break;
            }

            skipRequested = false;
            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            DialogueText.text = message;
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = DialogueText.textInfo.characterCount;

            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = totalVisibleCharacters;
                    break;
                }

                DialogueText.maxVisibleCharacters = i;

                if (i < totalVisibleCharacters)
                {
                    char c = DialogueText.textInfo.characterInfo[i].character;
                    float delay = currentTypingSpeed;

                    // Rhythmic Punctuation Logic
                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isMidWord = (i + 1 < totalVisibleCharacters) && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i + 1].character);
                        bool isEllipsis = (i + 1 < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i + 1].character == '.') ||
                                         (i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.');

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isMidWord) delay = currentTypingSpeed;
                        else delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }

                    yield return UnityUtils.GetWait(delay);
                }
            }

            // Append color-coded completion indicator
            DialogueText.text = message + $" <color={currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

            typingCoroutine = null;
        }

        private IEnumerator PopScale(Transform target, float duration, float multiplier)
        {
            Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = elapsed / duration;
                float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI) * (multiplier - 1f);
                target.localScale = initialScale * scaleFactor;
                yield return null;
            }

            target.localScale = initialScale;
            namePopCoroutine = null;
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

        private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
        {
            if (DialogueCanvasGroup == null)
            {
                DialogueBox.SetActive(targetAlpha > 0);
                yield break;
            }

            if (targetAlpha > 0) DialogueBox.SetActive(true);

            float startAlpha = DialogueCanvasGroup.alpha;
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }

            DialogueCanvasGroup.alpha = targetAlpha;
            if (targetAlpha <= 0) DialogueBox.SetActive(false);
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return WaitForSecondsOrSkip(1.0f);

            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
            yield return WaitForSecondsOrSkip(7.5f);

            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
            yield return WaitForSecondsOrSkip(6.0f);

            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
            yield return WaitForSecondsOrSkip(8.0f);

            ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
            yield return WaitForSecondsOrSkip(7.0f);

            ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
            yield return WaitForSecondsOrSkip(4.5f);

            yield return WaitForSecondsOrSkip(2.0f);

            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            yield return FadeDialogueBox(0.0f, 0.5f);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");

            // Invoke the event so the Battle Orchestrator knows to begin combat
            OnCinematicComplete?.Invoke();
        }
    }
}
