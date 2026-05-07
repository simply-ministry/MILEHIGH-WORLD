// --- UNITY SCENE SETUP --- //
//
// 1. Create an empty GameObject in your scene and name it "SceneController".
// 2. Attach this script (Cinematic_IntoTheVoid.cs) to it.
// 3. Configure character prefabs (Sky.ix, Kai, Delilah) with Animators and AudioSources.
// 4. Setup UI: Canvas -> DialogueBox (Panel) -> SpeakerNameText (TMP), DialogueText (TMP), SkipHint (TMP).
// 5. Drag and drop references into the Inspector.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// Controls the cinematic sequence for "Deep within the anti-reality of ŤĤÊ VØĪĐ".
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
    public TextMeshProUGUI SkipHint = null!;

    [Header("UX Settings")]
    [FormerlySerializedAs("typingSpeed")]
    public float baseTypingSpeed = 0.03f;
    public float kaiSpeedMultiplier = 3.0f;
    public float skyixSpeedMultiplier = 1.2f;
    public float idleHintThreshold = 2.0f;

    private Coroutine? typingCoroutine;
    private float currentTypingSpeed;
    private bool skipRequested;
    private float idleTimer;
    private bool playerInteracted;

    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
    {
        if (!_waitForSecondsCache.TryGetValue(time, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[time] = wait;
        }
        return wait;
    }

    void Start()
    {
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components! Cinematic aborted.");
            return;
        }

        if (SkipHint == null)
        {
            // Fallback: try to find it in children if not assigned
            SkipHint = DialogueBox.GetComponentInChildren<TextMeshProUGUI>(true);
            if (SkipHint == SpeakerNameText || SkipHint == DialogueText) SkipHint = null!;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Update()
    {
        // Reset idle timer and hide hint on any interaction
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            idleTimer = 0;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            skipRequested = true;
        }
        else if (DialogueBox.activeInHierarchy && !playerInteracted)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleHintThreshold && SkipHint != null)
            {
                SkipHint.gameObject.SetActive(true);
            }
        }
    }

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        SpeakerNameText.text = speaker;

        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };
        SpeakerNameText.color = speakerColor;

        typingCoroutine = StartCoroutine(TypeDialogue(message, speakerColor));
    }

    private IEnumerator TypeDialogue(string message, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalCharacters = DialogueText.textInfo.characterCount;
        // The cue ▽ is usually the last character
        int textOnlyCount = totalCharacters - 1;

        for (int i = 0; i <= textOnlyCount; i++)
        {
            if (skipRequested) break;

            DialogueText.maxVisibleCharacters = i;

            if (i > 0 && i <= textOnlyCount)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
                float delay = currentTypingSpeed;

                // Rhythmic pauses
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = false;
                    if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                    if (i < textOnlyCount && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;

                    if (isEllipsis) delay *= 5f;
                    else if (i < textOnlyCount && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                    {
                        // Mid-word period (e.g. Sky.ix)
                        delay = currentTypingSpeed;
                    }
                    else
                    {
                        delay *= 15f;
                    }
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay *= 8f;
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
        float start = Time.time;
        while (Time.time - start < duration && !skipRequested)
        {
            yield return null;
        }
        skipRequested = false;
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

        yield return WaitForSecondsOrSkip(2.0f); // Dash sequence

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        Debug.Log("Cinematic Sequence Complete.");
    }
}
