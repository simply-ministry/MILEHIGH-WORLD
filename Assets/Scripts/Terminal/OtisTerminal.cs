using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

        [Header("Typewriter Settings")]
        [SerializeField] private float typingSpeed = 0.02f;
        [SerializeField] private float punctuationDelay = 0.15f;
        [SerializeField] private float commaDelay = 0.08f;

        // 🛡️ Sentinel: Security constants for input validation to prevent DoS and malformed input processing.
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);

        private Coroutine? _typewriterCoroutine;
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        // ⚡ Bolt: Shared cache for WaitForSeconds to eliminate GC allocations during typewriter effects.
        // Using int millisecond keys to avoid floating-point precision issues in dictionary lookups.
        private static readonly Dictionary<int, WaitForSeconds> _waitCache = new Dictionary<int, WaitForSeconds>();

        private static WaitForSeconds GetWait(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(ms, out WaitForSeconds wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[ms] = wait;
            }
            return wait;
        }

        private void Start()
        {
            if (commandInput != null)
            {
                commandInput.characterLimit = MaxInputLength;
                if (commandInput.placeholder is TMP_Text placeholderText)
                {
                    placeholderText.text = "Enter command...";
                }
            }

            if (outputDisplay != null)
            {
                outputDisplay.text = "";
                WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
            }
        }

        private void OnEnable()
        {
            if (commandInput != null)
            {
                commandInput.ActivateInputField();
            }
        }

        private void Update()
        {
            if (commandInput == null || !commandInput.isFocused) return;

            // 🎨 Palette: Command History (Up/Down Arrow) to recall previous inputs
            if (Input.GetKeyDown(KeyCode.UpArrow) && _commandHistory.Count > 0)
            {
                _historyIndex = Mathf.Clamp(_historyIndex + 1, 0, _commandHistory.Count - 1);
                commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _historyIndex = Mathf.Clamp(_historyIndex - 1, -1, _commandHistory.Count - 1);
                commandInput.text = _historyIndex == -1 ? "" : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }

            // 🎨 Palette: Tab Completion for common commands
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentText = commandInput.text.ToLower();
                if (currentText.StartsWith("h") && !currentText.Equals("help"))
                {
                    commandInput.text = "help";
                    commandInput.MoveTextEnd(false);
                }
                else if (currentText.StartsWith("c") && !currentText.Equals("clear"))
                {
                    commandInput.text = "clear";
                    commandInput.MoveTextEnd(false);
                }
            }
        }

        public void ProcessCommand(string input)
        {
            // 🎨 Palette: Reset history index on command submission
            _historyIndex = -1;

            // 🛡️ Sentinel: Early exit and basic echo for empty input.
            if (string.IsNullOrWhiteSpace(input))
            {
                WriteToTerminal("\n<color=#888888>></color>");
                CleanupInputAfterCommand();
                return;
            }

            // UX Enhancement: Clear input and refocus immediately for better flow
            CleanupInputAfterCommand();

            // 🛡️ Sentinel: Security - Strip Rich Text tags before echoing to prevent UI injection if validation fails.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection.
            // 🛡️ Sentinel: Input validation and DoS protection
            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection (e.g. Rich Text tags).
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput.Substring(0, 16)}...</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length (256 characters).");
                CleanupInputAfterCommand();
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters. Use only A-Z, 0-9, spaces, '.', '_', and '-'.");
                CleanupInputAfterCommand();
                return;
            }

            // 🎨 Palette: Echo user command to terminal.
            // 🎨 Palette: Echo user command to terminal AFTER validation to ensure safe rendering.
            // 🛡️ Sentinel: Ensures validated input is echoed, preventing UI injection via Rich Text tags.
            // 🎨 Palette: Echo validated user command to terminal and update history.
            WriteToTerminal($"\n<color=#888888>> {input}</color>");
            _commandHistory.Add(input);

            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();

            if (command == "clear")
            {
                outputDisplay.text = "";
                outputDisplay.maxVisibleCharacters = 0;
                WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
                return;
            }

            if (command == "help")
            {
                WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                                "\n - <color=#00FFFF>help</color>: Show this message." +
                                "\n - <color=#00FFFF>clear</color>: Clear the terminal display." +
                                "\n - <color=#00FFFF>[cmd] [arg1] [arg2]</color>: Execute extended system commands." +
                                "\n<color=#888888>Tip: Use Tab for completion, Up/Down for history.</color>");
                return;
            }

            if (parts.Length >= 3)
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                    WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: Command '{parts[0]}' executed.");
                }
            }
            else
            {
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{parts[0]}'. Type <color=#00FFFF>'help'</color> for options.</color>");
                if (commandInput != null) StartCoroutine(ShakeInputField());
            }
        }

        private void CleanupInputAfterCommand()
        {
            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }
        }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                // UX Enhancement: Reveal full previous message when interrupted.
                outputDisplay.maxVisibleCharacters = int.MaxValue;
            }

            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            outputDisplay.maxVisibleCharacters = startVisibleCount;
            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;

            int charactersToReveal = endVisibleCount - startVisibleCount;

            for (int i = 1; i <= charactersToReveal; i++)
            {
                outputDisplay.maxVisibleCharacters = startVisibleCount + i;

                // 🎨 Palette: Rhythmic punctuation pauses for an "analog" terminal feel.
                // We check the revealed character to pause after it appears.
                // ⚡ Bolt: Consolidated rhythmic delay calculation to eliminate redundant resumptions.
                // ⚡ Bolt: Calculate total delay for this character once to minimize coroutine resumptions.
                char c = outputDisplay.textInfo.characterInfo[startVisibleCount + i - 1].character;
                float totalDelay = typingSpeed;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (startVisibleCount + i < endVisibleCount)
                    {
                        char nextChar = outputDisplay.textInfo.characterInfo[startVisibleCount + i].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence)
                    {
                        bool isEllipsis = (c == '.' && startVisibleCount + i - 2 >= 0 && outputDisplay.textInfo.characterInfo[startVisibleCount + i - 2].character == '.');
                        totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                    }
                }
                else if (c == ',' || c == ':' || c == ';')
                {
                    totalDelay += commaDelay;
                }

                // ⚡ Bolt: Single zero-allocation yield per character reveal.
                yield return GetWait(totalDelay);
            }

            _typewriterCoroutine = null;
        }

        private IEnumerator ShakeInputField()
        {
            if (commandInput == null) yield break;
            Vector3 originalPos = commandInput.transform.localPosition;
            float elapsed = 0f;
            float duration = 0.2f;
            float magnitude = 5f;

            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                commandInput.transform.localPosition = originalPos + new Vector3(x, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            commandInput.transform.localPosition = originalPos;
        }

        private void ExecuteExtendedCommand(string cmd, string args)
        {
            // Implementation for specific terminal logic
        }
    }
}
