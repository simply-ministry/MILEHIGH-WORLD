using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
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

    [Header("UX Settings")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    public float baseTypingSpeed = 0.03f;
    [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
    public float kaiSpeedMultiplier = 3.0f;
    [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
    public float skyixSpeedMultiplier = 1.2f;

    private Coroutine? typingCoroutine;
    private float currentTypingSpeed;
    private bool skipRequested;

    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
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
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        // Ensure UI components are assigned to prevent NullReferenceException and potential stack trace leakage.
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Update()
    {
        if (Input.anyKeyDown) skipRequested = true;
    }

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        SpeakerNameText.text = speaker;

        // Apply speaker-specific speed multipliers
        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        // Apply character-specific colors
        switch (speaker)
        {
            case "Sky.ix":
                SpeakerNameText.color = Color.cyan;
                break;
            case "Kai":
                SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                break;
            case "Delilah":
                SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                break;
            default:
                SpeakerNameText.color = Color.white;
                break;
        }

        typingCoroutine = StartCoroutine(TypeDialogue(message));
    }

    private IEnumerator TypeDialogue(string message)
    {
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= message.Length; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = message.Length;
                break;
            }

            DialogueText.maxVisibleCharacters = i;

            if (i < message.Length)
            {
                char c = message[i];
                float delay = currentTypingSpeed;

                // UX Enhancement: Rhythmic punctuation pauses
                if (c == '.' || c == '!' || c == '?') delay += 0.4f;
                else if (c == ',' || c == ';' || c == ':') delay += 0.2f;

                yield return GetWait(delay);
            }
        }

        skipRequested = false;
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return GetWait(1.0f);

        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return GetWait(7.5f);

        yield return GetWait(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return GetWait(6.0f);

        yield return GetWait(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return GetWait(8.0f);

        yield return GetWait(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return GetWait(7.0f);

        yield return GetWait(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return GetWait(4.5f);

        yield return GetWait(2.0f);

        yield return GetWait(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return GetWait(3.5f);

        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return GetWait(5.5f);

        yield return GetWait(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return GetWait(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        Debug.Log("Cinematic Sequence Complete.");
    }
}
