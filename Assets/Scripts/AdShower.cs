using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using CodeStage.AntiCheat.Detectors;
using UnityEngine.SceneManagement;

public class AdShower : MonoBehaviour
{
    // Start is called before the first frame update
    private BannerView bannerView;

    public ObscuredBool adrejected = false;//falseなら表示してよし
    private void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
        ObscuredPrefs.OnAlterationDetected += OnCheaterDetected;

        adrejected = ((ObscuredPrefs.HasKey("MusicBuy")&& ObscuredPrefs.GetString("MusicBuy")== "74APNiSAZT73KUH3iCgy") || (ObscuredPrefs.HasKey("NoahBuy")&& ObscuredPrefs.GetString("NoahBuy")== "MzWgSru2NVh7PmuZTueL"));
        Debug.Log("adrejected="+adrejected);
    }

    private void OnCheaterDetected()
    {
       SceneManager.LoadScene("MenuScene");
    }

    public void RequestBanner()
    {
        if(adrejected) return;
#if UNITY_ANDROID && !UNITY_EDITOR
                           ObscuredString adUnitId="ca-app-pub-2102086917963437~5967035255";
#elif UNITY_IPHONE && !UNITY_EDITOR
              ObscuredString adUnitId ="ca-app-pub-2102086917963437/7135777155";
#else
        ObscuredString adUnitId = "unexpected_platform";

#endif
        if (adUnitId == "unexpected_platform") return;
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
    }

    public void Destroy()
    {
        return;
    }
}
