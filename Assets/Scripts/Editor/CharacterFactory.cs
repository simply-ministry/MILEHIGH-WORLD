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
            catch (System.Exception)
            {
                Debug.LogError("Failed to load or parse campaign data.");
                return;
            }

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
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                string safeFileName = GetSafeFileName(charProfile.name);
                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");
            return sanitized.TrimStart('.', '_');
        }
    }
}