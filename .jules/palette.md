## 2025-05-14 - [Typewriter Punctuation Pacing]
**Learning:** Punctuation delays in dialogue reveal effects should trigger after the character is visible to maintain natural reading rhythm.
**Action:** Always increment visibility counters before checking for punctuation-based wait times.

## 2025-06-12 - [Visual Stability in Typewriter Effects]
**Learning:** Appending text to a TextMeshPro component while a typewriter effect is active can cause a "flash" where the entire message is briefly visible before being hidden by `maxVisibleCharacters`.
**Action:** Set `maxVisibleCharacters` to the current glyph count *before* appending new text and calling `ForceMeshUpdate()`.

## 2025-06-12 - [CLI Interaction Feedback]
**Learning:** In terminal-style interfaces, mirroring (echoing) user input back into the scrollback buffer provides vital confirmation that the system received the command, especially when processing takes time.
**Action:** Prepend a prompt symbol (e.g., `>`) and use a secondary color (e.g., gray) to distinguish user "echoes" from system responses.

## 2025-06-12 - [Terminal Navigation Shortcuts]
**Learning:** Adding standard terminal shortcuts like Up/Down arrow history and Tab completion significantly lowers the cognitive load for power users and makes the interface feel more "professional" and "responsive".
**Action:** Always include history buffers and basic completion for command-line style interfaces.
