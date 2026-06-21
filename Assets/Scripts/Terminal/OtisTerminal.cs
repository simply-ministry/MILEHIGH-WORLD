using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;
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

        private void Update()
        {
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
            else if (Input.GetKeyDown(KeyCode.L) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) ClearTerminal();
        }

        private void ClearTerminal()
        {
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
            _lastSuggestion = "";

            if (string.IsNullOrWhiteSpace(input))
            {
                WriteToTerminal("\n<color=#AAAAAA>></color>");
                CleanupInput();
                return;
            }

            // 🛡️ Sentinel: Input validation pipeline: Validate -> Sanitize -> Echo -> Execute.
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInput();
                return;
            }

            // 🛡️ Sentinel: Escape Rich Text tags to prevent UI injection attacks.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal($"\n<color=#AAAAAA>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInput();
                return;
            }

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
            // ⚡ Bolt: Using StringBuilder to eliminate O(N^2) string allocations in the history display loop.
            StringBuilder sb = new StringBuilder();
            sb.Append("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>");

            if (_commandHistory.Count == 0)
            {
                sb.Append("\n <color=#AAAAAA>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
            }
            else
            {
                for (int i = 0; i < _commandHistory.Count; i++)
                {
                    // 🛡️ Sentinel: Sanitize history entries by escaping Rich Text tags to prevent UI injection/DoS.
                    string sanitizedEntry = _commandHistory[i].Replace("<", "&lt;").Replace(">", "&gt;");
                    sb.Append("\n ").Append(i + 1).Append(": <color=#00FFFF>").Append(sanitizedEntry).Append("</color>");
                }
            }
            WriteToTerminal(sb.ToString());
        }

        private void DisplayHelp()
        {
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
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
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length;
            int m = t.Length;

            if (n < m)
            {
                string tempS = s; s = t; t = tempS;
                int tempN = n; n = m; m = tempN;
            }

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
                    v1[j + 1] = Mathf.Min(Mathf.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }

                Span<int> temp = v0;
                v0 = v1;
                v1 = temp;
            }
            return v0[m];
        }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            // Consolidate: Stop any existing typewriter to start new one, preventing overlap.
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
            }

            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            // Remove the trailing cursor if it exists before appending new text
            if (outputDisplay.text.EndsWith("█"))
            {
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);
                startVisibleCount = Mathf.Max(0, startVisibleCount - 1);
            }

            outputDisplay.text += message + "█";
            outputDisplay.ForceMeshUpdate();
            int totalChars = outputDisplay.textInfo.characterCount;
            int endVisibleCount = totalChars - 1; // Exclude cursor from typewriter reveal

            for (int i = startVisibleCount; i < endVisibleCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i + 1;

                char c = outputDisplay.textInfo.characterInfo[i].character;
                float totalDelay = typingSpeed;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < endVisibleCount)
                    {
                        char nextChar = outputDisplay.textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence)
                    {
                        bool isEllipsis = (c == '.' && i - 1 >= 0 && outputDisplay.textInfo.characterInfo[i - 1].character == '.');
                        totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                    }
                }
                else if (c == ',' || c == ':' || c == ';')
                {
                    totalDelay += commaDelay;
                }

                yield return GetWait(totalDelay);
            }

            _typewriterCoroutine = null;
            _cursorVisible = true;
            UpdateCursorVisibility();
        }

        private IEnumerator HandleBlinkingCursor()
        {
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
