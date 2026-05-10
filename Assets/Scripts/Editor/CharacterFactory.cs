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

            HorizonGameData? data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
                string fileJson = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(fileJson);

                if (data == null || data.characters == null)
                {
                    Debug.LogError("Failed to parse campaign data.");
                    return;
                }

                // 🛡️ Sentinel: Security validation of deserialized data.
                // SECURITY: Always validate data after deserialization to ensure integrity
                if (!data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError($"[Security] Failed to load or parse campaign data: {ex.Message}");
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to prevent using malicious or corrupted data
            // SECURITY: Always validate data after deserialization to ensure integrity
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            string json = File.ReadAllText(path);
            HorizonGameData data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            // SECURITY: Always validate data after deserialization to prevent processing of malicious or malformed content.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            // 🛡️ Sentinel: Security validation of deserialized data.
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

            foreach (var charProfile in data.characters)
            {
                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name ?? "unnamed";
                asset.role = charProfile.role ?? "none";
                asset.traits = charProfile.traits ?? new string[0];
                asset.behaviorScript = charProfile.behaviorScript ?? "";

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                // We use Path.GetInvalidFileNameChars to filter out OS-level invalid characters and Path.GetFileName to strip directory traversal sequences.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                // We use Path.GetFileName to extract only the name part and replace OS-specific invalid characters.
                string baseName = charProfile.name;
                if (string.IsNullOrEmpty(baseName)) baseName = "unnamed_character";

                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string baseName = charProfile.name ?? "unnamed_character";
                string sanitizedName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                string safeFileName = baseName;
                // Malicious JSON could use "../" or absolute paths to write assets outside the intended directory.
                string baseName = charProfile.name ?? "unnamed_character";

                // 1. Replace invalid filename characters with underscores
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;

                // Malicious JSON could use "../" to write assets outside the intended directory.
                // Required sequence: replace invalid chars, use Path.GetFileName, then replace spaces.
                string rawName = charProfile.name ?? "unnamed_character";

                // 1. Strip directory traversal sequences using Path.GetFileName.
                string safeFileName = Path.GetFileName(rawName);

                // 2. Replace OS-specific invalid filename characters.
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = Path.GetFileName(baseName);
                string safeFileName = baseName;
                string sanitizedName = charProfile.name ?? "unnamed_character";
                string safeFileName = sanitizedName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    baseName = baseName.Replace(c, '_');
                }
                safeFileName = safeFileName.Replace(" ", "_");
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                // Ensure no directory traversal sequences remain and normalize whitespace
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                safeFileName = safeFileName.Replace(" ", "_");

                // Ensure no directory traversal sequences remain and finalize the name
                safeFileName = Path.GetFileName(safeFileName);

                // 2. Use Path.GetFileName to strip any remaining directory traversal sequences (like "../")
                // and replace spaces with underscores for better compatibility.
                string safeFileName = Path.GetFileName(baseName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.CreateAsset((UnityEngine.Object)asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");


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
                // SECURITY: Log relative asset path to avoid absolute path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
