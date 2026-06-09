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

## 2025-11-23 - [Terminal Retro Cursor Stability]
**Learning:** Implementing a blinking retro cursor ('█') using `maxVisibleCharacters` requires defensive checks to prevent negative values when the output display is empty, which can cause TextMeshPro to show all characters instead of hiding the cursor.
**Action:** Use `Mathf.Max(0, totalChars - 1)` when toggling the cursor off to ensure the terminal remains visually stable even when cleared.

## 2025-11-23 - [Repository Pollution via Build Artifacts]
**Learning:** Committing build outputs (bin/, obj/, .dll, .pdb) into the repository is a major violation of repository hygiene that can lead to PR rejection.
**Action:** Always verify that the repository is clean of build-generated artifacts using `rm -rf bin/ obj/ ./*.dll ./*.pdb` before submitting.
