using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    [DefaultExecutionOrder(-100)]
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager _instance;
        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                if (_instance == null) InitializeInstance();
                return _instance!;
            }
        }

        private static void InitializeInstance()
        {
            _instance = FindObjectOfType<CampaignManager>();
            if (_instance == null)
            {
                GameObject go = new GameObject("CampaignManager");
                _instance = go.AddComponent<CampaignManager>();
            }
        }

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
            string filePath = "";

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

                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                    // 🛡️ Sentinel: Perform security validation after deserialization to ensure data integrity and prevent DoS.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data. Consolidate redundant logging.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: If data fails validation, ensure it's not used by the application and log a safe error message.
                        Debug.LogError($"[Security] Campaign data from {fileName} failed security validation or parsing.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception)
                {
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // Consolidate redundant comments and remove ex.Message to prevent path leakage.
                    Debug.LogError($"Error loading campaign data from {fileName}. Parsing failure.");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Do not log ex.Message to avoid leaking absolute file paths or system details.
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely.
                    // Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                Debug.LogError($"Campaign master JSON not found: {fileName}");
                currentCampaignData = null;
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            Debug.Log("Void Saturation Level: " + currentVoidSaturationLevel);
        }
    }
}
