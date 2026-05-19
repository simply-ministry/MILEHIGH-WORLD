// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using UnityEngine;
using System.IO;
using MilehighWorld.Data;

namespace MilehighWorld.Core
{
    [UnityEngine.DefaultExecutionOrder(-100)]
    public class CampaignManager : UnityEngine.MonoBehaviour
    {
        private static CampaignManager? _instance;

        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindAnyObjectByType<CampaignManager>();
                    if (_instance == null)
                    {
                        UnityEngine.GameObject go = new UnityEngine.GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance;
            }
        }

        public HorizonGameData? currentCampaignData;
        public float currentVoidSaturationLevel;

        // 🛡️ Sentinel: Cache deviceUniqueIdentifier to avoid expensive native boundary crossing
        private static string? _cachedDeviceIdentifier;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                UnityEngine.Object.Destroy(this.gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            LoadCampaignData();
        }

        private void LoadCampaignData()
        {
            const string fileName = "campaign_master.json";
            string filePath;

#if UNITY_EDITOR
            filePath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Scripts/Data", fileName);
#else
            filePath = System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, fileName);
#endif

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    HorizonGameData? data = UnityEngine.JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Perform security and integrity validation after deserialization.
                    if (data != null && data.IsValid())
                    {
                        currentCampaignData = data;
                        if (data.metadata != null)
                        {
                            currentVoidSaturationLevel = data.metadata.voidSaturationLevel;
                        }
                        LogSecureInfo($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        LogSecureError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null;
                    }
                }
                catch (System.Exception ex)
                {
                    LogSecureError($"[Security] Error loading campaign data from {fileName}: ({ex.GetType().Name})");
                    currentCampaignData = null;
                }
            }
            else
            {
                LogSecureError($"Campaign master JSON not found: {fileName}");
            }
        }

        private void LogSecureInfo(string message)
        {
            // SECURITY: Ensure message doesn't contain sensitive path info if passed dynamically,
            // but here we just wrap UnityEngine.Debug for consistency.
            UnityEngine.Debug.Log(message);
        }

        private void LogSecureError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = UnityEngine.Mathf.Clamp01(currentVoidSaturationLevel + amount);
            UnityEngine.Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
            this.SaveSecureData("VoidSaturation", this.currentVoidSaturationLevel.ToString());
        }

        public void SaveSecureData(string key, string data)
        {
            if (System.String.IsNullOrEmpty(key) || System.String.IsNullOrEmpty(data)) return;

            string obfuscated = this.ProcessXOR(data);
            UnityEngine.PlayerPrefs.SetString($"SECURE_{key}", obfuscated);
            UnityEngine.PlayerPrefs.Save();
        }

        public string LoadSecureData(string key)
        {
            string obfuscated = UnityEngine.PlayerPrefs.GetString($"SECURE_{key}", "");
            if (System.String.IsNullOrEmpty(obfuscated)) return "";

            return this.ProcessXOR(obfuscated);
        }

        private string ProcessXOR(string textToProcess)
        {
            if (string.IsNullOrEmpty(_cachedDeviceIdentifier))
            {
                _cachedDeviceIdentifier = UnityEngine.SystemInfo.deviceUniqueIdentifier;
                if (string.IsNullOrEmpty(_cachedDeviceIdentifier) || _cachedDeviceIdentifier == "n/a")
                {
                    _cachedDeviceIdentifier = "MILEHIGH_FALLBACK_STABLE_SALT_2025";
                }
            }

            string salt = _cachedDeviceIdentifier;
            char[] output = new char[textToProcess.Length];

            for (int i = 0; i < textToProcess.Length; i++)
            {
                output[i] = (char)(textToProcess[i] ^ salt[i % salt.Length]);
            }

            return new string(output);
        }
    }
}
