using UnityEngine;
using TMPro; // Error 5 Fix
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

        // 🛡️ Sentinel: Security constants for input validation to prevent DoS and malformed input processing.
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);

        public void ProcessCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            // 🛡️ Sentinel: Input validation and DoS protection
            if (input.Length > MaxInputLength)
            {
                outputDisplay.text += "\n[SECURITY]: Input exceeds maximum length.";
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                outputDisplay.text += "\n[SECURITY]: Input contains invalid characters.";
                return;
            }

            string[] parts = input.Split(' ');

            // Error 12 Fix: Bounds checking before IndexOf/Substring
            if (parts.Length >= 3)
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                }
            }
            else
            {
                outputDisplay.text += "\n[SYSTEM]: Invalid argument count.";
            }
        }

        private void ExecuteExtendedCommand(string cmd, string args)
        {
            // Implementation for specific terminal logic
        }
    }
}
