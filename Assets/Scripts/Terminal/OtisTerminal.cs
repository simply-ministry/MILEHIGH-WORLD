using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
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
            // ⚡ Bolt: Using StringBuilder to minimize allocations in the history display loop.
            StringBuilder sb = new StringBuilder("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Command History:</color>");
            if (_commandHistory.Count == 0)
            {
                sb.Append("\n <color=#888888>Tip: History is empty. Use [Up/Down] arrows to navigate past commands once you've entered them!</color>");
            }
            else
            {
                for (int i = 0; i < _commandHistory.Count; i++)
                {
                    sb.Append("\n ").Append(i + 1).Append(": <color=#00FFFF>").Append(_commandHistory[i]).Append("</color>");
                }
            }
            WriteToTerminal(sb.ToString());
        }

        private void DisplayHelp()
        {
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                            "\n - <color=#00FFFF><b>help</b></color>: Show this message." +
                            "\n - <color=#00FFFF><b>clear</b></color>: Clear terminal display." +
                            "\n - <color=#00FFFF><b>history</b></color>: Show command history." +
                            "\n - <color=#00FFFF><b>infiniteration</b></color>: Execute engine algorithm." +
                            "\n\n<color=#888888>Shortcuts: [Tab] Completion, [Up/Down] History, [Esc] Clear Line, [Ctrl+L] Clear Screen</color>");
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
                "\n<color=#888888>Tip: Use [Tab] to auto-complete commands, or type 'help' for options.</color>");
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
            // ⚡ Bolt: Optimized Levenshtein Distance using Span<int> and stackalloc to eliminate heap allocations.
            // Uses O(M) space and swaps span references to avoid redundant copies.
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length;
            int m = t.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            if (n < m)
            {
                string tempS = s; s = t; t = tempS;
                int tempN = n; n = m; m = tempN;
            }

            Span<int> v0 = stackalloc int[m + 1];
            Span<int> v1 = stackalloc int[m + 1];

            for (int i = 0; i <= m; i++) v0[i] = i;

            for (int i = 0; i < n; i++)
            {
                v1[0] = i + 1;
                for (int j = 0; j < m; j++)
                {
                    int cost = (s[i] == t[j]) ? 0 : 1;
                    v1[j + 1] = Mathf.Min(Mathf.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }

                // ⚡ Bolt: Swap spans to eliminate O(M) copy operations per iteration.
                Span<int> temp = v0;
                v0 = v1;
                v1 = temp;
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
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
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
