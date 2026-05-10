using UnityEngine;
using System;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager _instance;
        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance;
            }
        }

        public HorizonGameData currentCampaignData = null!;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCampaignData();
        }

        private void LoadCampaignData()
        {
            string fileName = "campaign_master.json";
            string filePath;

#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, "Scripts/Data", fileName);
#else
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    currentCampaignData = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Security validation of deserialized data.
                    // SECURITY: Perform validation after deserialization to ensure data integrity
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        Debug.Log($"Campaign data loaded from {fileName}"); // Security: Don't log full paths
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data. Mask runtime exception details and avoid leaking absolute paths in logs.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data.
                        Debug.LogError($"Failed to validate campaign data from {fileName}. Ensure it conforms to security standards.");
                        currentCampaignData = null;
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null;
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                    // Log only the file name, not the absolute path, to prevent information disclosure
                    Debug.LogError($"Error loading campaign data from {Path.GetFileName(filePath)}: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}.");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                Debug.LogError($"Campaign master JSON not found: {Path.GetFileName(filePath)}");

                // Fallback for current environment if needed
                if (!Application.isEditor) {
                     // In some platforms we might need to use UnityWebRequest for StreamingAssets
                }
                Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
        }
    }
}
