using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using Milehigh.Data;

namespace Milehigh.Editor
{
    public class CharacterFactory : EditorWindow
    {
        [MenuItem("Milehigh/Import Campaign Data")]
        public static void ImportCampaignData()
        {
            string path = "Assets/Scripts/Data/campaign_master.json";
            if (!File.Exists(path))
            {
                Debug.LogError("Campaign master JSON not found at " + path);
                return;
            }

            HorizonGameData data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (System.Exception ex)
            {
                // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                Debug.LogError($"Failed to read or parse campaign data from {Path.GetFileName(path)}: {ex.Message}");
                return;
            }

            // SECURITY: Always validate data after deserialization to ensure integrity and prevent resource exhaustion.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                return;
            }

            string folderPath = "Assets/Data/Characters";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
                {
                    AssetDatabase.CreateFolder("Assets", "Data");
                }
                AssetDatabase.CreateFolder("Assets/Data", "Characters");
            }

            if (data.characters != null)
            {
                foreach (var charProfile in data.characters)
                {
                    if (charProfile == null) continue;

                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name;
                    asset.role = charProfile.role;
                    asset.traits = charProfile.traits;
                    asset.behaviorScript = charProfile.behaviorScript;
                    asset.health = charProfile.health;
                    asset.resonance = charProfile.resonance;
                    asset.integrity = charProfile.integrity;
                    asset.vanguardMultiplier = charProfile.vanguardMultiplier;

                    // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                    string safeFileName = GetSafeFileName(charProfile.name);
                    string assetPath = $"{folderPath}/{safeFileName}.asset";

                    AssetDatabase.CreateAsset(asset, assetPath);
                    // SECURITY: Log relative asset path to avoid absolute path disclosure.
                    Debug.Log($"Created character asset: {assetPath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Sanitizes a string for use as a file name.
        /// Prevents Path Traversal by using Path.GetFileName and a strict whitelist regex.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // 1. Replace invalid characters with underscores using a strict whitelist.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // 2. Strip leading dots/underscores and handle potential directory traversal.
            sanitized = sanitized.TrimStart('.', '_');

            // Ensure no directory traversal remains
            string safeName = Path.GetFileName(sanitized);

            if (string.IsNullOrEmpty(safeName))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            // 3. Replace whitespaces with underscores.
            safeName = safeName.Replace(" ", "_");

            return safeName;
        }
    }
}
