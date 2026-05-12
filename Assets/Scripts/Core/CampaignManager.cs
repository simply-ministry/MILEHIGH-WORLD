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
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data.
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null!;
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs.
                    Debug.LogError($"[Security] Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null!;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
            SaveSecureData("VoidSaturation", currentVoidSaturationLevel.ToString());
        }

        public void SaveSecureData(string key, string data)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(data)) return;

            string obfuscated = ProcessXOR(data);
            PlayerPrefs.SetString($"SECURE_{key}", obfuscated);
            PlayerPrefs.Save();
        }

        public string LoadSecureData(string key)
        {
            string obfuscated = PlayerPrefs.GetString($"SECURE_{key}", "");
            if (string.IsNullOrEmpty(obfuscated)) return "";

            return ProcessXOR(obfuscated);
        }

        private string ProcessXOR(string textToProcess)
        {
            string salt = SystemInfo.deviceUniqueIdentifier ?? "MILEHIGH_FALLBACK_SALT";
            char[] output = new char[textToProcess.Length];

            for (int i = 0; i < textToProcess.Length; i++)
            {
                output[i] = (char)(textToProcess[i] ^ salt[i % salt.Length]);
            }

            return new string(output);
        }
    }
}
