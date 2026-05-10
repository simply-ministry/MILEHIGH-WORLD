## 2025-05-15 - [Optimizing Typewriter Effects for Cinematic Pacing]
**Learning:** When implementing typewriter effects in Unity, using `TextMeshProUGUI.maxVisibleCharacters` is far more performant than manual string concatenation, as it avoids frequent layout rebuilds and memory allocations. Crucially, cinematic dialogue triggers should not always block the main sequence; by not yielding on the typewriter coroutine, we preserve the original pacing and synchronization of animations and sound while still providing a polished visual reveal.
**Action:** Always use `maxVisibleCharacters` for text animation in TMP and carefully consider whether a UX effect should block or run in parallel with sequence timers.
## 2026-03-19 - [Cinematic Typewriter Effect]
**Learning:** Using 'TextMeshProUGUI.maxVisibleCharacters' to reveal text character-by-character provides a smoother, more 'cinematic' feel for dialogue sequences compared to direct text updates. It also prevents layout recalculations that can occur when modifying the text string itself.
**Action:** Apply this 'ShowDialogue' and 'TypeDialogue' pattern for all dialogue-heavy cinematic sequences to ensure consistent pacing and visual polish.
## 2025-03-15 - Initial UX Exploration
**Learning:** This Unity project primarily contains C# scripts for a cinematic sequence. Most UI is handled via TMPro in Cinematic_IntoTheVoid.cs.
**Action:** Focus on improving the dialogue UI or interactions within the cinematic script.
## 2025-03-18 - [Typewriter Effect for Cinematics]
**Learning:** For dialogue-heavy cinematics in Unity, using TextMeshPro's `maxVisibleCharacters` is the most performant and layout-safe way to implement a typewriter effect. It prevents layout shifts and handles rich text tags (like colors or bolding) correctly by revealing the already-calculated characters rather than building the string character by character.
**Action:** Use `maxVisibleCharacters` in a coroutine and always manage coroutine references (`StopCoroutine`) to prevent overlapping text animations when dialogues change quickly.
## 2024-03-20 - [Cinematic Typewriter Effect]
**Learning:** Cinematic dialogue in Unity is more engaging when revealed via a typewriter effect using `TextMeshProUGUI.maxVisibleCharacters`, as it prevents layout rebuilds while maintaining pacing.
**Action:** Use a dedicated `TypeDialogue` coroutine and `ShowDialogue` helper to manage speaker transitions and text reveal animations.
## 2026-03-21 - [Typewriter Effect in Cinematics]
**Learning:** In cinematic dialogue sequences, revealing text character-by-character (typewriter effect) using `TextMeshProUGUI.maxVisibleCharacters` provides a more engaging and readable experience than instant text display. It allows players to follow the narrative pace and prevents them from being overwhelmed by large blocks of text appearing at once. This approach also avoids layout rebuilds during the reveal, ensuring a smooth visual experience.
**Action:** Use the `TypeDialogue` coroutine pattern with a standardized character reveal delay (e.g., 0.03s) for all cinematic dialogue in future mission implementations.
# Palette's Journal - MILEHIGH.WORLD

## 2025-05-15 - Initial UX Audit
**Learning:** Unity UI (TextMeshPro) dialogue sequences often feel static if text appears instantly. A typewriter effect adds a sense of "living" dialogue and helps users pace their reading.
**Action:** Implement a typewriter effect coroutine for the `Cinematic_IntoTheVoid` script.
## 2026-03-22 - [Unified Typewriter Effect in Unity TMP]
**Learning:** Consolidating multiple, often conflicting, dialogue implementations into a single 'ShowDialogue' pattern using 'TextMeshProUGUI.maxVisibleCharacters' significantly improves UI stability and performance. Reusing 'WaitForSeconds' objects via a cache (e.g., GetWait helper) is essential for minimizing GC pressure in character-by-character text reveal loops.
**Action:** Always prefer 'maxVisibleCharacters' over manual string concatenation for text reveal animations and implement a 'GetWait' caching pattern for all high-frequency coroutine yields.

## 2026-03-24 - [Natural Pacing and Skip Support for Cinematic Dialogue]
**Learning:** A "one-speed-fits-all" typewriter effect can feel robotic. Adding subtle delays after punctuation (periods, question marks, commas) significantly improves the "readability" and natural feel of dialogue. Furthermore, providing a responsive "skip to end" interaction is critical for accessibility and user agency. This is best implemented by polling for input in `Update()` to set a flag, ensuring the skip is caught even during long punctuation pauses.
**Action:** Implement `punctuationPause` and `commaPause` logic and an asynchronous `skipRequested` flag for all cinematic dialogue systems.
## 2024-05-20 - [Robust Dialogue Skip & Unity Serialization]
**Learning:** For responsive skip mechanics in Unity coroutines that include punctuation pauses, polling input in 'Update' to set a persistent flag is more reliable than per-frame 'Input' checks within the loop. Additionally, when renaming serialized UX settings (like 'typingSpeed'), using '[FormerlySerializedAs]' is critical to prevent data loss in the Unity Inspector.
**Action:** Use a 'skipRequested' flag pattern and 'FormerlySerializedAs' for all Unity UI/UX script refactors.
## 2026-03-23 - [Rhythmic Typewriter and Speaker Color-Coding]
**Learning:** Dialogue-heavy cinematics benefit from punctuation-aware typewriter timing (longer pauses after '.', '?', ',', etc.), which mimics the natural cadence of speech and improves readability. Additionally, color-coding speaker names provides an immediate visual cue for character identification, reducing cognitive load during fast-paced sequences.
**Action:** Implement rhythmic pauses in typewriter effects and use consistent character-specific color palettes for speaker UI to enhance immersion and accessibility.

## 2026-03-24 - [Dialogue Completion Indicator]
**Learning:** In narrative-driven interfaces, users often struggle to know exactly when a typewriter effect has finished, especially if the last sentence is short. Adding a visual "Continue" indicator (like a '▽' symbol) at the end of the reveal provides a clear affordance that the dialogue sequence is ready for the next interaction.
**Action:** Always append a completion symbol or change the UI state once a text reveal animation concludes to improve usability and reduce user uncertainty.
## 2025-05-20 - [Rich Text Aware Typewriter Reveal]
**Learning:** In Unity TextMeshPro, iterating over string length for typewriter reveals can break when rich text tags (like <color> or <b>) are present, as the tag characters are revealed one-by-one. Using 'TMP_Text.ForceMeshUpdate()' followed by iterating over 'TMP_Text.textInfo.characterCount' ensures only rendered characters are revealed, maintaining both accessibility (screen readers) and visual polish.
**Action:** Always use 'textInfo.characterCount' and 'ForceMeshUpdate' for typewriter effects to ensure compatibility with rich text and accurate character-based pacing.

## 2026-03-24 - [Scaling Rhythmic Typewriter and Progression Cues]
**Learning:** Rhythmic punctuation pauses in typewriter effects are most effective when they occur *after* the punctuation character is revealed (checking index `i-1`) and use multipliers (e.g., 15x, 8x) instead of fixed delays. This ensures the cadence remains natural even when base typing speeds vary by character. Additionally, appending a visual completion cue (like '▽') provides essential feedback that a dialogue block is finished and the user can proceed.
**Action:** Always use speed multipliers for rhythmic pauses and include a visual completion character after typewriter reveals to improve readability and interaction clarity.

## 2026-03-25 - [Unified Dialogue Skip UX]
**Learning:** In narrative-heavy Unity cinematics, players appreciate a responsive skipping system that respects their reading speed. Implementing a `WaitForSecondsOrSkip` pattern—where a single input skips both the typewriter reveal and the subsequent cinematic pause—creates a much smoother experience. Key to this is managing the `skipRequested` flag lifecycle such that it only resets *after* the final pause in a dialogue beat.
**Action:** Use `WaitForSecondsOrSkip` for all cinematic pauses and defer `skipRequested` resets until the end of the wait block to support unified skipping.
## 2024-03-28 - [Unified Skip and Visual Feedback for Cinematics]
**Learning:** In dialogue-heavy cinematics, user frustration is often caused by unskippable "cinematic pauses" that occur after the text has finished revealing. By implementing a unified skip flag that isn't reset until *after* the post-dialogue delay, users can skip both the typewriter reveal and the subsequent pause with a single input. Additionally, adding subtle visual feedback like a 'Pop' scale animation on the speaker name when it changes helps anchor the user's attention and makes the UI transition feel more intentional and polished.
**Action:** Use a 'WaitForSecondsOrSkip' pattern for all cinematic delays and always provide a subtle visual cue (e.g., scale pop, color shift) when switching speakers to improve interaction clarity and "juice".
## 2026-03-25 - [Context-Aware Punctuation Pacing]
**Learning:** Standard 15x punctuation delays in typewriter effects can feel broken when applied to mid-word periods (e.g., "Sky.ix") or ellipsis dots, which require shorter, rhythmic pauses. Verifying if a character is part of a cluster (for ellipsis) or followed by a non-whitespace character (for names/technical terms) allows the system to maintain conversational flow without technical interruptions.
**Action:** Implement look-ahead and look-behind checks for periods to distinguish between sentence ends, ellipsis dots, and mid-word abbreviations to ensure fluid dialogue pacing.
## 2026-03-25 - [Context-Aware Punctuation Pauses and Themed Feedback]
**Learning:** Standard punctuation pauses (e.g., 15x delay) can feel disruptive when applied to ellipses (...) or mid-word periods in specialized names (e.g., 'Sky.ix'). Refining these pauses to 5x for ellipsis and 0x for mid-word periods maintains narrative flow. Furthermore, color-coding completion cues (▽) to match speaker themes (using rich text tags) reinforces character identity and provides a polished, cohesive UI experience.
**Action:** Use look-ahead logic to distinguish punctuation context (ellipsis vs. end-of-sentence vs. mid-word) and apply speaker-themed rich text to all interaction cues.
## 2026-03-25 - [Accessibility and Interaction in Cinematics]
**Learning:** When adding skippable waits (`WaitForSecondsOrSkip`) to Unity cinematics, the `skipRequested` flag should be managed carefully to avoid race conditions. Resetting it only at the *start* of a new dialogue block (rather than after each skip) allows a single user input to skip both the typing animation and the following cinematic pause. For accessibility, always ensure high contrast by adding black outlines to TMP text elements in scenes with dynamic or unpredictable backgrounds.
**Action:** Use a centralized skip flag lifecycle and mandatory TMP outlines (0.2 width) for improved cinematic accessibility and control.
