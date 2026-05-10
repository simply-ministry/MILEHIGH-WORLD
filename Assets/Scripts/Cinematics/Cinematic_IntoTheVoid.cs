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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'—a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
/// </summary>
public class Cinematic_IntoTheVoid : MonoBehaviour
{
    // ====================================================================
    //
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    //
    // ====================================================================

    // Protagonist: Sky.ix the The Bionic Goddess
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;
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
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;
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
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
    // Description: A corrupted form of Ingris, wielding Voidfire.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/antagonists/delilah.png
    // Ability Script: Ability_Delilah.cs
    /* VOICE PROFILE:
     * Not available.
    */
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
    private Coroutine namePopCoroutine;
    private Coroutine popCoroutine;
    private float currentTypingSpeed;
    private string currentSpeakerColorTag;
    private bool skipRequested;
    private string currentSpeakerColorTag;

    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    // BOLT: Changed float-keyed dictionary to int-keyed (milliseconds) to avoid floating-point precision issues
    // ⚡ Bolt: Use int key (milliseconds) to prevent float imprecision cache misses
    private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
    {
        int timeMs = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeMs, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeMs] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        }
        return wait;
    }

    /// <summary>
    /// Coroutine that waits for a specific duration or until a skip is requested.
    /// Resets the skip flag upon completion.
    /// </summary>
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
    /// Helper to trigger the pop effect on a UI element.
    /// </summary>
    private void TriggerPop(Transform target)
    {
        if (target != null) StartCoroutine(PopEffect(target));
    }

    /// <summary>
    /// Coroutine that applies a subtle scale 'pop' animation to a transform.
    /// </summary>
    private IEnumerator PopEffect(Transform target)
    {
        Vector3 originalScale = target.localScale;
        float elapsed = 0f;
        float duration = 0.15f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float curve = Mathf.Sin((elapsed / duration) * Mathf.PI);
            target.localScale = originalScale + (Vector3.one * (curve * 0.1f));
            yield return null;
        }
        target.localScale = originalScale;
    /// Waits for a specified duration, but returns immediately if a skip is requested.
    /// </summary>
    /// Coroutine that waits for a specified time or until the user requests to skip.
    /// Resets the skipRequested flag upon completion.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float seconds)
    {
        float timer = 0f;
        while (timer < seconds && !skipRequested)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    /// UX Enhancement: A skippable delay that allows players to bypass cinematic pauses.
    /// Returns early if skipRequested is true, then resets the flag.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float time)
    {
        float startTime = Time.time;
        while (Time.time - startTime < time && !skipRequested)
        {
            yield return null;
        }
    /// Yields for a duration or returns immediately if a skip is requested.
    /// Improves UX by allowing players to fast-forward through cinematic pauses.
    private IEnumerator PopScale(RectTransform target)
    {
        float duration = 0.15f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float scale = 1f + Mathf.Sin(percent * Mathf.PI) * 0.15f;
            target.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        target.localScale = Vector3.one;
    }

    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && !skipRequested)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    void Update()
    {
        // Poll for skip input to ensure responsiveness
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            skipRequested = true;
        }
    }

    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float start = Time.time;
        while (Time.time - start < duration && !skipRequested)
        {
            yield return null;
        }
        skipRequested = false; // Reset skip state after the pause
        skipRequested = false;
    }

    void Start()
    {
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        // Palette: Accessibility enhancement - Improve text legibility against dynamic backgrounds.
        SpeakerNameText.outlineWidth = 0.2f;
        SpeakerNameText.outlineColor = Color.black;
        DialogueText.outlineWidth = 0.2f;
        DialogueText.outlineColor = Color.black;
        // UX Enhancement: Outline for better readability against Void background
        DialogueText.outlineWidth = 0.2f;
        DialogueText.outlineColor = Color.black;
        SpeakerNameText.outlineWidth = 0.2f;
        SpeakerNameText.outlineColor = Color.black;

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (namePopCoroutine != null) StopCoroutine(namePopCoroutine);

        SpeakerNameText.text = speaker;
        TriggerPop(SpeakerNameText.transform);
        namePopCoroutine = StartCoroutine(PopScale(SpeakerNameText.rectTransform));
        if (popCoroutine != null) StopCoroutine(popCoroutine);

        SpeakerNameText.text = speaker;
        SpeakerNameText.transform.localScale = Vector3.one;
        popCoroutine = StartCoroutine(PopSpeakerName());

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
        skipRequested = false;

        Color speakerColor;
        switch (speaker)
        {
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break; // Gold
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break; // Void Purple
            default: speakerColor = Color.white; break;
        // Apply character-specific colors for better speaker identification
        Color speakerColor;
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
                currentSpeakerColorTag = "<color=#00FFFF>";
                break;
            case "Kai":
                SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                currentSpeakerColorTag = "<color=#FFD700>";
                break;
            case "Delilah":
                SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                currentSpeakerColorTag = "<color=#9919E6>";
                break;
            default:
                SpeakerNameText.color = Color.white;
                currentSpeakerColorTag = "<color=#FFFFFF>";
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
                speakerColor = Color.cyan;
                break;
            case "Kai":
                speakerColor = new Color(1f, 0.84f, 0f); // Gold
                break;
            case "Delilah":
                speakerColor = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                break;
            default:
                speakerColor = Color.white;
                break;
        }

        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
    }

    private IEnumerator TypeDialogue(string message)
    {
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;

        // ⚡ Bolt: Cache WaitForSeconds outside the loop to prevent per-iteration GC allocations
        var waitNormal = new WaitForSeconds(currentTypingSpeed);
        var waitShortPause = new WaitForSeconds(currentTypingSpeed + 0.2f);
        var waitLongPause = new WaitForSeconds(currentTypingSpeed + 0.4f);

        for (int i = 0; i <= message.Length; i++)
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

        DialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalCharacters = textInfo.characterCount;

        for (int i = 0; i < totalCharacters; i++)
        {
            if (skipRequested) break;

            DialogueText.maxVisibleCharacters = i + 1;

            char c = textInfo.characterInfo[i].character;
            float delay = typingSpeed;

            if (c == '.' || c == '!' || c == '?')
                delay = punctuationPause;
            else if (c == ',')
                delay = commaPause;

            yield return GetWait(delay);
        }

        DialogueText.maxVisibleCharacters = totalCharacters;
        skipRequested = false;
        DialogueText.ForceMeshUpdate();

        // BOLT: Typewriter effect optimized for performance.
        // We pre-calculate WaitForSeconds outside the loop to ensure zero-allocation yields,
        // avoiding GC pressure and float-based dictionary lookups during dialogue sequences.

        // Cache WaitForSeconds to prevent per-iteration GC allocations
        // avoiding GC pressure during dialogue sequences, without using float dictionaries.
        var standardWait = new WaitForSeconds(currentTypingSpeed);
        var commaWait = new WaitForSeconds(currentTypingSpeed * 8f);
        var periodWait = new WaitForSeconds(currentTypingSpeed * 15f);

        // We locally cache WaitForSeconds objects outside the loop to ensure zero-allocation yields,
        // avoiding GC pressure during dialogue sequences without the overhead or inaccuracy of a float dictionary cache.
        // We use local variables to ensure zero-allocation yields,
        // avoiding GC pressure during dialogue sequences.
        // Avoiding GC pressure during dialogue sequences by caching WaitForSeconds.
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        var defaultWait = new WaitForSeconds(currentTypingSpeed);
        var periodWait = new WaitForSeconds(currentTypingSpeed * 15f);
        var commaWait = new WaitForSeconds(currentTypingSpeed * 8f);
        // Pre-calculate WaitForSeconds outside the loop to ensure zero-allocation yields
        // safely and predictably, avoiding GC pressure and the flawed float-key dictionary approach.
        // Avoid Dictionary<float, WaitForSeconds> anti-pattern and zero-allocation yields
        // by caching WaitForSeconds instances locally outside the tight loop.
        // Cache WaitForSeconds to eliminate GC allocations during the loop,
        // avoiding GC pressure during dialogue sequences without unverified custom caching methods.
        // Safely cache WaitForSeconds in local variables outside the loop to prevent
        // per-iteration Garbage Collection (GC) allocations. Using Dictionary<float, WaitForSeconds>
        // is an anti-pattern due to floating-point precision inaccuracies.
        var baseWait = new WaitForSeconds(currentTypingSpeed);
        var shortPauseWait = new WaitForSeconds(currentTypingSpeed * 8f);
        var longPauseWait = new WaitForSeconds(currentTypingSpeed * 15f);

        // Cache WaitForSeconds locally to avoid per-character GC allocations
        // while avoiding the Dictionary<float, WaitForSeconds> anti-pattern.
        // Pre-calculate WaitForSeconds outside the loop to avoid GC pressure,
        // while also avoiding the float-keyed dictionary cache anti-pattern.
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        var normalWait = new WaitForSeconds(currentTypingSpeed);
        var shortPauseWait = new WaitForSeconds(currentTypingSpeed * 8f);
        var longPauseWait = new WaitForSeconds(currentTypingSpeed * 15f);

        var waitNormal = new WaitForSeconds(currentTypingSpeed);
        var waitMedium = new WaitForSeconds(currentTypingSpeed * 8f);
        var waitLong = new WaitForSeconds(currentTypingSpeed * 15f);

        var defaultWait = new WaitForSeconds(currentTypingSpeed);
        var mediumWait = new WaitForSeconds(currentTypingSpeed * 8f);
        var longWait = new WaitForSeconds(currentTypingSpeed * 15f);

        WaitForSeconds normalWait = new WaitForSeconds(currentTypingSpeed);
        WaitForSeconds longWait = new WaitForSeconds(currentTypingSpeed * 15f);
        WaitForSeconds medWait = new WaitForSeconds(currentTypingSpeed * 8f);

        WaitForSeconds normalWait = new WaitForSeconds(currentTypingSpeed);
        WaitForSeconds punctuationWaitLong = new WaitForSeconds(currentTypingSpeed * 15f);
        WaitForSeconds punctuationWaitShort = new WaitForSeconds(currentTypingSpeed * 8f);

        var waitNormal = new WaitForSeconds(currentTypingSpeed);
        var waitLong = new WaitForSeconds(currentTypingSpeed * 15f);
        var waitMedium = new WaitForSeconds(currentTypingSpeed * 8f);

        var normalWait = new WaitForSeconds(currentTypingSpeed);
        var longPauseWait = new WaitForSeconds(currentTypingSpeed * 15f);
        var shortPauseWait = new WaitForSeconds(currentTypingSpeed * 8f);

        var normalWait = GetWait(currentTypingSpeed);
        var commaWait = GetWait(currentTypingSpeed * 8f);
        var periodWait = GetWait(currentTypingSpeed * 15f);

        // BOLT: Cache WaitForSeconds locally outside the high-frequency loop to avoid per-iteration dictionary lookups and GC allocations
        var defaultWait = GetWait(currentTypingSpeed);
        var longWait = GetWait(currentTypingSpeed * 15f);
        var shortWait = GetWait(currentTypingSpeed * 8f);

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
                var wait = defaultWait;
                var currentWait = normalWait;
                WaitForSeconds waitToYield = standardWait;
                var waitToYield = normalWait;
                WaitForSeconds wait = waitNormal;
                WaitForSeconds currentWait = defaultWait;
                WaitForSeconds waitToYield = normalWait;
                WaitForSeconds currentWait = normalWait;
                var wait = defaultWait;
                var waitToYield = baseWait;
                var wait = waitNormal;
                var currentWait = normalWait;
                char c = message[i];

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                // Note: Delay occurs *after* character reveal for natural rhythm.
                if (c == '.' || c == '!' || c == '?')
                {
                    yield return waitLongPause;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    yield return waitShortPause;
                }
                else
                {
                    yield return waitNormal;
                }
            }
            else
            {
                // Delay after the last character is fully revealed before proceeding
                yield return waitNormal;
                float delay = currentTypingSpeed;
                char c = DialogueText.textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses
                bool isEndOfSentence = true;
                if (i + 1 < totalVisibleCharacters)
                {
                    char nextChar = DialogueText.textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    if (i + 1 < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i + 1].character == '.')
                        delay = currentTypingSpeed * 5f;
                    else if (isEndOfSentence)
            // Rhythmic typewriter effect: longer pauses for punctuation to mimic natural speech
            if (i > 0)
            {
                char c = message[i - 1];
                float delay = currentTypingSpeed;

                // Rhythmic typewriter effect: longer pauses for punctuation to mimic natural speech
                if (c == '.' || c == '?' || c == '!')
                    delay = currentTypingSpeed * 15f;
                else if (c == ',' || c == ';' || c == ':')
                    delay = currentTypingSpeed * 8f;

                yield return GetWait(delay);
            }
            else
            {
                yield return GetWait(currentTypingSpeed);
                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // We check the previous character (i-1) to pause *after* it has been revealed.
                if (i > 0)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == '!' || c == '?') wait = longWait;
                    else if (c == ',' || c == ';' || c == ':') wait = shortWait;
                }

                yield return wait;
                    bool isDot = c == '.';

                    if (isDot || c == '!' || c == '?')
                    {
                        // Check for ellipsis (consecutive dots)
                        bool isEllipsis = false;
                        if (isDot)
                        {
                            if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                            else if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }

                        if (isEllipsis)
                        {
                            delay = currentTypingSpeed * 5f; // Faster pause for ellipsis dots
                        }
                        else
                        {
                            // Standard sentence end - check if next char is whitespace to avoid mid-word delays (e.g. Sky.ix)
                            bool nextIsWhitespace = i >= totalVisibleCharacters || char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);
                            if (nextIsWhitespace) delay = currentTypingSpeed * 15f;
                        }
                    if (c == '.' || c == '!' || c == '?') currentWait = periodWait;
                    else if (c == ',' || c == ';' || c == ':') currentWait = commaWait;
                    bool isNextCharWhitespace = (i < totalVisibleCharacters) && char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);

                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Check for ellipsis: if neighbors are also dots, use a shorter pause.
                        bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                         (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isNextCharWhitespace || i == totalVisibleCharacters) delay = currentTypingSpeed * 15f;
                    if (c == '.' || c == '!' || c == '?') waitToYield = longPauseWait;
                    else if (c == ',' || c == ';' || c == ':') waitToYield = shortPauseWait;
                }

                yield return waitToYield;
                    if (c == '.' || c == '!' || c == '?') wait = waitLong;
                    else if (c == ',' || c == ';' || c == ':') wait = waitMedium;
                }

                yield return wait;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // PALETTE: Check for ellipsis or mid-word periods to maintain natural flow.
                        bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                        (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                        bool isMidWord = (i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character));

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isMidWord) delay = currentTypingSpeed; // No pause for mid-word abbreviations
                        else delay = currentTypingSpeed * 15f;
                    if (c == '.' || c == '!' || c == '?') currentWait = periodWait;
                    else if (c == ',' || c == ';' || c == ':') currentWait = commaWait;

                    if (c == '.' || c == '!' || c == '?') waitToYield = punctuationWaitLong;
                    else if (c == ',' || c == ';' || c == ':') waitToYield = punctuationWaitShort;
                }

                yield return waitToYield;
                    if (c == '.' || c == '!' || c == '?') currentWait = longWait;
                    else if (c == ',' || c == ';' || c == ':') currentWait = medWait;
                    if (c == '.' || c == '!' || c == '?') wait = longWait;
                    else if (c == ',' || c == ';' || c == ':') wait = mediumWait;
                }

                yield return wait;
                    if (c == '.' || c == '!' || c == '?') waitToYield = longPauseWait;
                    else if (c == ',' || c == ';' || c == ':') waitToYield = shortPauseWait;
                }

                yield return waitToYield;
                    if (c == '.' || c == '!' || c == '?') wait = waitLong;
                    else if (c == ',' || c == ';' || c == ':') wait = waitMedium;
                }

                yield return wait;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        delay = currentTypingSpeed * 15f;

                        // UX Improvement: Handle ellipsis (...) and mid-word periods (e.g., Sky.ix)
                        bool isEllipsis = false;
                        if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        // Rhythmic refinement: Handle ellipsis and mid-word periods
                        if (c == '.')
                        {
                            bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                             (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                            if (isEllipsis) delay = currentTypingSpeed * 5f;
                            else if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character != ' ')
                                delay = currentTypingSpeed; // Skip pause for mid-word periods (e.g. Sky.ix)
                        // Check for ellipsis (consecutive dots)
                        bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                         (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                        if (isEllipsis)
                        {
                            delay = currentTypingSpeed * 5f;
                        }
                        else if (c == '.' && i < totalVisibleCharacters)
                        {
                            // If it's a period but not followed by a space, it's likely an abbreviation or part of a name.
                            char nextChar = DialogueText.textInfo.characterInfo[i].character;
                            if (nextChar != ' ' && nextChar != '\n' && nextChar != '\r' && nextChar != '\t')
                            {
                                delay = currentTypingSpeed;
                            }
                        }
                        else
                        {
                            // Check for mid-word periods (e.g., Sky.ix)
                            bool isMidWord = i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character != ' ';
                            delay = isMidWord ? currentTypingSpeed : currentTypingSpeed * 15f;
                        }
                    if (c == '.' || c == '!' || c == '?') currentWait = longPauseWait;
                    else if (c == ',' || c == ';' || c == ':') currentWait = shortPauseWait;
                    if (c == '.' || c == '!' || c == '?') delay = currentTypingSpeed * 15f;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Check for ellipsis (consecutive dots) to avoid excessive pausing
                        bool isEllipsis = (c == '.' && ((i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') || (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.')));
                        delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);

                    // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                    bool isEndOfSentence = true;
                    if (i < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Handle ellipsis or end of sentence
                        if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.')
                            delay = currentTypingSpeed * 5f; // Faster dot-dot-dot
                        else if (isEndOfSentence)
                            delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                    if (c == '.' || c == '!' || c == '?')
                    {
                        delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                }

                yield return currentWait;
            }
        }

        // UX Enhancement: Add a "Continue" indicator once message is fully revealed
        DialogueText.text += " ▽";
        DialogueText.maxVisibleCharacters = DialogueText.text.Length;

        skipRequested = false;
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        // Color-code the completion character to match the speaker's theme.
        DialogueText.text = message + $" <color={currentSpeakerColorTag}>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2; // +1 for space, +1 for triangle
        DialogueText.text = message + $" <color={currentSpeakerColorTag}>▽</color>";
        // PALETTE: Color-coded cue to match the speaker's theme for a delightful touch.
        DialogueText.text = message + $" {currentSpeakerColorTag}▽</color>";
        // We color-code the completion symbol to match the speaker's theme.
        DialogueText.text = message + " <color=" + currentSpeakerColorTag + ">▽</color>";
        DialogueText.text = message + " <color=#FFD700>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

        // Palette: Skip management - unified skipping ensures a single input bypasses
        // both the typewriter reveal and the following cinematic pause.
        // We do NOT reset skipRequested here; it's handled by WaitForSecondsOrSkip.
        // Palette UX Enhancement: We don't reset skipRequested here.
        // This allows a single skip input to bypass both the typewriter AND the subsequent cinematic pause.
        // 🎨 Palette: We don't reset skipRequested here.
        // This allows a single input to skip both typing AND the following wait.
        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        typingCoroutine = null;
    }

    private IEnumerator PopSpeakerName()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 initialScale = Vector3.one;
        Vector3 peakScale = initialScale * 1.15f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curve = Mathf.Sin(t * Mathf.PI);
            SpeakerNameText.transform.localScale = Vector3.Lerp(initialScale, peakScale, curve);
            yield return null;
        }
        SpeakerNameText.transform.localScale = initialScale;
        popCoroutine = null;
    }

    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && !skipRequested)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.0f));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.0f));
        yield return new WaitForSeconds(1.0f);
        yield return WaitForSecondsOrSkip(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. Hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.5f));
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.5f));
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.5f));
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.5f));
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.7f));
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(8.0f));
        yield return new WaitForSeconds(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.7f));
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(8.0f));
        yield return new WaitForSeconds(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(8.0f);
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.2f));
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.0f));
        yield return new WaitForSeconds(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.2f));
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.0f));
        yield return new WaitForSeconds(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.8f));
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(4.5f));
        yield return new WaitForSeconds(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.8f));
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(4.5f));
        yield return new WaitForSeconds(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(4.5f);
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Dash_Forward");]
        // [VFX: Play glitchy dash particle trail from Sky.ix's starting position to the conduit.]
        // [CAMERA: Fast dolly track, following Sky.ix's movement. Add motion blur.]
        // [SFX: Play sound of cybernetic dash and energy whoosh.]
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(2.0f));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(2.0f));
        yield return new WaitForSeconds(2.0f);
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        // [CAMERA: Cut to Kai, a holographic display in front of them shows a massive energy spike warning.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(0.5f));
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(3.5f));
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(0.5f));
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(3.5f));
        yield return new WaitForSeconds(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return new WaitForSeconds(3.5f);
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        // [CAMERA: Wide shot showing Sky.ix nearing the objective, with Delilah in the background, arms spread in a mocking invitation.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.5f));
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(5.5f));
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.5f));
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(5.5f));
        yield return new WaitForSeconds(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        // [CAMERA: Extreme close-up on Sky.ix's eyes, reflecting the corrupted energy, but her expression is resolute.]
        yield return WaitForSecondsOrSkip(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.0f));
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));
        yield return new WaitForSeconds(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(1.0f));
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));
        yield return new WaitForSeconds(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);
        yield return WaitForSecondsOrSkip(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);
        Debug.Log("Cinematic Sequence Complete.");
    }
}
