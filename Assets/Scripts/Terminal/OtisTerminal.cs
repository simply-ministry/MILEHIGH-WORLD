using UnityEngine;
using TMPro; // Error 5 Fix
using System.Collections.Generic;
using System.Collections;

namespace Milehigh.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

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

            string[] parts = input.Trim().Split(' ');

            // UX Enhancement: Clear input and refocus immediately for better flow
            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }

            // Error 12 Fix: Bounds checking before IndexOf/Substring
            if (parts.Length >= 3)
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                    outputDisplay.text += $"\n[SYSTEM]: <color=#00FF00>Command '{parts[0]}' executed.</color>";
                }
            }
            else
            {
                // UX Enhancement: Visual feedback for error
                outputDisplay.text += "\n[SYSTEM]: <color=#FF0000>Invalid argument count.</color>";
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
