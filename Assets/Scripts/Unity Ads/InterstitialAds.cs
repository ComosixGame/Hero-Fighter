using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Core;
using System.Threading.Tasks;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Sample Implementation of Unity Mediation
    /// </summary>
    public class InterstitialAds : MonoBehaviour
    {
        [Header("Ad Unit Ids"), Tooltip("Ad Unit Ids for each platform that represent Mediation waterfalls.")]
        public string androidAdUnitId;
        [Tooltip("Ad Unit Ids for each platform that represent Mediation waterfalls.")]
        public string iosAdUnitId;
        public UnityEvent OnAdLoaded;
        public UnityEvent OnAdFailedLoad;
        public UnityEvent ResetLevelWithAdsFailed;
        public UnityEvent OnCloseAds;

        IInterstitialAd m_InterstitialAd;
        private bool loadFailed;

        async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                InitializationComplete();
            }
            catch (Exception e)
            {
                InitializationFailed(e);
            }
        }

        void OnDestroy()
        {
            m_InterstitialAd?.Dispose();
        }

        public async void ShowInterstitial()
        {
            if (m_InterstitialAd?.AdState == AdState.Loaded)
            {
                try
                {
                    InterstitialAdShowOptions showOptions = new InterstitialAdShowOptions();
                    showOptions.AutoReload = true;
                    await m_InterstitialAd.ShowAsync(showOptions);
                }
                catch (ShowFailedException e)
                {
                    Debug.Log($"Interstitial failed to show : {e.Message}");
                }
            }
        }

        public void ResetLevelWithAds() {
            if(loadFailed) {
                ResetLevelWithAdsFailed?.Invoke();
            } else {
                ShowInterstitial();
            }
        }

        async void LoadAd()
        {
            try
            {
                await m_InterstitialAd.LoadAsync();
            }
            catch (LoadFailedException)
            {
                // We will handle the failure in the OnFailedLoad callback
            }
        }

        void InitializationComplete()
        {
            // Impression Event
            MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    m_InterstitialAd = MediationService.Instance.CreateInterstitialAd(androidAdUnitId);
                    break;

                case RuntimePlatform.IPhonePlayer:
                    m_InterstitialAd = MediationService.Instance.CreateInterstitialAd(iosAdUnitId);
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.LinuxEditor:
                    m_InterstitialAd = MediationService.Instance.CreateInterstitialAd(!string.IsNullOrEmpty(androidAdUnitId) ? androidAdUnitId : iosAdUnitId);
                    break;
                default:
                    Debug.LogWarning("Mediation service is not available for this platform:" + Application.platform);
                    return;
            }

            // Load Events
            m_InterstitialAd.OnLoaded += AdLoaded;
            m_InterstitialAd.OnFailedLoad += AdFailedLoad;

            // Show Events
            m_InterstitialAd.OnClosed += AdClosed;
            LoadAd();
        }

        void InitializationFailed(Exception error)
        {
            SdkInitializationError initializationError = SdkInitializationError.Unknown;
            if (error is InitializeFailedException initializeFailedException)
            {
                initializationError = initializeFailedException.initializationError;
            }
            Debug.Log($"Initialization Failed: {initializationError}:{error.Message}");
            OnAdFailedLoad?.Invoke();
            loadFailed = true;
        }

        void AdClosed(object sender, EventArgs args)
        {
            OnCloseAds?.Invoke();
        }

        void AdLoaded(object sender, EventArgs e)
        {
            OnAdLoaded?.Invoke();
        }

        void AdFailedLoad(object sender, LoadErrorEventArgs e)
        {
            loadFailed = true;
            OnAdFailedLoad?.Invoke();
            Debug.Log(e.Message);
        }

        void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log($"Impression event from ad unit id {args.AdUnitId} : {impressionData}");
        }
    }
}
