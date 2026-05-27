using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;

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

        // 🎨 Palette: Multi-command History
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        // ⚡ Bolt: Cache for WaitForSeconds to eliminate GC allocations during coroutine execution.
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        private string _lastCommand = "";
        private readonly string[] _availableCommands = { "help", "clear" };

        // 🎨 Palette: Available commands for autocomplete
        private static readonly string[] ValidCommands = { "help", "clear" };

        // ⚡ Bolt: Shared cache for WaitForSeconds to eliminate GC allocations during typewriter effects.
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

            // 🎨 Palette: Command History Navigation (Up/Down Arrows)
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateHistory(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateHistory(1);
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            int newIndex = _historyIndex + direction;

            // Clamp and handle boundary cases
            if (newIndex >= _commandHistory.Count)
            {
                _historyIndex = _commandHistory.Count;
                commandInput.text = "";
                return;
            // 🎨 Palette: Command History navigation
            // 🎨 Palette: Command History (Up Arrow) to recall previous input
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_commandHistory.Count > 0 && _historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
            }
            // 🎨 Palette: Clear input (Down Arrow) for quick reset
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                commandInput.text = "";
            }
            // 🎨 Palette: Tab Completion for common commands
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentText = commandInput.text.ToLower();
                if ("help".StartsWith(currentText) && currentText != "help") commandInput.text = "help";
                else if ("clear".StartsWith(currentText) && currentText != "clear") commandInput.text = "clear";
                commandInput.MoveTextEnd(false);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
                else if (_historyIndex == 0)
                {
                    _historyIndex = -1;
                    commandInput.text = "";
                }
            }
            // 🎨 Palette: Tab completion for 'help' and 'clear'
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentInput = commandInput.text.ToLower();
                if (string.IsNullOrEmpty(currentInput)) return;

            if (newIndex < 0)
            {
                newIndex = 0;
            }
        }

        public void ProcessCommand(string input)
        {
            if (newIndex != _historyIndex)
            {
                _historyIndex = newIndex;
                commandInput.text = _commandHistory[_historyIndex];
                commandInput.MoveTextEnd(false);
                if ("help".StartsWith(currentInput))
                {
                    commandInput.text = "help";
                    commandInput.MoveTextEnd(false);
                }
                else if ("clear".StartsWith(currentInput))
                {
                    commandInput.text = "clear";
                    commandInput.MoveTextEnd(false);
            // 🎨 Palette: Tab Completion for discoverable commands
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentText = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentText))
                {
                    foreach (string cmd in ValidCommands)
                    {
                        if (cmd.StartsWith(currentText))
                        {
                            commandInput.text = cmd;
                            commandInput.MoveTextEnd(false);
                            break;
                        }

            // 🎨 Palette: Tab Completion
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentInput = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentInput))
                {
                    string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
                    if (match != null)
                    {
                        commandInput.text = match;
                        commandInput.MoveTextEnd(false);
                    }
                }
            }
        }

        public void ProcessCommand(string input)
        {
            // 🛡️ Sentinel: Early exit and basic echo for empty input.
            if (string.IsNullOrWhiteSpace(input))
            {
                WriteToTerminal("\n<color=#888888>></color>");
                CleanupInputAfterCommand();
                return;
            }

            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection (e.g. Rich Text tags).
            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
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

            WriteToTerminal($"\n<color=#888888>> {input}</color>");
            // 🎨 Palette: Echo validated user command to terminal.
            WriteToTerminal($"\n<color=#888888>> {input}</color>");

            // 🎨 Palette: Add to history if unique from the last entry
            if (_commandHistory.Count == 0 || _commandHistory[_commandHistory.Count - 1] != input)
            {
                _commandHistory.Add(input);
            }
            _historyIndex = _commandHistory.Count;

            // 🎨 Palette: Update command history
            _commandHistory.Add(input);
            _historyIndex = -1;

            _lastCommand = input;
            CleanupInputAfterCommand();

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
                                "\n <color=#888888>Tip: Use [Tab] to autocomplete, [Up] for history, [Down] to clear.</color>");
                                "\n <color=#888888>Tip: Use Up/Down arrows to navigate command history.</color>");
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History</color>");
                                "\n <color=#888888>(Tip: Use Tab for autocomplete and Up Arrow for history)</color>");
                                "\n<color=#888888><i>(Tip: Use Tab for auto-completion and Up Arrow for history)</i></color>");
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

                // ⚡ Bolt: Single zero-allocation yield per character reveal via shared cache.
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
