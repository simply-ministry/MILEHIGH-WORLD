// --- UNITY SCENE SETUP --- //
//
// 1. Create an empty GameObject in your scene and name it "SceneController".
// 2. Attach this script (Cinematic_IntoTheVoid.cs) to it.
// 3. Configure character prefabs (Sky.ix, Kai, Delilah) with Animators and AudioSources.
// 4. Setup UI: Canvas -> DialogueBox (Panel) -> SpeakerNameText (TMP), DialogueText (TMP), SkipHint (TMP).
// 5. Drag and drop references into the Inspector.

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
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
/// Controls the cinematic sequence for "Deep within the anti-reality of ŤĤÊ VØĪĐ".
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of THE VOID, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'--a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
/// </summary>
public class Cinematic_IntoTheVoid : MonoBehaviour
namespace Milehigh.Cinematics
{
    [Header("Character Assets")]

    [Header("Character References")]
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
    // ====================================================================
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    // ====================================================================

    [Header("Character References")]
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;

    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;

    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
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
    public GameObject DialogueBox = null!;
    public CanvasGroup? DialogueCanvasGroup;
    public TextMeshProUGUI SpeakerNameText = null!;
    public TextMeshProUGUI DialogueText = null!;
    public TextMeshProUGUI SkipHint = null!;
    public TextMeshProUGUI? SkipHint;
    public TextMeshProUGUI SkipHint = null!;
    public GameObject DialogueBox;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI DialogueText;

    [Header("UX Settings")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    [FormerlySerializedAs("typingSpeed")]
    public float baseTypingSpeed = 0.03f;
    public float kaiSpeedMultiplier = 3.0f;
    public float skyixSpeedMultiplier = 1.2f;
    public float idleHintThreshold = 2.0f;

    private Coroutine? typingCoroutine;
    private CanvasGroup? dialogueCanvasGroup;
    private Coroutine typingCoroutine;
    private Coroutine namePopCoroutine;
    private string lastSpeaker;
    private float currentTypingSpeed;
    private string currentSpeakerHex;
    private bool skipRequested;
    private float idleTimer;
    private bool playerInteracted;

    // BOLT: Shared cache for WaitForSeconds to eliminate GC allocations
    private void Update()
    {
        // UX Enhancement: Standardized skip logic for both keyboard and mouse
    // Cache for WaitForSeconds to eliminate GC allocations
    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    // ⚡ Bolt: Using int keys (milliseconds) prevents float precision cache misses and redundant GC allocations.
    // ⚡ Bolt: Cache for WaitForSeconds using integer keys (milliseconds) to eliminate GC allocations and floating-point cache misses
    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();
    private Vector3 _originalSpeakerScale;
    private string _lastSpeaker;
    private Coroutine _popCoroutine;

    void Update()
    {
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
    {
        // Poll for skip input to ensure responsiveness across multiple accessible inputs
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    // ⚡ Bolt: Use int (milliseconds) instead of float for dictionary key to avoid cache misses from float imprecision
    // ⚡ Bolt: Using int (milliseconds) instead of float for dictionary keys prevents cache misses from float imprecision
    // BOLT: Changed to int (milliseconds) key to prevent cache misses from float precision issues
    // BOLT: Refactored dictionary to use int (milliseconds) instead of float to prevent precision-based cache misses
    // ⚡ Bolt: Use int (milliseconds) for the key to avoid float imprecision cache misses
    private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
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

        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int ms = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[ms] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeMs = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeMs, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeMs] = wait;
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
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Awake()
    {
        // Automated component lookup for CanvasGroup if not assigned
        if (DialogueCanvasGroup == null && DialogueBox != null)
        {
            DialogueCanvasGroup = DialogueBox.GetComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        // Initialize UI state
        if (DialogueCanvasGroup != null) DialogueCanvasGroup.alpha = 0;
        DialogueBox.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Awake()
    {
        if (DialogueBox != null)
        {
            dialogueCanvasGroup = DialogueBox.GetComponent<CanvasGroup>();
            if (dialogueCanvasGroup == null) dialogueCanvasGroup = DialogueBox.AddComponent<CanvasGroup>();
            dialogueCanvasGroup.alpha = 0f;
        }
    }

    void Start()
    {
        // 🛡️ Sentinel: Defensive programming to prevent NullReferenceException
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Update()
    {
        if (Input.anyKeyDown) skipRequested = true;
    }

    private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
    {
        if (dialogueCanvasGroup == null) yield break;
        float startAlpha = dialogueCanvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        if (Input.anyKeyDown || (Input.GetMouseButtonDown(0)))
        {
            skipRequested = true;
            idleTimer = 0;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2.0f && !playerInteracted && SkipHint != null && DialogueBox != null && DialogueBox.activeInHierarchy)
            {
        // Poll for skip input to ensure responsiveness
        // 🎨 Palette: Prefer specific keys for skip to avoid accidental triggers
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
    private IEnumerator WaitForSecondsOrSkip(float time)
    {
        // UX: Support all input types (keyboard, mouse, gamepad) for cinematic skipping
        if (Input.anyKeyDown) skipRequested = true;
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
            playerInteracted = true;
            idleTimer = 0;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
    }

        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2f && !playerInteracted && SkipHint != null)
            {
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
            if (SpeakerNameText != null)
                originalSpeakerScale = SpeakerNameText.transform.localScale;
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
            elapsed += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        dialogueCanvasGroup.alpha = targetAlpha;
    }

    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float start = Time.time;
        while (Time.time - start < duration && !skipRequested) yield return null;
        skipRequested = false;
    }

    private void Start()
    {
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
        // Note: skipRequested is NOT reset here to allow skipping both the reveal and the subsequent pause
    }


        DialogueCanvasGroup.alpha = targetAlpha;
        if (targetAlpha <= 0) DialogueBox.SetActive(false);
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
        //  Sentinel: Security enhancement - Defensive programming
        // Ensure UI components are assigned to prevent NullReferenceException and potential stack trace leakage.
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        private void Update()
        {
            Debug.LogError("Missing UI components! Cinematic aborted.");
            return;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        if (SkipHint == null)
        {
            // Fallback: try to find it in children if not assigned
            SkipHint = DialogueBox.GetComponentInChildren<TextMeshProUGUI>(true);
            if (SkipHint == SpeakerNameText || SkipHint == DialogueText) SkipHint = null!;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    /// <summary>
    /// Updates the speaker name and begins the typewriter effect for the dialogue message.
    /// </summary>
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Palette UX: Improve text contrast in the dark "Void" environment via outlines.
        DialogueText.fontMaterial.EnableKeyword("OUTLINE_ON");
        DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);

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

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
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
        // Apply speaker-specific speed multipliers and colors
        float multiplier = 1.0f;
        Color speakerColor = Color.white;

        // Reset idle timer and hint state for each new dialogue line
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
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

        // Apply character-specific colors for better speaker identification
        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };
        Color speakerColor;
        switch (speaker)
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Apply character-specific colors for better speaker identification
        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };

        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break; // Gold
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break; // Void Purple
            default: speakerColor = Color.white; break;
        }
        }
        SpeakerNameText.color = speakerColor;
        public void ShowDialogue(string speaker, string message)
        {
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break;
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break;
            default: speakerColor = Color.white; break;
            case "Sky.ix":
                multiplier = skyixSpeedMultiplier;
                speakerColor = Color.cyan;
                break;
            case "Kai":
                multiplier = kaiSpeedMultiplier;
                speakerColor = new Color(1f, 0.84f, 0f); // Gold
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

        SpeakerNameText.color = speakerColor;
        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        typingCoroutine = StartCoroutine(TypeDialogue(message, speakerColor));
    }

    private IEnumerator TypeDialogue(string message, Color themeColor)
    {
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        currentSpeakerHex = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        typingCoroutine = StartCoroutine(TypeDialogue(message));
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

    private IEnumerator PopScale(Transform target, float duration, float multiplier)
    {
        Vector3 initialScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            // Use a sine wave for a smooth 'pop' effect (scaling up and back down)
            float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI) * (multiplier - 1f);
            target.localScale = initialScale * scaleFactor;
            yield return null;
        }

        target.localScale = initialScale;
        namePopCoroutine = null;
    }

    private IEnumerator TypeDialogue(string message, Color color)
    {
        // BOLT: Efficient typewriter reveal using maxVisibleCharacters
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();


        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalCharacters = textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = totalCharacters;
                break;
            if (skipRequested) break;

        int totalCharacters = DialogueText.textInfo.characterCount;
        // The cue ▽ is usually the last character
        int textOnlyCount = totalCharacters - 1;

        for (int i = 0; i <= textOnlyCount; i++)
        {
            if (skipRequested) break;
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            // ⚡ Bolt: Use centralized UnityUtils.GetWait to avoid GC allocations
            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                bool isEndOfSentence = true;
                if (i + 1 < totalCharacters)
                {
                    char nextChar = textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence) delay *= 15f;
                else delay *= 5f; // Faster pause for mid-word periods (e.g. Sky.ix)
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        // Calculation: characterCount includes the completion cue '▽'.
        // We only want to apply rhythmic pacing to the message itself.
        int messageVisibleCount = totalVisibleCharacters - 2; // Subtracting ' ▽'

        for (int i = 0; i <= totalVisibleCharacters; i++)
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
            DialogueText.maxVisibleCharacters = i + 1;

            if (i < totalCharacters)
            if (i > 0 && i < totalVisibleCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
            if (i < totalCharacters - 1) // Don't delay after the last character (cue)
            if (i > 0 && i <= textOnlyCount)
            if (i > 0 && i <= totalVisibleCharacters)
            {
                char c = textInfo.characterInfo[i].character;
                float delay = currentTypingSpeed;
                char c = textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalCharacters && !char.IsWhiteSpace(textInfo.characterInfo[i + 1].character))
                        isEndOfSentence = false;

                    delay = currentTypingSpeed * (isEndOfSentence ? 15f : 1f);
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                char c = DialogueText.textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') || (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');
                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (i < totalVisibleCharacters && char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character)) delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':') delay = currentTypingSpeed * 8f;
                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // We check the character at current index (i) because it just became visible.
                if (c == '.' || c == '!' || c == '?')
                {
                    // Smart Punctuation: Look ahead/behind to refine pacing
                    bool isEllipsis = (c == '.' && ((i > 0 && textInfo.characterInfo[i - 1].character == '.') || (i < totalCharacters - 2 && textInfo.characterInfo[i + 1].character == '.')));
                    bool isMidWordPeriod = (c == '.' && i < totalCharacters - 2 && !char.IsWhiteSpace(textInfo.characterInfo[i + 1].character));

                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (isMidWordPeriod) delay = currentTypingSpeed;
                    else delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i+1].character))
                        isEndOfSentence = false;

                    if (isEndOfSentence) delay += 0.4f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay += 0.2f;
                // Note: Delay occurs AFTER character reveal for natural rhythm.
                if (i > 0)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    else delay = currentTypingSpeed * 5f; // Potential mid-word or ellipsis
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                char c = DialogueText.textInfo.characterInfo[i - 1].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    SkipHint.text = "[Space] Skip";
                    SkipHint.gameObject.SetActive(true);
                }
            }
        }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
            if (i < totalCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
                float delay = currentTypingSpeed;
                char c = textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalCharacters)
                    {
                        char nextChar = textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
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
                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // Use characterInfo for robust detection that handles rich text correctly.
                if (i > 0 && i <= messageVisibleCount)
            currentTypingSpeed = baseTypingSpeed * multiplier;

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
                        // Handle ellipsis or end of sentence
                        delay = currentTypingSpeed * 15f;

                        // Refined ellipsis and mid-word detection
                        // Palette UX: Look ahead/back to distinguish sentence-end, ellipsis, or mid-word periods (e.g., Sky.ix).
                        bool isEllipsis = false;
                        if (c == '.' && i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;

                        if (isEllipsis)
                        {
                            delay = currentTypingSpeed * 5f; // Faster cadence for ellipsis
                        }
                        else
                        {
                            bool isMidWord = false;
                            if (c == '.' && i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character)) isMidWord = true;

                            if (!isMidWord) delay = currentTypingSpeed * 15f;
                        }
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                    if (c == '.' || c == '!' || c == '?')
                    {
                        delay = currentTypingSpeed * 15f;

                        // Look-ahead: avoid pauses for mid-word periods (e.g., Sky.ix)
                        if (c == '.' && i < totalVisibleCharacters)
                        {
                            char nextChar = DialogueText.textInfo.characterInfo[i].character;
                            if (!char.IsWhiteSpace(nextChar)) delay = currentTypingSpeed;
                        }

                        // Ellipsis detection: 5x delay for dots in '...'
                        bool isEllipsis = false;
                        if (c == '.')
                        {
                            if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                            if (i < messageVisibleCount && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (c == '.' && i < messageVisibleCount && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                            else if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }
                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                            if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                        else if (i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                        {
                            delay = currentTypingSpeed; // Mid-word (e.g. Sky.ix)
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

        // UX Enhancement: Visual progression cue
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.ForceMeshUpdate();
        DialogueText.maxVisibleCharacters = DialogueText.textInfo.characterCount;

        private IEnumerator PopScale(Transform target, float duration, float multiplier)
        {
            Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;
                // ⚡ Bolt: Use global yield cache to eliminate GC allocations
                yield return Milehigh.Core.UnityUtils.GetWait(delay);
                if (isEllipsis) delay *= 5f;
                else if (isEndOfSentence) delay *= 15f;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay *= 8f;
            }
            else if (totalVisibleCharacters > 0)
            {
                // Palette UX: Final pause after last punctuation for natural pacing before completion symbol.
                char lastC = DialogueText.textInfo.characterInfo[totalVisibleCharacters - 1].character;
                if (lastC == '.' || lastC == '!' || lastC == '?' || lastC == ',' || lastC == ';' || lastC == ':')
                {
                    yield return GetWait(currentTypingSpeed * 10f);
                }
            }
            else if (i == 0) yield return GetWait(currentTypingSpeed);

            yield return UnityUtils.GetWait(delay);
        }

        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        skipRequested = false;
        typingCoroutine = null;
    }


    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras, set initial character states]
        DialogueBox.SetActive(true);
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        string hexColor = ColorUtility.ToHtmlStringRGB(themeColor);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 1;

        skipRequested = false;
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
        // Note: skipRequested is NOT reset here to allow skipping the subsequent pause.
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        // Color-coded to the current speaker for a subtle touch of delight.
        DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";

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

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return StartCoroutine(FadeDialogueBox(1f, 0.5f));
        yield return FadeDialogueBox(1f, 0.5f);
        yield return WaitForSecondsOrSkip(1.0f);

        // --- Dialogue Line 1: Delilah ---
        yield return WaitForSecondsOrSkip(1.5f);
        yield return FadeDialogueBox(1.0f, 0.5f);
        yield return WaitForSecondsOrSkip(1.0f);

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return GetWait(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

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

        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        yield return GetWait(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        yield return WaitForSecondsOrSkip(0.7f);
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        yield return GetWait(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        yield return WaitForSecondsOrSkip(1.2f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        yield return GetWait(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        // ACTION: Sky.ix dashes towards the conduit
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        yield return GetWait(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);


        // --- ACTION: Sky.ix dashes towards the conduit ---
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);
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
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        yield return WaitForSecondsOrSkip(1.5f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        // [CAMERA: Wide shot showing Sky.ix nearing the objective, with Delilah in the background, arms spread in a mocking invitation.]
        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        yield return WaitForSecondsOrSkip(1.0f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        // [CAMERA: Extreme close-up on Sky.ix's eyes, reflecting the corrupted energy, but her expression is resolute.]
        yield return GetWait(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        yield return StartCoroutine(FadeDialogueBox(0f, 0.5f));
        yield return FadeDialogueBox(0f, 0.5f);

        Debug.Log("Cinematic Sequence Complete.");
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);
        yield return FadeDialogueBox(0.0f, 0.5f);

        Debug.Log("Cinematic Sequence Complete.");
        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to gameplay/boss fight]
        // Example: PlayerInput.Instance.DisableControls();
        // Example: CinematicCamera.SetActive(false);
        // Example: BossFightController.StartFight();
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of THE VOID...]");
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
