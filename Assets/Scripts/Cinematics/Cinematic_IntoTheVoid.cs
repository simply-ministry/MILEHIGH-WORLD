using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
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
        }
    }
}
