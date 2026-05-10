using UnityEngine;
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

        public HorizonGameData currentCampaignData;
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
            filePath = Path.Combine(Application.dataPath, "Scripts/Data");
            filePath = Path.Combine(filePath, fileName);
#else
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    currentCampaignData = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Security validation of deserialized data to ensure data integrity
                    // 🛡️ Sentinel: Security validation of deserialized data.
                    // SECURITY: Perform validation after deserialization to ensure data integrity
                    // 🛡️ Sentinel: Perform validation after deserialization to ensure data integrity.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log("Campaign data loaded and validated from " + fileName);
                    }
                    else
                    {
                        // SECURITY: Explicitly nullify data on validation failure to prevent use of insecure state
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError("Failed to parse or validate campaign data from " + fileName);
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        Debug.LogError($"[Security] Campaign data from {fileName} failed security validation or parsing.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        // 🛡️ Sentinel: Combined logic to handle both deserialization failure and security validation rejection.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        // SECURITY: Log only the file name to avoid absolute path disclosure
                        Debug.LogError($"Failed to validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely.
                    // Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                        Debug.LogError($"[Security] Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        // SECURITY: Fail securely and don't use invalid data. Mask runtime exception details and avoid leaking absolute paths.
                        Debug.LogError($"Campaign data from {fileName} failed security validation or parsing.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        // SECURITY: If data fails validation, ensure it's not used by the application
                        Debug.LogError($"Failed to validate campaign data from {fileName}.");
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                    else
                    {
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception)
                {
                    // SECURITY: Fail securely and avoid leaking internal details.
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}: {ex.Message}");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError("Error loading campaign data from " + fileName + ": " + ex.Message);
                    Debug.LogError($"Error loading or parsing campaign data from {fileName}: {ex.Message}");
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null; // Ensure failure state is consistent
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path.
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                Debug.LogError("Campaign master JSON not found: " + fileName);
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
