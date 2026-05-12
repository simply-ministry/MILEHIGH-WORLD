using UnityEngine;
using TMPro; // Error 5 Fix
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

        // 🛡️ Sentinel: Security constants for input validation to prevent DoS and malformed input processing.
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);

        private void Start()
        {
            if (outputDisplay != null)
            {
                outputDisplay.text = "[SYSTEM]: OTIS Terminal Online. Type 'help' for commands.";
            }
        }

        private void OnEnable()
        {
            if (commandInput != null)
            {
                commandInput.ActivateInputField();
            }
        }

        public void ProcessCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            // 🛡️ Sentinel: Input validation and DoS protection
            if (input.Length > MaxInputLength)
            {
                outputDisplay.text += "\n[SECURITY]: Input exceeds maximum length (256 characters).";
            // 🛡️ Sentinel: Input validation and DoS protection to prevent malicious command injection or resource exhaustion.
            if (input.Length > MaxInputLength)
            {
                if (outputDisplay != null) outputDisplay.text += "\n[SECURITY]: Input exceeds maximum length.";
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                outputDisplay.text += "\n[SECURITY]: Invalid characters. Use only A-Z, 0-9, spaces, '.', '_', and '-'.";
                if (commandInput != null) StartCoroutine(ShakeInputField());
                return;
            }

            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();
                if (outputDisplay != null) outputDisplay.text += "\n[SECURITY]: Input contains invalid characters.";
                return;
            }

            // UX Enhancement: Clear input and refocus immediately for better flow
            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }

            // Handle basic UX commands
            if (command == "clear")
            {
                outputDisplay.text = "";
                return;
            }

            if (command == "help")
            {
                outputDisplay.text += "\n[SYSTEM]: <color=#FFFF00>Available Commands:</color>";
                outputDisplay.text += "\n - <color=#00FFFF>help</color>: Show this message.";
                outputDisplay.text += "\n - <color=#00FFFF>clear</color>: Clear the terminal display.";
                outputDisplay.text += "\n - <color=#00FFFF>[cmd] [arg1] [arg2]</color>: Execute extended system commands.";
                return;
            }
            string[] parts = input.Trim().Split(' ');

            // Error 12 Fix: Bounds checking before IndexOf/Substring
            if (parts.Length >= 3)
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                    if (outputDisplay != null) outputDisplay.text += $"\n[SYSTEM]: <color=#00FF00>Command '{parts[0]}' executed.</color>";
                }
            }
            else
            {
                // UX Enhancement: Visual feedback for error
                outputDisplay.text += $"\n[SYSTEM]: <color=#FF0000>Unknown command or invalid argument count: '{parts[0]}'</color>";
                if (outputDisplay != null) outputDisplay.text += "\n[SYSTEM]: <color=#FF0000>Invalid argument count.</color>";
                if (commandInput != null) StartCoroutine(ShakeInputField());
            }
        }

        private IEnumerator ShakeInputField()
        {
            Vector3 originalPos = commandInput.transform.localPosition;
            float elapsed = 0f;
            float duration = 0.2f;
            float magnitude = 5f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
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
