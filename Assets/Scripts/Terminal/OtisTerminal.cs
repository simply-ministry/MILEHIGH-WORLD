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
        [SerializeField] private TMP_InputField commandInput = null!;
        [SerializeField] private TextMeshProUGUI outputDisplay = null!;

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

        private static readonly string[] ValidCommands = { "help", "clear" };
        private string _lastCommand = "";
        private readonly string[] _availableCommands = { "help", "clear", "infiniteration" };
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        private string _lastCommand = "";
        private static readonly string[] _availableCommands = { "help", "clear" };

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
                commandInput.onSubmit.AddListener(ProcessCommand);
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
                if (_commandHistory.Count > 0)
                {
                    string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentText));
                    if (match != null && match != currentText)
                    {
                        commandInput.text = match;
                        commandInput.MoveTextEnd(false);
                    }
                }
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            // direction -1 is Up (older), +1 is Down (newer)
            int newIndex = _historyIndex == -1 ? _commandHistory.Count - 1 : _historyIndex + direction;

            if (newIndex >= 0 && newIndex < _commandHistory.Count)
            {
                _historyIndex = newIndex;
                commandInput.text = _commandHistory[_historyIndex];
                commandInput.MoveTextEnd(false);
            }
            else if (newIndex >= _commandHistory.Count)
            {
                _historyIndex = -1;
                commandInput.text = "";
                if (_commandHistory.Count > 0)
            // 🎨 Palette: Command History Navigation
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_commandHistory.Count > 0 && _historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex = Mathf.Clamp(_historyIndex + 1, 0, _commandHistory.Count - 1);
                    commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_historyIndex > -1)
                _historyIndex = Mathf.Clamp(_historyIndex - 1, -1, _commandHistory.Count - 1);
                commandInput.text = _historyIndex == -1 ? "" : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }
            // 🎨 Palette: Tab Completion for common commands
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    commandInput.text = _historyIndex == -1 ? "" : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
            }
            // 🎨 Palette: Tab Completion for common commands
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentText = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentText))
                {
                    string? match = ValidCommands.FirstOrDefault(c => c.StartsWith(currentText));
                    if (match != null)
                else if (_historyIndex == 0)
                {
                    _historyIndex = -1;
                    commandInput.text = "";
                }
            }
            // 🎨 Palette: Tab Completion
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentInput = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentInput))
                {
                    string match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
                    if (!string.IsNullOrEmpty(match))
                    {
                        commandInput.text = match;
                        commandInput.MoveTextEnd(false);
                    }
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

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
            // Sanitize input to escape rich text tags.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            if (input.Length > MaxInputLength)
            {
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput.Substring(0, 16)}...</color>");
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInputAfterCommand();
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters.");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInputAfterCommand();
                return;
            }

            // 🎨 Palette: Echo validated user command to terminal.
            // 🛡️ Sentinel: Ensures sanitized input is echoed, preventing UI injection via Rich Text tags.
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");

            // Update command history
            // 🎨 Palette: Echo validated user command to terminal and update history.
            WriteToTerminal($"\n<color=#888888>> {input}</color>");

            if (_commandHistory.Count == 0 || _commandHistory[_commandHistory.Count - 1] != input)
            {
                _commandHistory.Add(input);
            }
            _historyIndex = -1;

            CleanupInputAfterCommand();

            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();

            if (command == "clear")
            {
                outputDisplay.text = "";
                outputDisplay.maxVisibleCharacters = 0;
                WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
                CleanupInputAfterCommand();
                return;
            }

            if (command == "help")
            {
                WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                                "\n - <color=#00FFFF>help</color>: Show this message." +
                                "\n - <color=#00FFFF>clear</color>: Clear terminal." +
                                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History</color>");
                CleanupInputAfterCommand();
                                "\n - <color=#00FFFF>clear</color>: Clear the terminal display." +
                                "\n - <color=#00FFFF>[cmd] [arg1] [arg2]</color>: Execute extended system commands." +
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History</color>");
                return;
            }

            if (command == "infiniteration")
            {
                ExecuteInfiniteration();
                CleanupInputAfterCommand();
                return;
            }

            // Extended commands
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
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{parts[0]}'.</color>");
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{parts[0]}'. Type <color=#00FFFF>'help'</color> for options.</color>");
                StartCoroutine(ShakeInputField());
                string suggestion = GetFuzzyMatch(parts[0]);
                string suggestionText = !string.IsNullOrEmpty(suggestion) ? $" Did you mean <color=#00FFFF>'{suggestion}'</color>?" : "";
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{parts[0]}'.{suggestionText} Type <color=#00FFFF>'help'</color> for options.</color>");
                if (commandInput != null) StartCoroutine(ShakeInputField());
            }

            CleanupInputAfterCommand();
        }

        private void ExecuteInfiniteration()
        {
            string sequence = "108-99-90-81-72-63-54-45-36-27-18-09-108";
            WriteToTerminal($"\n<color=#00FF00>[ENGINE]</color>: Initializing Infiniteration Engine..." +
                            $"\n<color=#FFFF00>Sequence:</color> {sequence}" +
                            $"\n<color=#FFFF00>Digital Root:</color> 9" +
                            $"\n<color=#00FFFF>[STATUS]</color>: Loop Closed. 12-11-10...01-012");
        }

        private string GetFuzzyMatch(string input)
        {
            string bestMatch = "";
            int minDistance = 3; // Max distance for a suggestion

            foreach (string cmd in _availableCommands)
            {
                int dist = GetLevenshteinDistance(input.ToLower(), cmd);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestMatch = cmd;
                }
            }
            return bestMatch;
        }

        private int GetLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Mathf.Min(Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
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
                    delay += commaDelay;
                }

                    totalDelay += commaDelay;
                }

                // ⚡ Bolt: Single zero-allocation yield per character reveal via shared cache.
                yield return GetWait(totalDelay);
                    delay += commaDelay;
                }

                // ⚡ Bolt: Zero-allocation yield via shared cache
                yield return GetWait(delay);
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
