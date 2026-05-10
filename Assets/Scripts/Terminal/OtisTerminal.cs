using UnityEngine;
using TMPro; // Error 5 Fix
using System.Collections.Generic;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

        public void ProcessCommand(string input)
        {
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
