// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilehighWorld.Cinematics
{
    /// <summary>
    /// Manages the cinematic sequence "Into the Void", handling dialogue, character animations, audio, and rhythmic text reveal.
    /// BOLT: Optimized to remove redundant engine calls, consolidate caches, and eliminate duplicate logic from botched merges.
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

        private Animator? _skyixAnimator;
        private Animator? _kaiAnimator;
        private Animator? _delilahAnimator;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public CanvasGroup DialogueCanvasGroup = null!;
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
        private Color currentSpeakerColor = Color.white;
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private bool _isDialogueBoxActive;
        private bool _isTypeRevealComplete;
        private bool _isSkipHintActive;
        private Vector3 originalSpeakerScale;
        private RectTransform _dialogueRect = null!;
        private Vector2 _originalDialoguePos;

        private struct SpeakerStyle
        {
            public Color color;
            public string hexColor;
            public float speedMultiplier;
            public AudioSource? voiceSource;
        }
        private readonly Dictionary<string, SpeakerStyle> _speakerStyles = new Dictionary<string, SpeakerStyle>();

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
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null || DialogueCanvasGroup == null)
            {
                Debug.LogError("Missing UI components required for cinematic. Aborting.");
                return;
            }

            _isDialogueBoxActive = DialogueBox.activeInHierarchy;
            originalSpeakerScale = SpeakerNameText.transform.localScale;
            _dialogueRect = DialogueBox.GetComponent<RectTransform>();
            _originalDialoguePos = _dialogueRect.anchoredPosition;

            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

            if (Skyix_Character != null) _skyixAnimator = Skyix_Character.GetComponent<Animator>();
            if (Kai_Character != null) _kaiAnimator = Kai_Character.GetComponent<Animator>();
            if (Delilah_Character != null) _delilahAnimator = Delilah_Character.GetComponent<Animator>();

            _speakerStyles["Sky.ix"] = new SpeakerStyle { color = Color.cyan, hexColor = ColorUtility.ToHtmlStringRGB(Color.cyan), speedMultiplier = skyixSpeedMultiplier, voiceSource = Skyix_VoiceSource };
            _speakerStyles["Kai"] = new SpeakerStyle { color = new Color(1f, 0.84f, 0f), hexColor = ColorUtility.ToHtmlStringRGB(new Color(1f, 0.84f, 0f)), speedMultiplier = kaiSpeedMultiplier, voiceSource = Kai_VoiceSource };
            _speakerStyles["Delilah"] = new SpeakerStyle { color = new Color(0.6f, 0.1f, 0.9f), hexColor = ColorUtility.ToHtmlStringRGB(new Color(0.6f, 0.1f, 0.9f)), speedMultiplier = 1.0f, voiceSource = Delilah_VoiceSource };

            ApplyTextOutline(SpeakerNameText);
            ApplyTextOutline(DialogueText);
            if (SkipHintText != null) ApplyTextOutline(SkipHintText);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void ApplyTextOutline(TextMeshProUGUI text)
        {
            if (text != null && text.fontMaterial != null)
            {
                text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.25f);
                text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            }
        }

        private void Update()
        {
            if (!_isDialogueBoxActive) return;

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0f;
                if (_isSkipHintActive && SkipHintText != null)
                {
                    SkipHintText.gameObject.SetActive(false);
                    _isSkipHintActive = false;
                }
            }

            if (!playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHintText != null && !_isSkipHintActive)
                {
                    SkipHintText.gameObject.SetActive(true);
                    _isSkipHintActive = true;
                }
            }

            if (_isSkipHintActive && SkipHintText != null)
            {
                float alpha = Mathf.PingPong(Time.time * 0.5f, 0.5f) + 0.5f;
                SkipHintText.canvasRenderer.SetAlpha(alpha);
            }

            if (_isTypeRevealComplete && DialogueText != null)
            {
                var textInfo = DialogueText.textInfo;
                if (textInfo.characterCount > 0)
                {
                    int lastCharIndex = textInfo.characterCount - 1;
                    if (textInfo.characterInfo[lastCharIndex].isVisible)
                    {
                        float pulse = Mathf.PingPong(Time.time * 2.5f, 0.5f) + 0.5f;
                        Color32 pulseColor = currentSpeakerColor;
                        pulseColor.a = (byte)(pulse * 255);

                        int materialIndex = textInfo.characterInfo[lastCharIndex].materialReferenceIndex;
                        int vertexIndex = textInfo.characterInfo[lastCharIndex].vertexIndex;
                        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

                        for (int i = 0; i < 4; i++) vertexColors[vertexIndex + i] = pulseColor;
                        DialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    }
                }
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            _isTypeRevealComplete = false;
            idleTimer = 0f;
            playerInteracted = false;
            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
            _isSkipHintActive = false;

            if (SpeakerNameText.text != speaker)
            {
                if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
                popScaleCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));
            }

            SpeakerNameText.text = speaker;

            if (_speakerStyles.TryGetValue(speaker, out SpeakerStyle style))
            {
                SpeakerNameText.color = style.color;
                currentSpeakerColor = style.color;
                currentSpeakerHex = style.hexColor;
                currentTypingSpeed = baseTypingSpeed * style.speedMultiplier;
                if (style.voiceSource != null) style.voiceSource.Play();
            }
            else
            {
                SpeakerNameText.color = Color.white;
                currentSpeakerColor = Color.white;
                currentSpeakerHex = "FFFFFF";
                currentTypingSpeed = baseTypingSpeed;
            }

            skipRequested = false;
            typingCoroutine = StartCoroutine(TypeDialogue(message));
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
            DialogueText.ForceMeshUpdate();
            _isTypeRevealComplete = true;
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

        private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
        {
            if (targetAlpha > 0)
            {
                DialogueBox.SetActive(true);
                _isDialogueBoxActive = true;
            }
            float startAlpha = DialogueCanvasGroup.alpha;

            Vector2 startPos = _originalDialoguePos + (targetAlpha > 0 ? new Vector2(0, -25f) : Vector2.zero);
            Vector2 endPos = targetAlpha > 0 ? _originalDialoguePos : _originalDialoguePos + new Vector2(0, -25f);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float ease = 1f - Mathf.Pow(1f - t, 3f);

                DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                if (_dialogueRect != null) _dialogueRect.anchoredPosition = Vector2.Lerp(startPos, endPos, ease);
                yield return null;
            }

            DialogueCanvasGroup.alpha = targetAlpha;
            if (_dialogueRect != null) _dialogueRect.anchoredPosition = endPos;
            if (targetAlpha <= 0)
            {
                DialogueBox.SetActive(false);
                _isDialogueBoxActive = false;
            }
        }

        private IEnumerator PopScale(Transform target, float duration, float scaleFactor)
        {
            Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float curve = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1f);
                target.localScale = initialScale * (1f + curve);
                yield return null;
            }
            target.localScale = initialScale;
            popScaleCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return WaitForSecondsOrSkip(1.0f);

            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Channeling_Idle");
            yield return PlayDialogueLine("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.", 2.5f);

            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("React_Furious");
            yield return PlayDialogueLine("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.", 1.5f);

            if (_kaiAnimator != null) _kaiAnimator.SetTrigger("Point_Urgent");
            yield return PlayDialogueLine("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!", 2.0f);

            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Smirk_Dismissive");
            yield return PlayDialogueLine("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.", 2.0f);

            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Action_Ready");
            yield return PlayDialogueLine("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!", 1.0f);

            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Dash_Forward");
            yield return WaitForSecondsOrSkip(2.0f);

            if (_kaiAnimator != null) _kaiAnimator.SetTrigger("React_Alarmed");
            yield return PlayDialogueLine("Kai", "The energy spike is massive! Your shields won't hold for long!", 1.0f);

            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Taunt_OpenArms");
            yield return PlayDialogueLine("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.", 1.5f);

            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Determined_Resolve");
            yield return PlayDialogueLine("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.", 3.0f);

            yield return FadeDialogueBox(0f, 0.5f);
            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
        }
    }
}
