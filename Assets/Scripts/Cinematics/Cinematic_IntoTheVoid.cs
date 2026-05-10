using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// Controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
    /// Refactored for performance, readability, and natural dialogue pacing.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Characters")]
        public GameObject Skyix_Character;
        public AudioSource Skyix_VoiceSource;
        public GameObject Kai_Character;
        public AudioSource Kai_VoiceSource;
        public GameObject Delilah_Character;
        public AudioSource Delilah_VoiceSource;

        [Header("UI Components")]
        public GameObject DialogueBox;
        public TextMeshProUGUI SpeakerNameText;
        public TextMeshProUGUI DialogueText;

        [Header("UX Settings")]
        [FormerlySerializedAs("typingSpeed")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine typingCoroutine;
        private Coroutine popCoroutine;
        private float currentTypingSpeed;
        private string currentSpeakerColorTag;
        private bool skipRequested;
        private Vector3 originalSpeakerScale;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        private void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }

            originalSpeakerScale = SpeakerNameText.transform.localScale;

            // Accessibility: Apply outlines for better legibility against Void backgrounds
            SpeakerNameText.outlineWidth = 0.2f;
            SpeakerNameText.outlineColor = Color.black;
            DialogueText.outlineWidth = 0.2f;
            DialogueText.outlineColor = Color.black;

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (popCoroutine != null) StopCoroutine(popCoroutine);

            SpeakerNameText.text = speaker;
            popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform));

            float multiplier = 1.0f;
            if (speaker == "Kai")
            {
                multiplier = kaiSpeedMultiplier;
                DialogueText.characterSpacing = 2.5f;
            }
            else
            {
                if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;
                DialogueText.characterSpacing = 0f;
            }

            currentTypingSpeed = baseTypingSpeed * multiplier;

            // Apply character-specific colors
            switch (speaker)
            {
                case "Sky.ix":
                    SpeakerNameText.color = Color.cyan;
                    currentSpeakerColorTag = "#00FFFF";
                    break;
                case "Kai":
                    SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                    currentSpeakerColorTag = "#FFD700";
                    break;
                case "Delilah":
                    SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                    currentSpeakerColorTag = "#991AE6";
                    break;
                default:
                    SpeakerNameText.color = Color.white;
                    currentSpeakerColorTag = "#FFFFFF";
                    break;
            }

            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator PopScale(Transform target)
        {
            float duration = 0.2f;
            float elapsed = 0f;
            Vector3 peakScale = originalSpeakerScale * 1.15f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                target.localScale = Vector3.Lerp(originalSpeakerScale, peakScale, Mathf.Sin(t * Mathf.PI));
                yield return null;
            }
            target.localScale = originalSpeakerScale;
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
                        // Check for mid-word periods (e.g., Sky.ix)
                        bool isMidWord = (i + 1 < totalVisibleCharacters) && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i + 1].character);
                        // Check for ellipses
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

                    yield return GetWait(delay);
                }
            }

            // Append color-coded completion indicator
            DialogueText.text = message + $" <color={currentSpeakerColorTag}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

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
            skipRequested = false; // Reset skip state after the pause
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
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

            // Action: Sky.ix dashes
            yield return WaitForSecondsOrSkip(2.0f);

            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            skipRequested = false;
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);
            Debug.Log("Cinematic Sequence Complete.");
        }
    }
}
