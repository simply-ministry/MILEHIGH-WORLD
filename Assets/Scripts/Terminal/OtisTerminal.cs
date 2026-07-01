using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
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

        [Header("Cursor Settings")]
        [SerializeField] private float blinkRate = 0.5f;
        private const char TerminalCursor = '█';

        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9 \t._\-]+$", RegexOptions.Compiled);
        private static readonly string[] _availableCommands = { "help", "clear", "history", "infiniteration" };

        private Coroutine? _typewriterCoroutine;
        private Coroutine? _cursorCoroutine;
        private bool _cursorVisible = true;
        private int _lastCommandFrame;

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
            if (_cursorCoroutine == null)
            {
                _cursorCoroutine = StartCoroutine(HandleBlinkingCursor());
            }
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

            // Typewriter skip detection
            if (_typewriterCoroutine != null && Time.frameCount > _lastCommandFrame)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Space) && string.IsNullOrEmpty(commandInput.text)))
                {
                    FinalizeTypewriter();
                }
            }

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

        private void ClearTerminal()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;

            int hour = DateTime.Now.Hour;
            string greeting = hour switch
            {
                >= 5 and < 12 => "Good morning",
                >= 12 and < 17 => "Good afternoon",
                >= 17 and < 21 => "Good evening",
                _ => "Welcome back"
            };

            string timestamp = DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy");
            WriteToTerminal($"<color=#AAAAAA>Last login: {timestamp} on ttys000</color>" +
                            $"\n<color=#00FF00>[SYSTEM]</color>: OTIS v2.4.0-VOID_LATTICE" +
                            $"\n{greeting}, Operator. Type <color=#00FFFF>'help'</color> for available commands.");
        }

        private IEnumerator HandleBlinkingCursor()
        {
            while (true)
            {
                if (_typewriterCoroutine == null && outputDisplay != null)
                {
                    _cursorVisible = !_cursorVisible;
                    int totalChars = outputDisplay.textInfo.characterCount;
                    if (totalChars > 0 && outputDisplay.text.EndsWith(TerminalCursor))
                    {
                        outputDisplay.maxVisibleCharacters = _cursorVisible ? totalChars : totalChars - 1;
                    }
                }
                yield return GetWait(blinkRate);
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            if (_historyIndex == -1)
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

        public void ProcessCommand(string input)
        {
            _historyIndex = -1;
            _persistentInput = "";

            if (string.IsNullOrWhiteSpace(input))
            {
                if (_typewriterCoroutine != null && Time.frameCount > _lastCommandFrame)
                {
                    FinalizeTypewriter();
                }
                else
                {
                    WriteToTerminal("\n<color=#AAAAAA>></color>");
                }
                CleanupInput();
                return;
            }

            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInput();
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInput();
                return;
            }

            // ⚡ Bolt: Using StringBuilder to consolidate all command output into a single WriteToTerminal call.
            // This eliminates multiple typewriter coroutine restarts and redundant string allocations.
            StringBuilder sb = new StringBuilder("\n<color=#AAAAAA>> ");
            AppendSanitized(sb, input);
            sb.Append("</color>");

            _lastSuggestion = "";

            if (_commandHistory.Count == 0 || _commandHistory.Last() != input)
            {
                _commandHistory.Add(input);
            }

            string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            if (command == "clear")
            {
                ClearTerminal();
                CleanupInput();
                return;
            }

            if (command == "history") sb.Append(GetHistoryText());
            else if (command == "help") sb.Append(GetHelpText());
            else if (command == "infiniteration") sb.Append(GetInfiniterationText());
            else sb.Append(GetUnknownCommandText(command));

            _lastCommandFrame = Time.frameCount;
            WriteToTerminal(sb.ToString());
            CleanupInput();
        }

        private string GetHistoryText()
        {
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
                    sb.Append("\n ").Append(i + 1).Append(": <color=#00FFFF>");
                    AppendSanitized(sb, _commandHistory[i]);
                    sb.Append("</color>");
                }
            }
            return sb.ToString();
        }

        private string GetHelpText()
        {
            return "\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                   "\n - <color=#00FFFF><b>help</b></color>: Show this message." +
                   "\n - <color=#00FFFF><b>clear</b></color>: Clear the terminal display." +
                   "\n - <color=#00FFFF><b>history</b></color>: Show command history." +
                   "\n - <color=#00FFFF><b>infiniteration</b></color>: Execute engine algorithm." +
                   "\n\n<color=#AAAAAA>Shortcuts: <b>[Tab]</b> Completion | <b>[Up/Down]</b> History | <b>[Esc]</b> Clear Line | <b>[Ctrl+L]</b> Clear Screen</color>" +
                   "\n<color=#AAAAAA>Reveal: <b>[Space/Return/Esc]</b> Skip typewriter reveal.</color>";
        }

        private string GetInfiniterationText()
        {
            return "\n<color=#00FF00>[ENGINE]</color>: Initializing Infiniteration Engine..." +
                   "\n<color=#FFFF00>Sequence:</color> 108-99-90-81-72-63-54-45-36-27-18-09-108" +
                   "\n<color=#00FFFF>[STATUS]</color>: Loop Closed. 12-11-10...01-012";
        }

        private string GetUnknownCommandText(string command)
        {
            _lastSuggestion = GetFuzzyMatch(command);
            bool hasSuggestion = !string.IsNullOrEmpty(_lastSuggestion);
            string suggestionText = hasSuggestion ? $" Did you mean <color=#00FFFF>'{_lastSuggestion}'</color>?" : "";
            string tip = hasSuggestion
                ? "Press [Tab] to accept suggestion, or type 'help' for options."
                : "Use [Tab] to auto-complete commands, or type 'help' for options.";

            StartCoroutine(ShakeInputField());

            return $"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText}</color>" +
                   $"\n<color=#AAAAAA>Tip: {tip}</color>";
        }

        private void AppendSanitized(StringBuilder sb, string text)
        {
            foreach (char c in text)
            {
                if (c == '<') sb.Append("&lt;");
                else if (c == '>') sb.Append("&gt;");
                else sb.Append(c);
            }
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

            int n = s.Length, m = t.Length;
            if (n < m) { var tempS = s; s = t; t = tempS; var tempN = n; n = m; m = tempN; }

            if (m == 0) return n;

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

                Span<int> temp = v0;
                v0 = v1;
                v1 = temp;
            }
            return v0[m];
        }

        private void FinalizeTypewriter()
        {
            if (_typewriterCoroutine == null) return;

            StopCoroutine(_typewriterCoroutine);
            _typewriterCoroutine = null;

            if (outputDisplay != null)
            {
                outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
                _cursorVisible = true;
            }
        }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            FinalizeTypewriter();

            if (outputDisplay.text.EndsWith(TerminalCursor))
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);

            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            int startVisibleCount = outputDisplay.textInfo.characterCount;
            outputDisplay.text += message + TerminalCursor;
            outputDisplay.ForceMeshUpdate();

            int totalChars = outputDisplay.textInfo.characterCount;
            int endVisibleCount = totalChars - 1;

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
                        bool isEllipsis = (i > 0 && outputDisplay.textInfo.characterInfo[i - 1].character == '.');
                        totalDelay += isEllipsis ? typingSpeed * 3f : punctuationDelay;
                    }
                }
                else if (c == ',' || c == ':' || c == ';')
                {
                    totalDelay += commaDelay;
                }

                yield return GetWait(totalDelay);
            }

            outputDisplay.maxVisibleCharacters = totalChars;
            _cursorVisible = true;
            _typewriterCoroutine = null;
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
