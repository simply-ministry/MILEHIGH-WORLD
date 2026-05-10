using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
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
                UnityEngine.Debug.LogError("Campaign master JSON not found at " + path);
                return;
            }

            string json = File.ReadAllText(path);
            HorizonGameData data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || !data.IsValid())
            {
                UnityEngine.Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                return;
            }

            string folderPath = "Assets/Data/Characters";
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Data"))
                {
                    UnityEditor.AssetDatabase.CreateFolder("Assets", "Data");
                }
                UnityEditor.AssetDatabase.CreateFolder("Assets/Data", "Characters");
            }

            foreach (var charProfile in data.characters)
            {
                CharacterData asset = UnityEngine.ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                }
                safeFileName = System.IO.Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path to avoid absolute path disclosure
                UnityEngine.Debug.Log($"Created character asset: {assetPath}");
            }

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
