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
        private string _persistentInput = "";

        // 🎨 Palette: Blinking cursor state
        private const char CursorChar = '█';
        private float _blinkTimer;
        private bool _cursorVisible = true;

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
                    placeholderText.text = "Enter command...";
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
                string preview = sanitizedInput.Length > 16 ? sanitizedInput.Substring(0, 16) + "..." : sanitizedInput;
                WriteToTerminal($"\n<color=#888888>> {preview}</color>");
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
                for (int i = 0; i < _commandHistory.Count; i++) output += $"\n {i + 1}: <color=#00FFFF>{_commandHistory[i]}</color>";
            WriteToTerminal(output);
        }

        private void DisplayHelp()
        {
            WriteToTerminal("\n<color=#00FF00>[SYSTEM]</color>: <color=#FFFF00>Available Commands:</color>" +
                "\n - <color=#00FFFF>help</color>: Show this message." +
                "\n - <color=#00FFFF>clear</color>: Clear terminal." +
                "\n - <color=#00FFFF>history</color>: Show command history." +
                "\n - <color=#00FFFF>infiniteration</color>: Execute engine algorithm." +
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
            WriteToTerminal($"\n<color=#00FF00>[SYSTEM]</color>: <color=#FF0000>Unknown command: '{command}'.{suggestionText} Type <color=#00FFFF>'help'</color> for options.</color>");
            StartCoroutine(ShakeInputField());
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
            int n = s.Length, m = t.Length;
            if (n == 0) return m; if (m == 0) return n;
            int[,] d = new int[n + 1, m + 1];
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

            // 🎨 Palette: Remove cursor before appending new text
            if (outputDisplay.text.EndsWith(CursorChar))
            {
                outputDisplay.text = outputDisplay.text.Substring(0, outputDisplay.text.Length - 1);
            }

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.ForceMeshUpdate();
                outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
            }
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            // 🎨 Palette: Prevent "flash" by setting maxVisibleCharacters before appending
            int startVisibleCount = outputDisplay.textInfo.characterCount;
            outputDisplay.maxVisibleCharacters = startVisibleCount;

            // 🎨 Palette: Append message and cursor
            outputDisplay.text += message + CursorChar;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;
            int messageEndCount = endVisibleCount - 1;

            for (int i = startVisibleCount + 1; i <= messageEndCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i;

                char c = outputDisplay.textInfo.characterInfo[i - 1].character;
                float totalDelay = typingSpeed;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i < endVisibleCount)
                    {
                        char nextChar = outputDisplay.textInfo.characterInfo[i].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence)
                    {
                        bool isEllipsis = (c == '.' && i - 2 >= 0 && outputDisplay.textInfo.characterInfo[i - 2].character == '.');
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

            // 🎨 Palette: Finalize with cursor visible
            outputDisplay.maxVisibleCharacters = endVisibleCount;
            _cursorVisible = true;
            _blinkTimer = 0;
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
