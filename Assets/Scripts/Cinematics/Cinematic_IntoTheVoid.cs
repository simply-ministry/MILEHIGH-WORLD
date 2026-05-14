using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;

        [Header("Settings")]
        public float typingSpeed = 0.05f;

        private bool skipRequested = false;
        private Coroutine? typingCoroutine;

        private void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(speaker, message));
        }

        private IEnumerator TypeDialogue(string speaker, string message)
        {
            skipRequested = false;
            SpeakerNameText.text = speaker;
            DialogueText.text = message;
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalCharacters = DialogueText.textInfo.characterCount;

            for (int i = 0; i <= totalCharacters; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = totalCharacters;
                    break;
                }

                DialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(typingSpeed);
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

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return StartCoroutine(WaitForSecondsOrSkip(1.0f));

            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise.");
            yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));

            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're just a vandal smashing something beautiful.");
            yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));

            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. Hit the third resonant frequency conduit... now!");
            yield return StartCoroutine(WaitForSecondsOrSkip(8.0f));

            DialogueBox.SetActive(false);
            Debug.Log("Cinematic Sequence Complete.");
        }

        // Mocks for voice source if used elsewhere
        private void StopCoroutine(Coroutine coroutine) {}
    }

    // Dummy class for AudioSource if not in mocks
    public class AudioSource : MonoBehaviour
    {
        public void Play() {}
    }
}
