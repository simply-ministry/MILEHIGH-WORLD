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

            HorizonGameData? data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (System.Exception ex)
            {
                // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                Debug.LogError($"Failed to read or parse campaign data from {Path.GetFileName(path)}.");
                Debug.LogError($"Error loading or parsing campaign data from {path}: {ex.Message}");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to ensure integrity and prevent resource exhaustion
            if (data == null || !data.IsValid())
            {

                if (data == null || data.characters == null)
                {
                    Debug.LogError("Failed to parse campaign data.");
                    return;
                }
            }
            catch (System.Exception)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to ensure integrity and prevent DoS.
            // SECURITY: Always validate data after deserialization
            // SECURITY: Always validate data after deserialization to ensure integrity
            // SECURITY: Always validate data after deserialization to prevent using malicious or corrupted data
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
                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name;
                    asset.role = charProfile.role;
                    asset.traits = charProfile.traits;
                    asset.behaviorScript = charProfile.behaviorScript;
                if (charProfile == null) continue;

                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // Sentinel: Fix path traversal vulnerability by sanitizing the character name
                // Prevent attackers from writing assets outside the intended folder
                string safeName = charProfile.name.Replace(" ", "_");
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeName = safeName.Replace(c.ToString(), "");
                }

                string assetPath = $"{folderPath}/{safeName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                Debug.Log($"Created character asset: {assetPath}");
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // Standardized Path Sanitization Sequence:
                // 1. replace invalid chars with underscores
                // 2. use Path.GetFileName to prevent traversal
                // 3. replace whitespace with underscores
                string rawName = charProfile.name ?? "unnamed_character";
                string sanitizedName = rawName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    sanitizedName = sanitizedName.Replace(c, '_');
                }
                sanitizedName = Path.GetFileName(sanitizedName);
                sanitizedName = sanitizedName.Replace(" ", "_");

                string assetPath = $"{folderPath}/{sanitizedName}.asset";
                // 1. Replace invalid filename characters with underscores.
                // 2. Use Path.GetFileName() to prevent directory traversal.
                // 3. Replace whitespaces with underscores.

                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string baseName = string.IsNullOrEmpty(charProfile.name) ? "unnamed_character" : charProfile.name;
                string safeFileName = baseName;
                string baseName = charProfile.name ?? "unnamed_character";

                // Ensure no directory traversal sequences remain
                string safeFileName = Path.GetFileName(baseName);
                string safeFileName = baseName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                // Malicious JSON could use "../" to write assets outside the intended directory
                string sanitizedName = charProfile.name;
                string sanitizedName = string.Join("_", charProfile.name.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";

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

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                // Ensure no directory traversal sequences remain and replace whitespace
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                // Ensure no directory traversal sequences remain
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");
                safeFileName = Path.GetFileName(safeFileName);
                safeFileName = safeFileName.Replace(" ", "_");
                safeFileName = safeFileName.Replace(" ", "_");
                // Ensure no directory traversal sequences remain and replace spaces for clean paths
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");


                // Ensure no directory traversal sequences remain
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                // Ensure no directory separators or traversal sequences remain
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                // Ensure no directory traversal sequences remain
                string safeFileName = Path.GetFileName(sanitizedName);
                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }

            // 🛡️ Sentinel: Use a strict whitelist-based regex to strip dangerous characters and prevent path traversal.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");
            sanitized = sanitized.TrimStart('.', '_'); // Strip leading dots/underscores

            // Ensure no directory traversal remains
            string safeName = Path.GetFileName(sanitized);

            if (string.IsNullOrEmpty(safeName))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            return safeName;
        }
    }
}
