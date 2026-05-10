using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    [DefaultExecutionOrder(-100)]
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager? _instance;
        public static CampaignManager Instance
        {
            get
            {
                // BOLT: O(1) access in the common case after initialization
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<CampaignManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CampaignManager");
                    _instance = go.AddComponent<CampaignManager>();
                }
                // BOLT: Use null-forgiving operator as we guarantee _instance is not null here.
                return _instance!;
                return _instance;
            }
        }

        public HorizonGameData currentCampaignData = null!;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && (UnityEngine.Object)_instance != (UnityEngine.Object)this)
            {
                UnityEngine.Object.Destroy(gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
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
                    // SECURITY: Perform validation after deserialization to ensure data integrity.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                    // SECURITY: Perform validation after deserialization to ensure data integrity
                    if (currentCampaignData == null)
                    {
                        Debug.LogError($"Failed to parse campaign data from {fileName}.");
                    if (currentCampaignData != null)
                    {
                        if (currentCampaignData.IsValid())
                        {
                            currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                            // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                            Debug.Log($"Campaign data loaded and validated from {fileName}");
                        }
                        else
                        {
                            Debug.LogError($"Campaign data from {fileName} failed security validation.");
                            currentCampaignData = null;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to parse campaign data from {fileName}.");
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else if (!currentCampaignData.IsValid())
                    {
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null;
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"[Security] Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data.
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data. Mask details to prevent information disclosure.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Mask runtime exception details, avoid leaking absolute paths, and fail securely by not using invalid data.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                        // SECURITY: Fail securely by nullifying corrupted data and logging the error without leaking internal paths.
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null; // Prevent use of invalid data.
                        Debug.LogError($"Campaign data from {fileName} failed security validation or parsing.");
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null!; // Ensure we don't use invalid data
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    Debug.LogError($"[Security] Error loading campaign data from {fileName}: {ex.Message}");
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}.");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    // SECURITY: Fail securely by catching exceptions and masking sensitive details (e.g., stack traces).
                    Debug.LogError($"[Security] Critical error during campaign data load from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}. Error: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                    // Log only the file name, not the absolute path, to prevent information disclosure
                    Debug.LogError($"Error loading campaign data from {Path.GetFileName(filePath)}: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs.
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                // SECURITY: Log only the filename to prevent information disclosure.
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
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
