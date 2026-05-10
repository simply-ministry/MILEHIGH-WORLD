// --- UNITY SCENE SETUP --- //
//
// 1. Create an empty GameObject in your scene and name it "SceneController".
//
// 2. Attach this script (`Cinematic_IntoTheVoid.cs`)
//    to the "SceneController" GameObject.
//
// 3. Create or place the character prefabs/GameObjects for "Sky.ix", "Kai", and "Delilah" into the scene.
//
// 4. Ensure each character GameObject has the following components attached:
//    - An Animator component with a configured Animation Controller.
//    - An AudioSource component to be used for their voice lines.
//    - Their respective ability script (e.g., Sky.ix needs `Ability_Skyix.cs`, Kai needs `Ability_Kai.cs`, etc.)
//
// 5. Create the UI for the dialogue system:
//    - Right-click in the Hierarchy -> UI -> Canvas.
//    - Inside the Canvas, create a UI -> Panel. Rename it "DialogueBox". This will be the background.
//    - Inside the "DialogueBox", create two UI -> Text - TextMeshPro objects.
//    - Name the first one "SpeakerNameText" and position it where the speaker's name should appear.
//    - Name the second one "DialogueText" and position it for the main dialogue content.
//    - Initially, set the "DialogueBox" GameObject to be inactive (uncheck the box in the Inspector).
//
// 6. Select the "SceneController" GameObject. In the Inspector, drag and drop the corresponding scene objects
//    into the public fields of this script:
//    - Drag the "Sky.ix" GameObject into the `Skyix_Character` field.
//    - Drag the AudioSource from "Sky.ix" into the `Skyix_VoiceSource` field.
//    - Drag the "Kai" GameObject into the `Kai_Character` field.
//    - Drag the AudioSource from "Kai" into the `Kai_VoiceSource` field.
//    - Drag the "Delilah" GameObject into the `Delilah_Character` field.
//    - Drag the AudioSource from "Delilah" into the `Delilah_VoiceSource` field.
//    - Drag the "DialogueBox" panel into the `Dialogue Box` field.
//    - Drag the "SpeakerNameText" TMP object into the `Speaker Name Text` field.
//    - Drag the "DialogueText" TMP object into the `Dialogue Text` field.
//
// 7. Ensure your project has TextMeshPro imported (Window -> TextMeshPro -> Import TMP Essential Resources).

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    // ====================================================================
    //
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    //
    // ====================================================================

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
    public GameObject Skyix_Character;
    public AudioSource Skyix_VoiceSource;


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
    public GameObject Kai_Character;
    public AudioSource Kai_VoiceSource;


    // Antagonist: Delilah the The Desolate
    // Description: A corrupted form of Ingris, wielding Voidfire.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/antagonists/delilah.png
    // Ability Script: Ability_Delilah.cs
    /* VOICE PROFILE:
     * Not available.
    */
    public GameObject Delilah_Character;
    public AudioSource Delilah_VoiceSource;

    [Header("UI Components")]
    public GameObject DialogueBox = null!;
    public TextMeshProUGUI SpeakerNameText = null!;
    public TextMeshProUGUI DialogueText = null!;
    public TextMeshProUGUI SkipHint = null!;
    public GameObject DialogueBox;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI DialogueText;

    [Header("UX Settings")]
    [FormerlySerializedAs("typingSpeed")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    public float baseTypingSpeed = 0.03f;
    [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
    public float kaiSpeedMultiplier = 3.0f;
    [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
    public float skyixSpeedMultiplier = 1.2f;

    private Coroutine typingCoroutine;
    private float currentTypingSpeed;
    private string currentSpeakerHex;
    private bool skipRequested;
    private float idleTimer;
    private bool playerInteracted;

    private Vector3 _originalSpeakerScale;
    private string _lastSpeaker;
    private Coroutine _popCoroutine;

    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
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

    private IEnumerator WaitForSecondsOrSkip(float time)
    {
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown)
        {
            skipRequested = true;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }

        // UX Enhancement: Show "[Space] Skip" hint after 2 seconds of idleness during dialogue.
        // This makes the skip feature discoverable without cluttering the UI for experienced players.
        if (DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2.0f && SkipHint != null && !SkipHint.gameObject.activeSelf)
            {
                SkipHint.text = "[Space] Skip";
                SkipHint.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Yields for the specified duration but returns immediately if a skip is requested.
    /// Resets the skip flag upon completion.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float duration)
        float startTime = Time.time;
        while (Time.time - startTime < time && !skipRequested)
        {
            yield return null;
        }
        skipRequested = false;
    }

    private IEnumerator PopScale(Transform target)
    {
        Vector3 originalScale = target.localScale;
        float elapsed = 0f;
        float duration = 0.15f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.1f;
            target.localScale = originalScale * scale;
            yield return null;
        }
        target.localScale = originalScale;
    }

    void Start()
    {
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        // Ensure UI components are assigned to prevent NullReferenceException and potential stack trace leakage.
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        // Programmatic fallback for SkipHint to ensure UX feature works even if not assigned in inspector.
        if (SkipHint == null)
        {
            SkipHint = DialogueBox.transform.Find("SkipHint")?.GetComponent<TextMeshProUGUI>()!;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }
        // Palette UX: Apply subtle outlines to dialogue text for improved accessibility and legibility.
        SpeakerNameText.outlineWidth = 0.2f;
        SpeakerNameText.outlineColor = Color.black;
        DialogueText.outlineWidth = 0.2f;
        DialogueText.outlineColor = Color.black;
        // PALETTE: Capture original scale for animations
        _originalSpeakerScale = SpeakerNameText.transform.localScale;

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
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

        // PALETTE: Trigger "Pop" scale effect when speaker changes for visual delight
        if (speaker != _lastSpeaker)
        {
            if (_popCoroutine != null) StopCoroutine(_popCoroutine);
            _popCoroutine = StartCoroutine(PopEffect(SpeakerNameText.transform, _originalSpeakerScale));
        }
        _lastSpeaker = speaker;

        // Palette UX: Visual pop feedback for speaker transition.
        if (SpeakerNameText.text != speaker) StartCoroutine(PopScale(SpeakerNameText.transform));
        SpeakerNameText.text = speaker;

        // Reset idle timer and hint state for each new dialogue line
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Apply speaker-specific speed multipliers based on voice profiles
        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        public void ShowDialogue(string speaker, string message)
        {
            case "Sky.ix":
                SpeakerNameText.color = Color.cyan;
                currentSpeakerHex = "#00FFFF";
                break;
            case "Kai":
                SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                currentSpeakerHex = "#FFD700";
                break;
            case "Delilah":
                SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                currentSpeakerHex = "#991AE6";
                break;
            default:
                SpeakerNameText.color = Color.white;
                currentSpeakerHex = "#FFFFFF";
                break;
        }
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (popCoroutine != null) StopCoroutine(popCoroutine);

            SpeakerNameText.text = speaker;
            popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform));

    private IEnumerator PopEffect(Transform target, Vector3 baseScale)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;
            // Subtle pop using a sine wave: scales up to 1.1x and back down
            float curve = Mathf.Sin(percent * Mathf.PI);
            target.localScale = baseScale * (1f + 0.1f * curve);
            yield return null;
        }
        target.localScale = baseScale;
    }

    private IEnumerator TypeDialogue(string message)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
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
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
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
                float delay = currentTypingSpeed;

                if (i > 0)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEllipsis = false;
                        if (c == '.')
                        {
                            if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                            if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                        {
                            delay = currentTypingSpeed;
                        }
                        else delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':') delay = currentTypingSpeed * 8f;
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
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;

                    // Ellipsis detection: Use a faster delay for consecutive dots
                    bool isEllipsis = (c == '.' && i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                     (c == '.' && i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                    if (isEllipsis)
                    {
                        delay = currentTypingSpeed * 5f;
                    }
                    else if (c == '.' || c == '!' || c == '?')
                    {
                        // Look-ahead: Don't pause for mid-word periods (e.g., Sky.ix)
                        bool isMidWord = i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);
                        if (!isMidWord) delay = currentTypingSpeed * 15f;
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

        skipRequested = false;
        // UX Enhancement: Color-coded visual progression cue indicating text reveal is complete.
        DialogueText.text = $"{message} <color={currentSpeakerHex}>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;
        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return WaitForSecondsOrSkip(1.0f);

        // Unified Skip: Removed 'skipRequested = false' here to allow it to persist through the subsequent wait.
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras, set initial character states]
        // Example: PlayerInput.Instance.DisableControls();
        // Example: CinematicCamera.SetActive(true);

        DialogueBox.SetActive(true);
        yield return GetWait(1.0f);

        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        yield return GetWait(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        yield return GetWait(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        yield return GetWait(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        yield return GetWait(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(4.5f);

        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Dash_Forward");]
        // [VFX: Play glitchy dash particle trail from Sky.ix's starting position to the conduit.]
        // [CAMERA: Fast dolly track, following Sky.ix's movement. Add motion blur.]
        // [SFX: Play sound of cybernetic dash and energy whoosh.]
        yield return GetWait(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        // [CAMERA: Cut to Kai, a holographic display in front of them shows a massive energy spike warning.]
        yield return GetWait(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        // [CAMERA: Wide shot showing Sky.ix nearing the objective, with Delilah in the background, arms spread in a mocking invitation.]
        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        // [CAMERA: Extreme close-up on Sky.ix's eyes, reflecting the corrupted energy, but her expression is resolute.]
        yield return GetWait(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to gameplay/boss fight]
        // Example: PlayerInput.Instance.EnableControls();
        // Example: CinematicCamera.SetActive(false);
        // Example: BossFightController.StartFight();
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
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
