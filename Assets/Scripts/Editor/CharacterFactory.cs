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
            string fileName = "campaign_master.json";
            string path = Path.Combine("Assets/Scripts/Data", fileName);

            if (!File.Exists(path))
            {
                Debug.LogError($"Campaign master JSON not found at {path}");
                return;
            }

            // NRT Pattern: Explicitly mark deserialized object as nullable
            HorizonGameData? data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to load or parse campaign data.");
                return;
            }
            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            if (data == null || data.characters == null || !data.IsValid())
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            string json = File.ReadAllText(path);
            HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to ensure integrity and prevent out-of-bounds impacts.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation or is malformed.");
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

            // NRT Pattern: Capture property in local variable before iteration
            var characters = data.characters;
            if (characters != null)
            {
                foreach (var charProfile in characters)
                {
                    if (charProfile == null) continue;

                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name;
                    asset.role = charProfile.role;
                    asset.traits = charProfile.traits;
                    asset.behaviorScript = charProfile.behaviorScript;

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
        /// 🛡️ Sentinel: Robust path sanitization helper.
        /// Uses a strict whitelist to prevent directory traversal and hidden files.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                string safeFileName = GetSafeFileName(charProfile.name);
                if (string.IsNullOrEmpty(safeFileName))
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                if (string.IsNullOrEmpty(safeFileName))
                // Malicious JSON could use "../" to attempt writing assets outside the intended directory.
                // We sanitize by replacing invalid chars and ensuring only the filename component is used.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;

                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }
                // Use Path.GetFileName to ensure only the final component is used, and replace spaces.
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

            // Whitelist: Allow only alphanumeric, underscores, and hyphens.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // Strip leading dots/underscores to prevent hidden files or traversal.
            sanitized = sanitized.TrimStart('.', '_');

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Ensure no directory traversal sequences remain and normalize spacing
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path only to avoid internal path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            return sanitized;
        }

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");
            return sanitized.TrimStart('.', '_');
        }
    }
}