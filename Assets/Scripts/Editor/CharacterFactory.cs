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
            HorizonGameData data = null;
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
            {2w33 f. 
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
                {
                    AssetDatabase.CreateFolder("Assets", "Data");
                }
                AssetDatabase.CreateFolder("Assets/Data", "Characters");
            }

            foreach (var charProfile in data.characters)
            {
                if (charProfile == null) continue;

                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // Standardized Path Sanitization Sequence:
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
                // Malicious JSON could use "../" to write assets outside the intended directory
                string sanitizedName = charProfile.name;
                string sanitizedName = string.Join("_", charProfile.name.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";

                string sanitizedName = charProfile.name;

                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                }

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

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure

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
