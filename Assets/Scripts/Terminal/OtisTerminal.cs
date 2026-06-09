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

        [Header("Cursor Settings")]
        [SerializeField] private float blinkRate = 0.5f;
        private const char CursorChar = '█';
        private float _nextBlinkTime;
        private bool _cursorVisible = true;

        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);
        private static readonly string[] _availableCommands = { "help", "clear", "history", "infiniteration" };

        private Coroutine? _typewriterCoroutine;
        private readonly List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;
        private string _persistentInput = "";

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
                    placeholderText.text = "Enter command (type 'help' for info)...";
                commandInput.onSubmit.AddListener(ProcessCommand);
            }
            ClearTerminal();
        }

        private void OnEnable() => commandInput?.ActivateInputField();

        private void ClearTerminal()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;
            WriteToTerminal("<color=#00FF00>[SYSTEM]</color>: OTIS Terminal Online. Type 'help' for commands.");
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
            else if (Input.GetKeyDown(KeyCode.L) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) ClearTerminal();
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

            // Save current input if we are starting to navigate from the prompt
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
            if (string.IsNullOrEmpty(currentInput)) return;

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
                WriteToTerminal("\n<color=#888888>></color>");
                CleanupInput();
                return;
            }

            // 🛡️ Sentinel: Sanitize input to escape rich text tags BEFORE any validation or echoing.
            string sanitizedInput = input.Replace("<", "&lt;").Replace(">", "&gt;");

            // 🛡️ Sentinel: Input validation and DoS protection
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Input exceeds maximum length.");
                CleanupInput();
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");
                WriteToTerminal("\n<color=#FF0000>[SECURITY]</color>: Invalid characters detected.");
                CleanupInput();
                return;
            }

            // 🎨 Palette: Echo sanitized input
            WriteToTerminal($"\n<color=#888888>> {sanitizedInput}</color>");

            // Update command history
            if (_commandHistory.Count == 0 || _commandHistory.Last() != input)
            {
                _commandHistory.Add(input);
            }
            _persistentInput = "";

            string[] parts = input.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
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
            string output = "\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>";
            if (_commandHistory.Count == 0)
                output += "\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>";
            else
            {
                for (int i = 0; i < _commandHistory.Count; i++)
                {
                    output += $"\n {i + 1}: <color=#00FFFF>{_commandHistory[i]}</color>";
                }
            }
            WriteToTerminal(output);
        }

        private void DisplayHelp()
        {
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                "\n - <color=#00FFFF><b>help</b></color>: Show this message." +
                "\n - <color=#00FFFF><b>clear</b></color>: Clear terminal." +
                "\n - <color=#00FFFF><b>history</b></color>: Show command history." +
                "\n - <color=#00FFFF><b>infiniteration</b></color>: Execute engine algorithm." +
                "\n\n<color=#888888>Shortcuts: [Tab] Complete, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
        }

        private void ExecuteInfiniteration()
        {
            WriteToTerminal("\n<color=#00FF00>[ENGINE]</color>: Initializing Infiniteration Engine..." +
                "\n<color=#FFFF00>Sequence:</color> 108-99-90-81-72-63-54-45-36-27-18-09-108" +
                "\n<color=#00FFFF>[STATUS]</color>: Loop Closed. 12-11-10...01-012");
        }

        private void HandleUnknownCommand(string command)
        {
            string suggestion = GetFuzzyMatch(command);
            string suggestionText = !string.IsNullOrEmpty(suggestion) ? $" Did you mean <color=#00FFFF>'{suggestion}'</color>?" : "";
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText}</color>" +
                "\n<color=#888888>Tip: Use [Tab] to auto-complete commands.</color>");
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
            // ⚡ Bolt: Optimized Levenshtein Distance using O(M) space instead of O(N*M).
            // This significantly reduces heap allocations during fuzzy command matching.
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length, m = t.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            if (n < m) { string temp = s; s = t; t = temp; int tmp = n; n = m; m = tmp; }

            int[] v0 = new int[m + 1];
            int[] v1 = new int[m + 1];

            for (int i = 0; i <= m; i++) v0[i] = i;

            for (int i = 0; i < n; i++)
            {
                v1[0] = i + 1;
                for (int j = 0; j < m; j++)
                {
                    int cost = (s[i] == t[j]) ? 0 : 1;
                    v1[j + 1] = Mathf.Min(Mathf.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }
                for (int j = 0; j <= m; j++) v0[j] = v1[j];
            }
            return v0[m];
        }

        private void CleanupInput()
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

            // 🎨 Palette: Ensure terminal always ends with the blinking retro cursor
            string finalMessage = message.EndsWith(CursorChar.ToString()) ? message : message + CursorChar;
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(finalMessage));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            // 🎨 Palette: Prevent "flash" by setting maxVisibleCharacters before appending
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;
            outputDisplay.maxVisibleCharacters = startVisibleCount;

            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;

            for (int i = 1; i <= endVisibleCount - startVisibleCount; i++)
            {
                int currentIndex = startVisibleCount + i;
                outputDisplay.maxVisibleCharacters = currentIndex;

                // Ensure cursor is always visible if we're on the last character
                if (i == endVisibleCount - startVisibleCount)
                {
                    _cursorVisible = true;
                    _nextBlinkTime = Time.unscaledTime + blinkRate;
                }

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
