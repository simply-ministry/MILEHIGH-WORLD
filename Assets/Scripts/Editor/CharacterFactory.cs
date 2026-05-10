using UnityEngine;
using UnityEditor;
using System.IO;
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

            string json = File.ReadAllText(path);
            HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to ensure data integrity
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
8            // SECURITY: Always validate data after deserialization
                // 🛡️ Sentinel: Security validation of deserialized data.
                if (data == null || !data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                if (data == null)
                if (data == null || data.characters == null || !data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data is null or failed validation.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking absolute paths or full stack traces.
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                Debug.LogError("Failed to load or parse campaign data.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || data.characters == null || !data.IsValid())
            if (!data.IsValid())
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (!data.IsValid())
            // 🛡️ Sentinel: Security validation of deserialized data.
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to prevent using malicious or corrupted data
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || !data.IsValid() || data.characters == null)
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // NRT Pattern: Captured local 'data' ensures safety.
            if (!data.IsValid())
            // 🛡️ Sentinel: Security validation of deserialized data.            if (!data.IsValid())
            // SECURITY: Always validate data after deserialization to ensure integrity
                Debug.LogError($"Failed to load or parse campaign data. Error parsing file.");
                // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                Debug.LogError($"Failed to read or parse campaign data from {Path.GetFileName(path)}: {ex.Message}");
                return;
            }

            // SECURITY: Always validate data after deserialization to ensure integrity and prevent resource exhaustion.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("Failed to parse or validate campaign data.");
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

            foreach (var charProfile in data.characters)
            {
                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                // Malicious JSON could use "../" to write assets outside the intended directory.
                string rawName = charProfile.name ?? "unnamed_character";

                // 1. Strip directory traversal sequences using Path.GetFileName.
                string safeFileName = Path.GetFileName(rawName);

                // 2. Replace OS-specific invalid filename characters.
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" or absolute paths to write assets outside the intended directory.
                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                // Malicious JSON could use "../" to write assets outside the intended directory
                string sanitizedName = charProfile.name;
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                // We use Path.GetFileName to extract only the name part and replace OS-specific invalid characters.
                string safeFileName = charProfile.name ?? "unnamed_character";
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                // Use a strict whitelist-based approach to ensure generated paths remain within the intended directory.
                string safeFileName = charProfile.name ?? "unnamed_character";
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = charProfile.name;
                string safeFileName = GetSafeFileName(charProfile.name);
                // Malicious JSON could use directory traversal sequences (e.g., "../") to write assets outside the intended directory.
                string safeFileName = charProfile.name ?? "unnamed_character";
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                }

                // Ensure no directory traversal sequences remain and spaces are replaced
                // 3. Final cleanup: replace spaces with underscores and ensure extension.
                safeFileName = safeFileName.Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                // Ensure no directory traversal sequences remain and replace spaces
                // Ensure no directory traversal sequences remain and replace spaces for cleanliness
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
