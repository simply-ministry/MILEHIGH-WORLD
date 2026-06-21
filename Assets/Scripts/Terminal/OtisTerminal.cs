using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput = null!;
        [SerializeField] private TextMeshProUGUI outputDisplay = null!;

        [Header("Typewriter Settings")]
        [SerializeField] private float typingSpeed = 0.02f;
        [SerializeField] private float commaDelay = 0.08f;
        [SerializeField] private float punctuationDelay = 0.15f;
        [SerializeField] private float commaDelay = 0.08f;

        [Header("Cursor Settings")]
        [SerializeField] private float blinkRate = 0.5f;
        private const char CursorChar = '█';
        private float _nextBlinkTime;
        private bool _cursorVisible = true;

        private const int MaxInputLength = 256;
        // 🛡️ Sentinel: Use explicit whitespace classes [ \t] instead of \s to prevent newline injection/terminal spoofing.
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9 \t._\-]+$", RegexOptions.Compiled);
        private static readonly string[] _availableCommands = { "help", "clear", "history", "infiniteration" };

        private Coroutine? _typewriterCoroutine;
        private Coroutine? _cursorCoroutine;
        private bool _cursorVisible = true;
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;
        private string _persistentInput = "";
        private string _lastSuggestion = "";

        // 🎨 Palette: Blinking cursor state
        private const char CursorChar = '█';
        private float _blinkTimer;
        private bool _cursorVisible = true;

        // ⚡ Bolt: Shared cache for WaitForSeconds to eliminate GC allocations during typewriter effects.
        private static readonly Dictionary<int, WaitForSeconds> _waitCache = new Dictionary<int, WaitForSeconds>();

        private static WaitForSeconds GetWait(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(ms, out WaitForSeconds? wait) || wait == null)
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
                    placeholderText.text = "Enter command (type 'help' for info)...";
                commandInput.onSubmit.AddListener(ProcessCommand);
            }
            ClearTerminal();
        }

        private void OnEnable()
        {
            commandInput?.ActivateInputField();
            // ⚡ Bolt: Fix coroutine leak - only start if not already running.
            if (_cursorCoroutine == null)
            {
                _cursorCoroutine = StartCoroutine(HandleBlinkingCursor());
            }
            if (_cursorCoroutine == null) _cursorCoroutine = StartCoroutine(HandleBlinkingCursor());
        }

        private void OnDisable()
        {
            if (_cursorCoroutine != null)
            {
                StopCoroutine(_cursorCoroutine);
                _cursorCoroutine = null;
            }
        }

        private void ClearTerminal()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;
            string lastLogin = System.DateTime.Now.ToString("ddd MMM dd HH:mm:ss");
            WriteToTerminal($"Last login: {lastLogin} on ttys000" +
                "\n<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");

            // 🎨 Palette: Time-aware personalized greeting for a touch of delight.
            string greeting = DateTime.Now.Hour switch
            {
                >= 5 and < 12 => "Good morning",
                >= 12 and < 17 => "Good afternoon",
                >= 17 and < 21 => "Good evening",
                _ => "Welcome back"
            };

            // 🎨 Palette: Enhanced retro terminal startup sequence with simulated session info.
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>");
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\n{greeting}, Operator. Type <color=#00FFFF>'help'</color> for available commands.");
            // 🎨 Palette: Enhanced retro terminal startup sequence with simulated session info.
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            WriteToTerminal($"<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\nLast login: {timestamp} on ttys001" +
                            $"\nWelcome to the Terminal. Type <color=#00FFFF>'help'</color> for available commands.");
        }

            // 🎨 Palette: Enhanced retro terminal startup sequence with simulated session info and time-aware greeting.
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            int hour = DateTime.Now.Hour;
            string greeting = hour switch
            {
                >= 5 and < 12 => "Good morning",
                >= 12 and < 17 => "Good afternoon",
                >= 17 and < 21 => "Good evening",
                _ => "Welcome back"
            };

            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>");
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\n{greeting}, Operator. Type <color=#00FFFF>'help'</color> for available commands.");
            // 🎨 Palette: Enhanced retro terminal startup sequence with simulated session info.
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>");
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type <color=#00FFFF>'help'</color> for commands.");
            string lastLogin = DateTime.Now.ToString("ddd MMM dd HH:mm:ss");
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");

            WriteToTerminal($"<color=#888888>Last login: {lastLogin} on ttys000</color>");
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\nLast login: {timestamp} on ttys001" +
                            $"\nWelcome to the Terminal. Type <color=#00FFFF>'help'</color> for available commands.");
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            WriteToTerminal($"<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\n<color=#AAAAAA>Last login: {timestamp} on ttys001</color>" +
                            $"\nWelcome to the Terminal. Type <color=#00FFFF>'help'</color> for available commands.");
            int hour = DateTime.Now.Hour;
            string greeting = (hour >= 5 && hour < 12) ? "Good morning, Operator." :
                             (hour >= 12 && hour < 17) ? "Good afternoon, Operator." :
                             (hour >= 17 && hour < 21) ? "Good evening, Operator." :
                             "System active. Eyes on the void, Operator.";

            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss");
            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>\n" +
                            $"<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. {greeting}\n" +
                            "Type <color=#00FFFF>'help'</color> for available commands.");
        }

        private IEnumerator HandleBlinkingCursor()
        {
            while (true)
            {
                _cursorVisible = !_cursorVisible;
                UpdateCursorVisibility();
                yield return GetWait(0.53f); // Standard terminal blink rate
                UpdateCursorDisplay();
                yield return GetWait(0.53f);
            }
        }

        private void UpdateCursorVisibility()
        {
            if (outputDisplay == null) return;

            int totalChars = outputDisplay.textInfo.characterCount;
            if (totalChars == 0 || !outputDisplay.text.EndsWith("█")) return;

            if (_typewriterCoroutine != null) return; // Managed by typewriter during reveal

            outputDisplay.maxVisibleCharacters = _cursorVisible ? totalChars : Mathf.Max(0, totalChars - 1);
        }

        private void Update()
        {
            HandleBlinkingCursor();

            if (commandInput == null || !commandInput.isFocused) return;

            if (Input.GetKeyDown(KeyCode.UpArrow)) NavigateHistory(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) NavigateHistory(-1);
            else if (Input.GetKeyDown(KeyCode.Tab)) HandleTabCompletion();
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                commandInput.text = "";
                _persistentInput = "";
                _historyIndex = -1;
                commandInput.ActivateInputField();
            }
            else if (Input.GetKeyDown(KeyCode.L) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                ClearTerminal();
            }
        }

        private void HandleBlinkingCursor()
        {
            if (outputDisplay == null || _typewriterCoroutine != null) return;

            _blinkTimer += Time.deltaTime;
            if (_blinkTimer >= 0.5f)
            {
                _blinkTimer = 0;
                _cursorVisible = !_cursorVisible;
                int charCount = outputDisplay.textInfo.characterCount;
                outputDisplay.maxVisibleCharacters = _cursorVisible ? charCount : charCount - 1;
            }
        }

        private void HandleBlinkingCursor()
        {
            if (outputDisplay == null || _typewriterCoroutine != null) return;

            if (Time.unscaledTime >= _nextBlinkTime)
            {
                _cursorVisible = !_cursorVisible;
                _nextBlinkTime = Time.unscaledTime + blinkRate;

                int totalChars = outputDisplay.textInfo.characterCount;
                outputDisplay.maxVisibleCharacters = _cursorVisible ? totalChars : Mathf.Max(0, totalChars - 1);
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            // Save current input if we are starting to navigate away from the prompt
            if (_historyIndex == -1)
            {
                _persistentInput = commandInput.text;
            }

            _historyIndex = Mathf.Clamp(_historyIndex + direction, -1, _commandHistory.Count - 1);

            if (_historyIndex == -1)
            {
                commandInput.text = _persistentInput;
            }
            else
            {
                commandInput.text = _commandHistory[_commandHistory.Count - 1 - _historyIndex];
            int newIndex = Mathf.Clamp(_historyIndex + direction, -1, _commandHistory.Count - 1);
            if (newIndex != _historyIndex)
            {
                _historyIndex = newIndex;
                commandInput.text = (_historyIndex == -1) ? _persistentInput : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }

            commandInput.MoveTextEnd(false);
        }

        private void HandleTabCompletion()
        {
            string currentInput = commandInput.text.Trim().ToLower();
            if (string.IsNullOrEmpty(currentInput)) return;
            string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
            if (!string.IsNullOrEmpty(match))
            {
                commandInput.text = match;
                commandInput.MoveTextEnd(false);
            }
        private void ClearTerminal()
        {
            string currentInput = commandInput.text.Trim().ToLower();

            if (string.IsNullOrEmpty(currentInput))
            {
                if (!string.IsNullOrEmpty(_lastSuggestion))
                {
                    commandInput.text = _lastSuggestion;
                    commandInput.MoveTextEnd(false);
                    _lastSuggestion = "";
                }
                return;
            }
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;

            // 🎨 Palette: Enhanced retro terminal startup sequence with simulated session info.
            // Using #AAAAAA for secondary text to meet WCAG AA contrast (4.5:1) on dark backgrounds.
            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss");
            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>\n" +
                            $"<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\nWelcome to the Terminal. Type <color=#00FFFF>'help'</color> for available commands.");
        }

        public void ProcessCommand(string input)
        {
            _historyIndex = -1;
            _persistentInput = "";

            if (string.IsNullOrWhiteSpace(input))
            {
                WriteToTerminal("\n<color=#888888>></color>");
                WriteToTerminal("\n<color=#AAAAAA>></color>");
                CleanupInput();
                return;
            }

            // 🛡️ Sentinel: Sanitize input to escape rich text tags BEFORE any validation or echoing.
            // This prevents UI injection/XSS-like attacks in TextMeshPro.
            // 🛡️ Sentinel: Escaping tags to prevent UI injection.
            // 🛡️ Sentinel: Sanitize input by escaping Rich Text tags to prevent UI injection/DoS.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation pipeline: Validate -> Sanitize -> Echo -> Execute.
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal($"\n<color=#FF0000>[SECURITY]</color>: Input length exceeds limit.");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInput();
                return;
            }

            // 🛡️ Sentinel: Escape Rich Text tags to prevent UI injection attacks.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal($"\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInput();
                return;
            }

            // 🎨 Palette: Echo sanitized input to terminal
            // 🎨 Palette: Echo sanitized input using #AAAAAA for accessible contrast.
            WriteToTerminal($"\n<color=#AAAAAA>> {sanitizedInput}</color>");
            // 🎨 Palette: Echo sanitized input after validation passes
            // 🎨 Palette: Echo sanitized input
            WriteToTerminal($"\n<color=#AAAAAA>> {sanitizedInput}</color>");
            _lastSuggestion = "";
            // 🎨 Palette: Echo sanitized input after validation passes
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
            // 🎨 Palette: Echo sanitized input exactly once after validation.
            WriteToTerminal($"\n<color=#AAAAAA>> {sanitizedInput}</color>");

            if (_commandHistory.Count == 0 || _commandHistory.Last() != input)
            {
                _commandHistory.Add(input);
            }

            string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            if (command == "clear") ClearTerminal();
            else if (command == "history") DisplayHistory();
            else if (command == "help") DisplayHelp();
            else if (command == "infiniteration") ExecuteInfiniteration();
            else HandleUnknownCommand(command);

            CleanupInput();
        }

        private void DisplayHistory()
        {
            // ⚡ Bolt: Using StringBuilder to prevent O(N^2) string allocations and resolving triplicated display logic.
            StringBuilder sb = new StringBuilder("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>");
            if (_commandHistory.Count == 0)
            {
                sb.Append("\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
            StringBuilder sb = new StringBuilder();
            sb.Append("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>");
            if (_commandHistory.Count == 0)
            {
                sb.Append("\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
            // ⚡ Bolt: Use StringBuilder to avoid O(N^2) string allocations during history rendering.
            StringBuilder sb = new StringBuilder(128);
            // ⚡ Bolt: Using StringBuilder to eliminate O(N^2) string allocations in the history display loop.
            StringBuilder sb = new StringBuilder();
            sb.Append("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>");

        private void DisplayHelp()
        {
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                "\n - <color=#00FFFF>help</color>: Show this message." +
                "\n - <color=#00FFFF>clear</color>: Clear terminal." +
                "\n - <color=#00FFFF>history</color>: Show command history." +
                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
                "\n\n<color=#888888>Shortcuts: [Tab] Complete, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
            if (_commandHistory.Count == 0)
            {
                output += "\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>";
                sb.Append("\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
                sb.Append("\n <color=#AAAAAA>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
            }
            else
            {
                for (int i = 0; i < _commandHistory.Count; i++)
                {
                    sb.Append("\n ").Append(i + 1).Append(": <color=#00FFFF>").Append(_commandHistory[i]).Append("</color>");
                }
            }
                    sb.Append($"\n {i + 1}: <color=#00FFFF>{_commandHistory[i]}</color>");
                    // 🛡️ Sentinel: Sanitize history entries to prevent Rich Text UI injection.
                    // 🛡️ Sentinel: Sanitize history entries by escaping Rich Text tags to prevent UI injection/DoS.
                    string sanitizedEntry = _commandHistory[i].Replace("<", "&lt;").Replace(">", "&gt;");
                    sb.Append("\n ").Append(i + 1).Append(": <color=#00FFFF>").Append(sanitizedEntry).Append("</color>");
                }
            }
            WriteToTerminal(output);
            WriteToTerminal(sb.ToString());
        }

        private void DisplayHelp()
        {
            // ⚡ Bolt: Using a single string literal for better interning and to resolve triplicated display logic.
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                "\n - <color=#00FFFF>help</color>: Show this message." +
                "\n - <color=#00FFFF>clear</color>: Clear the terminal display." +
                "\n - <color=#00FFFF>history</color>: Show command history." +
                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
                "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
                "\n - <color=#00FFFF><b>help</b></color>: Show this message." +
                "\n - <color=#00FFFF><b>clear</b></color>: Clear terminal display." +
                "\n - <color=#00FFFF><b>history</b></color>: Show command history." +
                "\n - <color=#00FFFF><b>infiniteration</b></color>: Execute engine algorithm." +
                "\n\n<color=#888888>Shortcuts: [Tab] Complete, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
                            "\n - <color=#00FFFF><b>help</b></color>: Show this message." +
                            "\n - <color=#00FFFF><b>clear</b></color>: Clear the terminal display." +
                            "\n - <color=#00FFFF><b>history</b></color>: Show command history." +
                            "\n - <color=#00FFFF><b>infiniteration</b></color>: Execute engine algorithm." +
                            "\n\n<color=#AAAAAA>Shortcuts: <b>[Tab]</b> Completion | <b>[Up/Down]</b> History | <b>[Esc]</b> Clear Line | <b>[Ctrl+L]</b> Clear Screen</color>");
        }

        private void ExecuteInfiniteration()
        {
            WriteToTerminal("\n<color=#00FF00>[ENGINE]</color>: Initializing Infiniteration Engine..." +
                "\n<color=#FFFF00>Sequence:</color> 108-99-90-81-72-63-54-45-36-27-18-09-108" +
                "\n<color=#00FFFF>[STATUS]</color>: Loop Closed. 12-11-10...01-012");
        }

        private void HandleUnknownCommand(string command)
        {
            // ⚡ Bolt: Consolidating redundant WriteToTerminal calls and triplicated logic.
            // This reduces the number of typewriter coroutines started per unknown command.
            string suggestion = GetFuzzyMatch(command);
            string suggestionText = !string.IsNullOrEmpty(suggestion) ? $" Did you mean <color=#00FFFF>'{suggestion}'</color>?" : "";

            StringBuilder sb = new StringBuilder("\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '").Append(command).Append("'.");
            sb.Append(suggestionText).Append("</color>");
            sb.Append("\n<color=#888888>Tip: Use [Tab] to auto-complete commands.</color>");

            WriteToTerminal(sb.ToString());
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText}</color>" +
                "\n<color=#888888>Tip: Use [Tab] to auto-complete commands. Type <color=#00FFFF>'help'</color> for options.</color>");
                "\n<color=#888888>Tip: Use [Tab] to auto-complete commands or type 'help' for options.</color>");
            _lastSuggestion = GetFuzzyMatch(command);
            bool hasSuggestion = !string.IsNullOrEmpty(_lastSuggestion);
            string suggestionText = hasSuggestion ? $" Did you mean <color=#00FFFF>'{_lastSuggestion}'</color>?" : "";
            string tip = hasSuggestion
                ? "Press [Tab] to accept suggestion, or type 'help' for options."
                : "Use [Tab] to auto-complete commands, or type 'help' for options.";

            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText}</color>" +
                $"\n<color=#AAAAAA>Tip: {tip}</color>");

            StartCoroutine(ShakeInputField());
        }

        private string GetFuzzyMatch(string input)
        {
            string bestMatch = "";
            int minDistance = 3;
            string lowerInput = input.ToLower();
            foreach (string cmd in _availableCommands)
            {
                int dist = GetLevenshteinDistance(lowerInput, cmd);
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
            // ⚡ Bolt: Optimized Levenshtein Distance using stackalloc Span<int> for O(M) space.
            // This eliminates heap allocations for typical command lengths during fuzzy matching.
            // ⚡ Bolt: Optimized Levenshtein Distance using O(M) space and stackalloc to eliminate heap allocations.
            // This significantly reduces GC pressure during fuzzy command matching.
            int n = s.Length, m = t.Length;
            if (n == 0) return m; if (m == 0) return n;
            int[,] d = new int[n + 1, m + 1];
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length, m = t.Length;
            if (n < m) { (s, t) = (t, s); (n, m) = (m, n); }

            int n = s.Length;
            int m = t.Length;
            if (n < m) { string temp = s; s = t; t = temp; int tmp = n; n = m; m = tmp; }

            // Use stackalloc for small strings to avoid heap allocation.
            // Max command length is 256, but typical commands are much shorter.
            if (n == 0) return m;
            // ⚡ Bolt: Optimized Levenshtein Distance using Span<int> and stackalloc to eliminate heap allocations.
            // Uses O(M) space and swaps span references to avoid redundant copies.
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length, m = t.Length;
            if (n < m) { var tempS = s; s = t; t = tempS; var tempN = n; n = m; m = tempN; }

            if (m == 0) return n;

            // ⚡ Bolt: Optimized Levenshtein Distance using O(M) space and stackalloc Span<int> for inputs up to 128 characters.
            // This eliminates heap allocations during high-frequency fuzzy command matching.
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length;
            int m = t.Length;

            if (n < m)
            {
                string tempString = s; s = t; t = tempString;
                int tempInt = n; n = m; m = tempInt;
            }

            int[] v0 = new int[m + 1];
            int[] v1 = new int[m + 1];
            // Max command length is 256, so 257 ints = 1028 bytes, well within safe stack limits.
            Span<int> v0 = stackalloc int[m + 1];
            Span<int> v1 = stackalloc int[m + 1];
            // ⚡ Bolt: Use stack allocation for small strings (common in terminal commands) to avoid heap allocations.
            // Swapping Span references is used instead of CopyTo to save O(M) operations per iteration.
            // ⚡ Bolt: Optimized Levenshtein Distance using Span<int> and stackalloc to eliminate heap allocations.
            // Uses O(M) space and swaps span references to avoid redundant copies.
            Span<int> v0 = m < 128 ? stackalloc int[m + 1] : new int[m + 1];
            Span<int> v1 = m < 128 ? stackalloc int[m + 1] : new int[m + 1];

            for (int i = 0; i <= m; i++) v0[i] = i;

            for (int i = 0; i < n; i++)
            {
                v1[0] = i + 1;
                for (int j = 0; j < m; j++)
                {
                    int cost = (s[i] == t[j]) ? 0 : 1;
                    v1[j + 1] = Math.Min(Math.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }
                v1.CopyTo(v0);

                for (int j = 0; j <= m; j++) v0[j] = v1[j];
                // Swap references for the next iteration to avoid copying the entire buffer.
                // Swap spans to eliminate O(M) copy operations per iteration.
                for (int j = 0; j <= m; j++) v0[j] = v1[j];
                Span<int> temp = v0;
                v0 = v1;
                v1 = temp;
            }
            return v0[m];
        }

        private void CleanupInput() { if (commandInput != null) { commandInput.text = ""; commandInput.ActivateInputField(); } }
        private void CleanupInput()
        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                // Ensure previous message and its cursor logic are handled
                outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
            }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            // 🎨 Palette: Remove cursor before appending new text
            if (outputDisplay.text.EndsWith(CursorChar))
            {
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);
            }

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.ForceMeshUpdate();
            // Remove previous cursor before appending new message
            if (outputDisplay.text.EndsWith("█"))
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);

            if (_typewriterCoroutine != null) StopCoroutine(_typewriterCoroutine);

            // Remove previous cursor before appending new message
            if (outputDisplay.text.EndsWith("█"))
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);

            if (outputDisplay.text.EndsWith("█"))
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);


            // Consolidate: Stop any existing typewriter to start new one, preventing overlap.
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                // 🎨 Palette: When skipping, ensure we don't leave a trailing cursor from the coroutine
                string currentText = outputDisplay.text;
                if (currentText.EndsWith("█")) outputDisplay.text = currentText.Substring(0, currentText.Length - 1);
                outputDisplay.maxVisibleCharacters = int.MaxValue;
                // Ensure all characters (excluding the cursor) are visible
                outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
            }

            // 🎨 Palette: Ensure terminal always ends with the blinking retro cursor
            string finalMessage = message.EndsWith(CursorChar.ToString()) ? message : message + CursorChar;
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(finalMessage));
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            // 🎨 Palette: Prevent "flash" by setting maxVisibleCharacters before appending
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            // Remove cursor from previous message if it exists
            outputDisplay.text += message + "█"; // Append message and the cursor
            // Remove the trailing cursor if it exists before appending
            if (outputDisplay.text.EndsWith("█"))
            {
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);
                outputDisplay.ForceMeshUpdate();
                startVisibleCount = outputDisplay.textInfo.characterCount;
            }

            string messageWithCursor = message + "█";
            outputDisplay.text += messageWithCursor;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount; // includes the cursor

            // We want to reveal up to endVisibleCount - 1 (the cursor) and keep the cursor visible
            for (int i = 1; i <= endVisibleCount - startVisibleCount - 1; i++)
            {
                int currentIndex = startVisibleCount + i;
                outputDisplay.maxVisibleCharacters = currentIndex + 1; // +1 to show the cursor
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            int startVisibleCount = outputDisplay.textInfo.characterCount;

            outputDisplay.text += message + "█"; // Append message and the cursor
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            outputDisplay.ForceMeshUpdate();

            int totalChars = outputDisplay.textInfo.characterCount;
            int endVisibleCount = totalChars - 1; // Exclude cursor from typewriter reveal

            for (int i = startVisibleCount; i < endVisibleCount; i++)
            {
                int currentIndex = i + 1;
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            // 🎨 Palette: Append message and cursor
            outputDisplay.text += message + CursorChar;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;
            int messageEndCount = endVisibleCount - 1;

            for (int i = startVisibleCount + 1; i <= messageEndCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i;

                char c = outputDisplay.textInfo.characterInfo[i - 1].character;
            for (int i = 1; i <= (totalChars - startVisibleCount - 1); i++)
            {
            // Remove the trailing cursor if it exists before appending new text
            if (outputDisplay.text.EndsWith("█"))
            {
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);
                startVisibleCount = Mathf.Max(0, startVisibleCount - 1);
            }

            outputDisplay.text += message + "█";
            outputDisplay.ForceMeshUpdate();

            int totalChars = outputDisplay.textInfo.characterCount;
            int endVisibleCount = totalChars - 1;

            for (int i = startVisibleCount; i < totalChars; i++)
            {
                int currentIndex = i;
            for (int i = 0; i <= (endVisibleCount - startVisibleCount); i++)
            for (int i = 1; i <= (totalChars - startVisibleCount); i++)
            {
                int currentIndex = startVisibleCount + i;
                // Reveal character and ensure cursor remains visible at the end
                outputDisplay.maxVisibleCharacters = _cursorVisible ? currentIndex : currentIndex - 1;
            for (int i = 1; i <= (totalChars - startVisibleCount - 1); i++)
            {
                int currentIndex = startVisibleCount + i;
                outputDisplay.maxVisibleCharacters = _cursorVisible ? currentIndex + 1 : currentIndex;
            for (int i = startVisibleCount; i <= endVisibleCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i;
                if (i > startVisibleCount)
            for (int i = startVisibleCount; i < endVisibleCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i + 1;

                if (currentIndex == 0) continue;

                // Ensure cursor is always visible if we're on the last character
                if (i == endVisibleCount - startVisibleCount)
                {
                    _cursorVisible = true;
                    _nextBlinkTime = Time.unscaledTime + blinkRate;
                }

                char c = outputDisplay.textInfo.characterInfo[currentIndex - 1].character;
                if (i > 0)
                char c = outputDisplay.textInfo.characterInfo[i].character;
                float totalDelay = typingSpeed;

                if (currentIndex <= endVisibleCount)
                {
                    char c = outputDisplay.textInfo.characterInfo[currentIndex - 1].character;
                    float totalDelay = typingSpeed;

                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEndOfSentence = true;
                        if (currentIndex < endVisibleCount)
                        {
                            char nextChar = outputDisplay.textInfo.characterInfo[currentIndex].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence)
                        {
                            bool isEllipsis = (c == '.' && currentIndex - 2 >= 0 && outputDisplay.textInfo.characterInfo[currentIndex - 2].character == '.');
                            totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                        }
                    {
                        bool isEndOfSentence = true;
                        if (currentIndex < endVisibleCount)
                        {
                            char nextChar = outputDisplay.textInfo.characterInfo[currentIndex].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence)
                        {
                            bool isEllipsis = (c == '.' && currentIndex - 2 >= 0 && outputDisplay.textInfo.characterInfo[currentIndex - 2].character == '.');
                            totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                        }
                    bool isEndOfSentence = true;
                    if (i < endVisibleCount)
                    {
                        char nextChar = outputDisplay.textInfo.characterInfo[i].character;
                    if (currentIndex < endVisibleCount - 1)
                    if (i + 1 < endVisibleCount)
                    {
                        char nextChar = outputDisplay.textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }
                    else if (c == ',' || c == ':' || c == ';')
                    {
                        bool isEllipsis = (c == '.' && i - 2 >= 0 && outputDisplay.textInfo.characterInfo[i - 2].character == '.');
                        bool isEllipsis = (currentIndex - 2 >= 0 && outputDisplay.textInfo.characterInfo[currentIndex - 2].character == '.');
                        totalDelay += commaDelay;
                    }

                    yield return GetWait(totalDelay);
                    }

                    yield return GetWait(totalDelay);
                        bool isEllipsis = (c == '.' && i - 1 >= 0 && outputDisplay.textInfo.characterInfo[i - 1].character == '.');
                        totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                    }
                }

                yield return GetWait(totalDelay);
            }

            _typewriterCoroutine = null;
                else if (c == ',' || c == ':' || c == ';')
                {
                    char c = outputDisplay.textInfo.characterInfo[i - 1].character;
                    yield return GetWait((c == '.' || c == '!' || c == '?') ? punctuationDelay : typingSpeed);
                }

                yield return GetWait(totalDelay);
            }

            // 🎨 Palette: Finalize with cursor visible
            outputDisplay.maxVisibleCharacters = endVisibleCount;
            _cursorVisible = true;
            _blinkTimer = 0;
            _typewriterCoroutine = null;
            StartCoroutine(HandleBlinkingCursor());
            }

            _typewriterCoroutine = null;
            UpdateCursorVisibility();
        }

            UpdateCursorVisibility();
        }

            UpdateCursorDisplay();
        }

            _cursorVisible = true;
            UpdateCursorVisibility();
        }

        private IEnumerator HandleBlinkingCursor()
        {
            if (outputDisplay == null) yield break;

            while (_typewriterCoroutine == null)
            {
                int totalChars = outputDisplay.textInfo.characterCount;
                // Toggle cursor visibility using maxVisibleCharacters
                outputDisplay.maxVisibleCharacters = totalChars;
                yield return GetWait(0.5f);
                if (_typewriterCoroutine != null) break;
                outputDisplay.maxVisibleCharacters = Mathf.Max(0, totalChars - 1);
                yield return GetWait(0.5f);
            while (true)
            {
                _cursorVisible = !_cursorVisible;
                UpdateCursorVisibility();
                yield return GetWait(0.53f); // Standard terminal blink rate
            }
        }

        private void UpdateCursorVisibility()
        {
            if (outputDisplay == null) return;

            int totalChars = outputDisplay.textInfo.characterCount;
            if (totalChars == 0 || !outputDisplay.text.EndsWith("█")) return;

            // If typewriter is running, it handles cursor visibility
            if (_typewriterCoroutine != null) return;

            outputDisplay.maxVisibleCharacters = _cursorVisible ? totalChars : Mathf.Max(0, totalChars - 1);
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            if (_historyIndex == -1 && direction == 1)
            {
                _persistentInput = commandInput.text;
            }

            int newIndex = Mathf.Clamp(_historyIndex + direction, -1, _commandHistory.Count - 1);

            if (newIndex != _historyIndex)
            {
                _historyIndex = newIndex;
                commandInput.text = (_historyIndex == -1) ? _persistentInput : _commandHistory[_commandHistory.Count - 1 - _historyIndex];
                commandInput.MoveTextEnd(false);
            }
        }

        private void HandleTabCompletion()
        {
            string currentInput = commandInput.text.Trim().ToLower();

            if (string.IsNullOrEmpty(currentInput))
            {
                if (!string.IsNullOrEmpty(_lastSuggestion))
                {
                    commandInput.text = _lastSuggestion;
                    commandInput.MoveTextEnd(false);
                    _lastSuggestion = "";
                }
                return;
            }

            string? match = _availableCommands.FirstOrDefault(c => c.StartsWith(currentInput));
            if (!string.IsNullOrEmpty(match))
            {
                commandInput.text = match;
                commandInput.MoveTextEnd(false);
            }
        }

        private void CleanupInput()
        {
            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }
        }

        private IEnumerator ShakeInputField()
        {
            if (commandInput == null) yield break;
            Vector3 originalPos = commandInput.transform.localPosition;
            float elapsed = 0f, duration = 0.2f;
            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-5f, 5f);
                commandInput.transform.localPosition = originalPos + new Vector3(x, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            commandInput.transform.localPosition = originalPos;
        }
    }
}
