## 2025-05-14 - [Typewriter Punctuation Pacing]
**Learning:** Punctuation delays in dialogue reveal effects should trigger after the character is visible to maintain natural reading rhythm.
**Action:** Always increment visibility counters before checking for punctuation-based wait times.

## 2025-06-12 - [Visual Stability in Typewriter Effects]
**Learning:** Appending text to a TextMeshPro component while a typewriter effect is active can cause a "flash" where the entire message is briefly visible before being hidden by `maxVisibleCharacters`.
**Action:** Set `maxVisibleCharacters` to the current glyph count *before* appending new text and calling `ForceMeshUpdate()`.

## 2025-06-12 - [CLI Interaction Feedback]
**Learning:** In terminal-style interfaces, mirroring (echoing) user input back into the scrollback buffer provides vital confirmation that the system received the command, especially when processing takes time.
**Action:** Prepend a prompt symbol (e.g., `>`) and use a secondary color (e.g., gray) to distinguish user "echoes" from system responses.

## 2025-06-12 - [Terminal Discoverability]
**Learning:** Terminal-based interfaces suffer from low discoverability of power-user features like command history and tab completion. Placing subtle hints in the `help` command's output increases awareness without needing a persistent UI overlay.
**Action:** Include a muted "Tip" line in system help responses to educate users on keyboard shortcuts.
## 2025-06-12 - [Terminal Navigation Shortcuts]
**Learning:** Adding standard terminal shortcuts like Up/Down arrow history and Tab completion significantly lowers the cognitive load for power users and makes the interface feel more "professional" and "responsive".
**Action:** Always include history buffers and basic completion for command-line style interfaces.
## 2025-06-15 - [Terminal Shortcut Discoverability]
**Learning:** Keyboard shortcuts in CLI interfaces (like Up/Down for history or Tab for completion) are often invisible to new users unless explicitly hinted.
**Action:** Include a muted gray (#888888) tip in the 'help' command output detailing available keyboard shortcuts.

## 2025-06-18 - [Terminal Fuzzy Matching]
**Learning:** Terminal-based interfaces are prone to user typos, especially when commands are technical. Providing "Did you mean?" suggestions using Levenshtein distance significantly reduces user frustration and improves the "intelligence" feel of the system.
**Action:** Implement fuzzy matching suggestions for unknown commands in CLI-style interfaces to guide users toward valid inputs.

## 2025-06-21 - [Terminal Power-User Shortcuts]
**Learning:** Power-user shortcuts (like Ctrl+L for clear or Esc for clear-line) significantly enhance the "terminal-native" feel for experienced users, but remain entirely undiscoverable unless explicitly documented in the 'help' command.
**Action:** Always pair new keyboard shortcuts with updated help documentation and muted shortcut hints.

## 2025-06-25 - [Contextual UX Education]
**Learning:** Users are most likely to seek help for a feature (like history) when they try to use it and fail. Providing contextual tips (e.g., in empty states) is more effective than static documentation for teaching power-user shortcuts.
**Action:** Always include helpful, contextual hints in "empty states" to guide users toward relevant shortcuts or features.

## 2025-06-28 - [Following Cursor UX]
**Learning:** For a retro terminal typewriter effect, having the cursor "follow" the text reveal (by appending it to the end of the substring being revealed) creates a much more immersive experience than having it jump only after the full message is revealed.
**Action:** In typewriter coroutines, update the text string per step to include the cursor, then transition to an idle blinking state using zero-allocation maxVisibleCharacters toggling.
## 2025-11-23 - [Terminal Retro Cursor Stability]
**Learning:** Implementing a blinking retro cursor ('█') using `maxVisibleCharacters` requires defensive checks to prevent negative values when the output display is empty, which can cause TextMeshPro to show all characters instead of hiding the cursor.
**Action:** Use `Mathf.Max(0, totalChars - 1)` when toggling the cursor off to ensure the terminal remains visually stable even when cleared.

## 2025-11-23 - [Repository Pollution via Build Artifacts]
**Learning:** Committing build outputs (bin/, obj/, .dll, .pdb) into the repository is a major violation of repository hygiene that can lead to PR rejection.
**Action:** Always verify that the repository is clean of build-generated artifacts using `rm -rf bin/ obj/ ./*.dll ./*.pdb` before submitting.
## 2026-06-11 - [Retro Terminal Cursor Feedback]
**Learning:** A blinking block cursor ('█') provides essential visual feedback in terminal interfaces, signaling that the system is active during typewriter effects and ready for input when idle.
**Action:** Implement 'following' cursors in CLI-style reveals and use zero-allocation visibility toggles (maxVisibleCharacters) for idle blinking.
## 2025-06-28 - [Retro Terminal Immersion]
**Learning:** Retro terminal immersion is significantly enhanced by providing simulated system metadata (like version strings and "Last Login" timestamps) during the startup sequence. This makes the interface feel like a functional part of the game's universe rather than just a UI layer.
**Action:** Include simulated session info and system versions in CLI-style startup sequences to deepen environmental storytelling and player immersion.
## 2025-06-28 - [Terminal Blinking Cursor Integration]
**Learning:** When implementing a blinking cursor in a typewriter-reveal UI (TextMeshPro), appending the cursor ('█') to the text buffer and managing its visibility by toggling `maxVisibleCharacters` between `N` and `N+1` (where N is the current revealed character count) provides a stable, flicker-free effect that respects Rich Text tags.
**Action:** Coordinate cursor visibility coroutines with typewriter reveal logic to ensure the cursor always trails the most recently revealed character.

## 2026-06-17 - [Accessibility Contrast & Reactive Completion]
**Learning:** To meet WCAG AA standards on dark backgrounds, secondary/muted text color should be at least #AAAAAA (up from #888888) to ensure a contrast ratio >= 4.5:1. Furthermore, providing a "reactive" tab completion that recalls the last fuzzy suggestion on an empty input line significantly improves error recovery flow.
**Action:** Always use #AAAAAA for muted text and implement suggestion-aware tab completion in CLI interfaces.

## 2026-11-23 - [Typewriter Reveal Skipping]
**Learning:** Providing a way to skip typewriter reveal animations is essential for power-user efficiency. When implementation involves stopping an active coroutine, the system must explicitly force the final state (all characters visible) to prevent visual "stutter" or incomplete messages being left in the buffer.
**Action:** Always pair typewriter skip flags with an explicit 'reveal-all' state update when interrupting active coroutines.
