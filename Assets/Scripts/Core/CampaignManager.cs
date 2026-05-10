using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager? _instance;
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
                return _instance!;
            }
        }

        public HorizonGameData currentCampaignData = null!;
        public HorizonGameData? currentCampaignData;
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
                    // NRT Pattern: Explicitly mark deserialized object as nullable
                    HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Perform validation after deserialization to ensure data integrity.
                    // NRT Pattern: Use local variable 'data' for consistent flow analysis.
                    if (data != null && data.IsValid())
                    {
                        currentCampaignData = data;
                        currentVoidSaturationLevel = data.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                    // 🛡️ Sentinel: Security validation of deserialized data.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation.");
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        // 🛡️ Sentinel: Failed validation means we cannot trust the campaign data.
                        // SECURITY: Log the validation failure without exposing internal paths
                        Debug.LogError($"Campaign data from {fileName} failed security validation or is malformed.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    // SECURITY: Catch exceptions to fail securely and avoid leaking internal stack traces or paths.
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}. Error: {ex.Message}");
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}");
                    currentCampaignData = null;
                }
            }
            else
            {
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
