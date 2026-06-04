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

        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);
        private static readonly string[] _availableCommands = { "help", "clear", "history", "infiniteration" };

        private Coroutine? _typewriterCoroutine;
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        private string _lastCommand = "";
        private readonly string[] _availableCommands = { "help", "clear" };

        // 🎨 Palette: Available commands for autocomplete
        private static readonly string[] ValidCommands = { "help", "clear" };
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;
        private string _lastCommand = "";
        private static readonly string[] _availableCommands = { "help", "clear", "history" };
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        private static readonly string[] _availableCommands = { "help", "clear", "history", "infiniteration" };

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
                ClearTerminal();
            }
        }

        private void OnEnable()
        {
            if (commandInput != null)
            {
                commandInput.ActivateInputField();
            }
        }

        private void ClearTerminal()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;
            WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
        }

        private void Update()
        {
            if (commandInput == null || !commandInput.isFocused) return;

            // 🎨 Palette: Unified Input Handling (History & Autocomplete)
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateHistory(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateHistory(-1);
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleAutocomplete();
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            // We move backwards in history from the end
            int newIndex = _historyIndex + direction;

            if (newIndex >= _commandHistory.Count)
            {
                newIndex = _commandHistory.Count - 1;
            }

            if (newIndex < -1)
            {
                newIndex = -1;
            }

            if (newIndex != _historyIndex)
            {
                _historyIndex = newIndex;
                if (_historyIndex == -1)
                {
                    commandInput.text = "";
                }
                else
                {
                    commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                }
                commandInput.MoveTextEnd(false);
            }
        }

        private void HandleAutocomplete()
            // 🎨 Palette: Command History (Up/Down Arrow) to recall previous inputs
            if (Input.GetKeyDown(KeyCode.UpArrow) && _commandHistory.Count > 0)
            // 🎨 Palette: History Navigation
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateHistory(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateHistory(-1);
            }
            // 🎨 Palette: Tab Completion for common commands
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentText = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentText))
                {
                    string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentText));
                    if (match != null)
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleTabCompletion();
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateHistory(-1);
            // 🎨 Palette: Command History Navigation
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_commandHistory.Count > 0 && _historyIndex < _commandHistory.Count - 1)
                {
                    _historyIndex++;
                    commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                commandInput.text = "";
            }
            else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.L))
            {
                ClearTerminal();
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            _historyIndex = Mathf.Clamp(_historyIndex + direction, -1, _commandHistory.Count - 1);

            if (_historyIndex == -1)
            {
                commandInput.text = "";
                if (_historyIndex > -1)
                {
                    _historyIndex--;
                    commandInput.text = _historyIndex == -1 ? "" : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                    commandInput.MoveTextEnd(false);
                }
            }
            // 🎨 Palette: Tab Completion
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                string currentInput = commandInput.text.Trim().ToLower();
                if (!string.IsNullOrEmpty(currentInput))
                {
                    string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
                    if (!string.IsNullOrEmpty(match))
                    {
                        commandInput.text = match;
                        commandInput.MoveTextEnd(false);
                    }
                }
            }
            // 🎨 Palette: Escape to Clear Current Line
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                commandInput.text = "";
            }
            // 🎨 Palette: Ctrl+L to Clear Terminal
            else if (Input.GetKeyDown(KeyCode.L) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                ClearTerminal();
            }
            else
            {
                commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.text = "";
                commandInput.ActivateInputField();
            }
            commandInput.MoveTextEnd(false);
        }

        private void HandleTabCompletion()
        {
            string currentInput = commandInput.text.Trim().ToLower();
            if (string.IsNullOrEmpty(currentInput)) return;

            string? match = ValidCommands.FirstOrDefault(c => c.StartsWith(currentInput));
            if (match != null)
            {
                commandInput.text = match;
                commandInput.MoveTextEnd(false);
            string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
            if (!string.IsNullOrEmpty(match))
            {
                commandInput.text = match;
                commandInput.MoveTextEnd(false);
                ClearTerminal();
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            // direction 1 is Up (older), -1 is Down (newer)
            int newIndex = _historyIndex + direction;

            if (newIndex >= 0 && newIndex < _commandHistory.Count)
            {
                _historyIndex = newIndex;
                commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }
            else if (newIndex == -1)
            {
                _historyIndex = -1;
                commandInput.text = "";
            _historyIndex = Mathf.Clamp(_historyIndex + direction, -1, _commandHistory.Count - 1);
            commandInput.text = (_historyIndex == -1) ? "" : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
            commandInput.MoveTextEnd(false);
        }

        private void ClearTerminal()
        {
            if (outputDisplay != null)
            {
                outputDisplay.text = "";
                outputDisplay.maxVisibleCharacters = 0;
                WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
            }
        }

        private void ClearTerminal()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;
            WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
        }

        public void ProcessCommand(string input)
        {
            // 🛡️ Sentinel: Early exit for empty or whitespace-only input.
            // 🛡️ Sentinel: Reset history index and cleanup input immediately for UX and flow.
            _historyIndex = -1;
            CleanupInputAfterCommand();

            // 🛡️ Sentinel: Early exit and basic echo for empty or whitespace-only input.
            _historyIndex = -1;

            if (string.IsNullOrWhiteSpace(input))
            {
                WriteToTerminal("\n<color=#888888>></color>");
                return;
            }

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection (e.g. Rich Text tags).
            if (input.Length > MaxInputLength)
            {
                string sanitizedInputPreview = input.Replace("<", "&lt;").Replace(">", "&gt;").Substring(0, 16);
                WriteToTerminal($"\n<color=#888888>> {sanitizedInputPreview}...</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length (256 characters).");
            // 🛡️ Sentinel: Sanitize input to escape rich text tags BEFORE any validation or echoing.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection BEFORE echoing to prevent UI injection.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInputAfterCommand();
                return;
            }

            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            if (!SafeCommandRegex.IsMatch(input))
            {
                string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters. Use only A-Z, 0-9, spaces, '.', '_', and '-'.");
                return;
            }

            // 🎨 Palette: Echo validated user command to terminal and update history.
            WriteToTerminal($"\n<color=#888888>> {input}</color>");

            // 🛡️ Sentinel: Echo validated user command to terminal.
            // Ensures validated input is echoed, preventing UI injection via Rich Text tags.
            WriteToTerminal($"\n<color=#888888>> {input}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInputAfterCommand();
                return;
            }

            // 🛡️ Sentinel: Echo sanitized input.
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");

            // 🎨 Palette: Echo validated user command to terminal.
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");

            // Update command history
            if (_commandHistory.Count == 0 || _commandHistory[_commandHistory.Count - 1] != input)
            // 🛡️ Sentinel: Echo sanitized input
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");

            if (_commandHistory.Count == 0 || _commandHistory.Last() != input)
            {
                _commandHistory.Add(input);
            }
            _historyIndex = -1;

            CleanupInputAfterCommand();

            _lastCommand = input;

            string[] parts = input.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();

            if (command == "clear") ClearTerminal();
            if (command == "clear")
            {
                ClearTerminal();
                return;
            }

            if (command == "history")
            {
                string historyOutput = "\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>";
                for (int i = 0; i < _commandHistory.Count; i++)
                {
                    historyOutput += $"\n {i}: <color=#00FFFF>{_commandHistory[i]}</color>";
                }
                WriteToTerminal(historyOutput);
                CleanupInputAfterCommand();
                return;
            }
            else if (command == "history")
            {
                string historyOutput = "\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>";
                for (int i = 0; i < _commandHistory.Count; i++)
                    historyOutput += $"\n {i + 1}: <color=#00FFFF>{_commandHistory[i]}</color>";
                WriteToTerminal(historyOutput);
                CleanupInputAfterCommand();
                return;
            }
            else if (command == "help")
            {
                WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                                "\n - <color=#00FFFF>help</color>: Show this message." +
                                "\n - <color=#00FFFF>clear</color>: Clear terminal." +
                                "\n - <color=#00FFFF>history</color>: Show command history." +
                                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
                                "\n - <color=#00FFFF>clear</color>: Clear the terminal display." +
                                "\n - <color=#00FFFF>history</color>: Show command history." +
                                "\n - <color=#00FFFF>[cmd] [arg1] [arg2]</color>: Execute extended system commands." +
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History</color>");
                                "\n\n<color=#888888>Shortcuts: [Tab] Complete, [Up/Down] History, [Ctrl+L] Clear, [Esc] Reset</color>");
                                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
                                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
                CleanupInputAfterCommand();
                return;
            }
            else if (command == "infiniteration")
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                    WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: Command '{parts[0]}' executed.");
                    return;
                }
            }

            // 🎨 Palette: "Did you mean?" fuzzy matching for unknown commands
            string? suggestion = GetClosestCommand(command);
            string errorMsg = $"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.</color>";
            if (suggestion != null)
            {
                errorMsg += $" <color=#FFFF00>Did you mean '{suggestion}'?</color>";
            }
            errorMsg += " Type <color=#00FFFF>'help'</color> for options.";

            WriteToTerminal(errorMsg);
            if (commandInput != null) StartCoroutine(ShakeInputField());
        }

        private string? GetClosestCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            string? bestMatch = null;
            int minDistance = 3; // Max threshold for "closeness"

            foreach (string cmd in ValidCommands)
            {
                int distance = ComputeLevenshteinDistance(input, cmd);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestMatch = cmd;
                }
            }

            return bestMatch;
        }

        private int ComputeLevenshteinDistance(string s, string t)
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
                    d[i, j] = Mathf.Min(
                        Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
                ExecuteInfiniteration();
            }
            else if (parts.Length >= 3)
            {
                ExecuteExtendedCommand(parts[0], input.Substring(input.IndexOf(parts[1])));
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: Command '{parts[0]}' executed.");
            }
            else if (command == "infiniteration") ExecuteInfiniteration();
            else
            {
                string suggestion = GetFuzzyMatch(command);
                // 🎨 Palette: Fuzzy Matching for unknown commands.
                string suggestion = GetFuzzyMatch(command);
                string suggestion = GetFuzzyMatch(parts[0]);
                string suggestionText = !string.IsNullOrEmpty(suggestion) ? $" Did you mean <color=#00FFFF>'{suggestion}'</color>?" : "";
                WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText} Type <color=#00FFFF>'help'</color> for options.</color>");
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
            int minDistance = 3;

            foreach (string cmd in _availableCommands)
            {
                int dist = GetLevenshteinDistance(input.ToLower(), cmd);
                if (dist < minDistance) { minDistance = dist; bestMatch = cmd; }
            }
            return bestMatch;
        }

        private int GetLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            int[,] d = new int[n + 1, m + 1];
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0) return m; if (m == 0) return n;
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int j = 0; j <= m; d[0, j] = j++) ;
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Mathf.Min(Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            return d[n, m];
        }

        private void CleanupInputAfterCommand() { if (commandInput != null) { commandInput.text = ""; commandInput.ActivateInputField(); } }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.maxVisibleCharacters = int.MaxValue;
            }
            if (_typewriterCoroutine != null) { StopCoroutine(_typewriterCoroutine); outputDisplay.maxVisibleCharacters = int.MaxValue; }
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            outputDisplay.ForceMeshUpdate();
            int startCount = outputDisplay.textInfo.characterCount;
            outputDisplay.maxVisibleCharacters = startCount;
            int startVisibleCount = outputDisplay.textInfo.characterCount;
            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;

            for (int i = 1; i <= endVisibleCount - startVisibleCount; i++)
            {
                outputDisplay.maxVisibleCharacters = startVisibleCount + i;

                char c = outputDisplay.textInfo.characterInfo[startVisibleCount + i - 1].character;
                float delay = typingSpeed;

                if (c == '.' || c == '!' || c == '?') delay += punctuationDelay;
                else if (c == ',' || c == ':' || c == ';') delay += commaDelay;

                yield return GetWait(delay);

            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endCount = outputDisplay.textInfo.characterCount;
            for (int i = 1; i <= endCount - startCount; i++)
            {
                outputDisplay.maxVisibleCharacters = startCount + i;
                char c = outputDisplay.textInfo.characterInfo[startCount + i - 1].character;
                float totalDelay = typingSpeed;
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEnd = true;
                    if (startCount + i < endCount)
                        if (!char.IsWhiteSpace(outputDisplay.textInfo.characterInfo[startCount + i].character)) isEnd = false;
                    if (isEnd) totalDelay += (c == '.' && startCount + i - 2 >= 0 && outputDisplay.textInfo.characterInfo[startCount + i - 2].character == '.') ? typingSpeed * 3f : punctuationDelay;
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

                // ⚡ Bolt: Zero-allocation yield via shared cache
                yield return GetWait(delay);
                    totalDelay += commaDelay;
                }
                else if (c == ',' || c == ':' || c == ';') totalDelay += commaDelay;

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
            while (elapsed < 0.2f)
            float elapsed = 0f, duration = 0.2f, magnitude = 5f;
            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-5f, 5f);
                commandInput.transform.localPosition = originalPos + new Vector3(x, 0, 0);
                elapsed += Time.deltaTime; yield return null;
            }
            commandInput.transform.localPosition = originalPos;
        }

        private void ExecuteExtendedCommand(string cmd, string args) { }
    }
}
