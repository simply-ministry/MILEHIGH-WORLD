using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// Manages the cinematic sequence "Into the Void", handling dialogue, character animations, audio, and rhythmic text reveal.
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
        public CanvasGroup DialogueCanvasGroup = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI? SkipHintText;
        public TextMeshProUGUI SkipHint = null!;

        [Header("UX Settings")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? popScaleCoroutine;
        private float currentTypingSpeed;
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;

        private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

        private WaitForSeconds GetWait(float time)
        {
            int msKey = Mathf.RoundToInt(time * 1000f);
            if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
            {
                wait = new WaitForSeconds(time);
                _waitForSecondsCache[msKey] = wait;
            }
            return wait;
        }

        private void Start()
        {
            if (DialogueBox != null) DialogueBox.SetActive(false);
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                skipRequested = true;
                idleTimer = 0;
                playerInteracted = true;
                if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            }
            else
            {
                idleTimer += Time.deltaTime;
                if (!playerInteracted && idleTimer >= 2.0f && SkipHint != null)
                {
                    SkipHint.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return FadeDialogue(1f, 0.5f);

            // --- Dialogue Line 1: Delilah ---
            yield return GetWait(1.0f);
            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
            yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));

            // --- Dialogue Line 2: Sky.ix ---
            yield return GetWait(0.5f);
            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
            yield return StartCoroutine(WaitForSecondsOrSkip(5.5f));

            // --- Dialogue Line 3: Kai ---
            yield return GetWait(0.5f);
            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
            yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));

            // --- Dialogue Line 4: Delilah ---
            yield return GetWait(0.5f);
            ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
            yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));

            // --- Dialogue Line 5: Sky.ix ---
            yield return GetWait(0.5f);
            ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
            yield return StartCoroutine(WaitForSecondsOrSkip(4.5f));

            // --- Dialogue Line 6: Kai ---
            yield return GetWait(0.5f);
            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return StartCoroutine(WaitForSecondsOrSkip(3.5f));

            // --- Dialogue Line 7: Delilah ---
            yield return GetWait(0.5f);
            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return StartCoroutine(WaitForSecondsOrSkip(5.5f));

            // --- Dialogue Line 8: Sky.ix ---
            yield return GetWait(0.5f);
            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            yield return FadeDialogue(0f, 0.5f);
            DialogueBox.SetActive(false);
            Debug.Log("Cinematic Sequence Complete.");
        }

        public void ShowDialogue(string speaker, string message)
        {
            skipRequested = false;
            SpeakerNameText.text = speaker;

            float multiplier = 1.0f;
            if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
            else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;
            currentTypingSpeed = baseTypingSpeed * multiplier;

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(message));

            if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
            popScaleCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));
        }

        private IEnumerator TypeDialogue(string message)
        {
            DialogueText.text = "";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.text = message;

            for (int i = 0; i <= message.Length; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = message.Length;
                    break;
                }
                DialogueText.maxVisibleCharacters = i;
                yield return GetWait(currentTypingSpeed);
            }
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

        private IEnumerator FadeDialogue(float targetAlpha, float duration)
        {
            float startAlpha = DialogueCanvasGroup.alpha;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }
            DialogueCanvasGroup.alpha = targetAlpha;
        }

        private IEnumerator PopScale(Transform target, float duration, float scaleMultiplier)
        {
            Vector3 initialScale = Vector3.one;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI) * (scaleMultiplier - 1f);
                target.localScale = initialScale * scaleFactor;
                yield return null;
            }
            target.localScale = initialScale;

        private Coroutine? typingCoroutine;
        private Coroutine? popCoroutine;
        private float currentTypingSpeed;
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

        void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }
            originalSpeakerScale = SpeakerNameText.transform.localScale;
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0;
                if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            }
            else if (DialogueBox != null && DialogueBox.activeInHierarchy)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && !playerInteracted && SkipHint != null)
                {
                    SkipHint.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator WaitForSecondsOrSkip(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && !skipRequested)
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
            if (targetAlpha > 0) DialogueBox.SetActive(true);
            float startAlpha = DialogueCanvasGroup.alpha;
            float elapsed = 0;
            while (elapsed < duration)

            originalSpeakerScale = SpeakerNameText.transform.localScale;

            // Palette: Programmatically locate SkipHint if not assigned.
            if (SkipHintText == null && DialogueBox != null)
            {
                Transform hintTransform = DialogueBox.transform.Find("SkipHint");
                if (hintTransform != null) SkipHintText = hintTransform.GetComponent<TextMeshProUGUI>();
            }
            DialogueCanvasGroup.alpha = targetAlpha;
            if (targetAlpha <= 0) DialogueBox.SetActive(false);

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
                if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
            }

            // UX Enhancement: Show skip hint after 2 seconds of idleness.
            if (DialogueBox != null && DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHintText != null)
                {
                    SkipHintText.gameObject.SetActive(true);
                }
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (popCoroutine != null) StopCoroutine(popCoroutine);

            // UX Enhancement: Reset idle timer for each new dialogue line.
            idleTimer = 0f;

            // UX Enhancement: Trigger a subtle "Pop" animation when the speaker changes.
            if (SpeakerNameText.text != speaker)
            {
                if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
                popScaleCoroutine = StartCoroutine(PopScaleEffect());
            }

            SpeakerNameText.text = speaker;
            popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));

            // Apply speaker-specific speed multipliers and colors.
            float multiplier = 1.0f;
            Color speakerColor = Color.white;
            AudioSource? voiceSource = null;

            switch (speaker)
            {
                case "Sky.ix":
                    multiplier = skyixSpeedMultiplier;
                    speakerColor = Color.cyan;
                    voiceSource = Skyix_VoiceSource;
                    break;
                case "Kai":
                    multiplier = kaiSpeedMultiplier;
                    speakerColor = new Color(1f, 0.84f, 0f); // Gold
                    voiceSource = Kai_VoiceSource;
                    break;
                case "Delilah":
                    speakerColor = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                    voiceSource = Delilah_VoiceSource;
                    break;
            }

            SpeakerNameText.color = speakerColor;
            currentSpeakerHex = ColorUtility.ToHtmlStringRGB(speakerColor);
            currentTypingSpeed = baseTypingSpeed * multiplier;
            skipRequested = false;

            // Audio: Play the character's voice line if assigned.
            if (voiceSource != null) voiceSource.Play();

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
            DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            TMP_TextInfo textInfo = DialogueText.textInfo;
            int totalCharacters = textInfo.characterCount;
            int mainMessageLength = totalCharacters - 1;

            for (int i = 0; i <= mainMessageLength; i++)
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f),
                "Delilah" => new Color(0.6f, 0.1f, 0.9f),
                _ => Color.white
            };
            SpeakerNameText.color = speakerColor;
            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = DialogueText.textInfo.characterCount;
            int messageLength = totalVisibleCharacters - 2;

            for (int i = 0; i <= messageLength; i++)
            {
                if (skipRequested) break;
                DialogueText.maxVisibleCharacters = i;
                if (i > 0 && i <= messageLength)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    float delay = currentTypingSpeed;
                    if (c == "."[0] || c == "!"[0] || c == "?"[0]) delay *= 15f;
                    else if (c == ","[0] || c == ";"[0] || c == ":"[0]) delay *= 8f;
                    yield return GetWait(delay);
                }
            }
            DialogueText.maxVisibleCharacters = totalVisibleCharacters;
            typingCoroutine = null;
        }

        private IEnumerator PopScale(Transform target, float duration, float scaleFactor)
        {
            Vector3 initialScale = Vector3.one;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float curve = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1f);
                target.localScale = initialScale * (1f + curve);
                yield return null;
            }
            target.localScale = initialScale;
            popCoroutine = null;
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

            yield return FadeDialogueBox(0f, 0.5f);
            Debug.Log("Cinematic Sequence Complete.");
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= mainMessageLength)
                {
                    char c = textInfo.characterInfo[i - 1].character;
                    float delay = currentTypingSpeed;

                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEndOfSentence = true;
                        if (i < mainMessageLength)
                        {
                            char nextChar = textInfo.characterInfo[i].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence)
                        {
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

        private IEnumerator PlayDialogueLine(string speaker, string message, float readingPause)
        {
            ShowDialogue(speaker, message);
            while (typingCoroutine != null) yield return null;
            yield return WaitForSecondsOrSkip(readingPause);
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return WaitForSecondsOrSkip(1.0f);

            // Line 1: Delilah
            if (Delilah_Character != null) Delilah_Character.GetComponent<Animator>()?.SetTrigger("Channeling_Idle");
            yield return PlayDialogueLine("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.", 2.5f);

            // Line 2: Sky.ix
            if (Skyix_Character != null) Skyix_Character.GetComponent<Animator>()?.SetTrigger("React_Furious");
            yield return PlayDialogueLine("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.", 1.5f);

            // Line 3: Kai
            if (Kai_Character != null) Kai_Character.GetComponent<Animator>()?.SetTrigger("Point_Urgent");
            yield return PlayDialogueLine("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!", 2.0f);

            // Line 4: Delilah
            if (Delilah_Character != null) Delilah_Character.GetComponent<Animator>()?.SetTrigger("Smirk_Dismissive");
            yield return PlayDialogueLine("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.", 2.0f);

            // Line 5: Sky.ix
            if (Skyix_Character != null) Skyix_Character.GetComponent<Animator>()?.SetTrigger("Action_Ready");
            yield return PlayDialogueLine("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!", 1.0f);

            // ACTION: Sky.ix dashes
            if (Skyix_Character != null) Skyix_Character.GetComponent<Animator>()?.SetTrigger("Dash_Forward");
            yield return WaitForSecondsOrSkip(2.0f);

            // Line 6: Kai
            if (Kai_Character != null) Kai_Character.GetComponent<Animator>()?.SetTrigger("React_Alarmed");
            yield return PlayDialogueLine("Kai", "The energy spike is massive! Your shields won't hold for long!", 1.0f);

            // Line 7: Delilah
            if (Delilah_Character != null) Delilah_Character.GetComponent<Animator>()?.SetTrigger("Taunt_OpenArms");
            yield return PlayDialogueLine("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.", 1.5f);

            // Line 8: Sky.ix
            if (Skyix_Character != null) Skyix_Character.GetComponent<Animator>()?.SetTrigger("Determined_Resolve");
            yield return PlayDialogueLine("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.", 3.0f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
        }
    }
}
