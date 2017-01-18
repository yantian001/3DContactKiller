using UnityEngine;
using System.Collections;
using AudienceNetwork;
using System;
using UnityEngine.UI;

public class Native : MonoBehaviour
{

    public static string nativePlacementID = "814021835405350_814023708738496";

    protected NativeAd nativeAd;

    public Text title;

    public Text socialContext;

    public RawImage coverSprite = null;

    public RawImage iconSprite = null;

    public Text callToAction;

    public Button[] callToActionButton;

    public string defaultGPUrl = "";
    public string defaultiOSUrl = "";
    // Use this for initialization
    void Start()
    {

        RegDefaultAction();

        nativeAd = new NativeAd(nativePlacementID);
        nativeAd.RegisterGameObjectForImpression(gameObject, new Button[] { });
        nativeAd.NativeAdDidLoad = OnNativeAdDidLoad;
        nativeAd.NativeAdWillLogImpression = delegate () { Debug.Log("Native will impression!!"); };
        nativeAd.NativeAdDidFailWithError = delegate (string error) { Debug.Log("Native error :" + error); };
        nativeAd.LoadAd();
        // texture.te

    }

    private void RegDefaultAction()
    {
        // throw new NotImplementedException();
        foreach (Button button in callToActionButton)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
#if UNITY_ANDROID
                Application.OpenURL(defaultGPUrl);

#elif UNITY_IOS
                 Application.OpenURL(defaultiOSUrl);
#endif
            });
        }
    }

    void OnNativeAdDidLoad()
    {
        Debug.Log("native ad Loaded.");
        StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
        StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
        nativeAd.RegisterGameObjectForImpression(gameObject, callToActionButton);
        title.text = nativeAd.Title;
        socialContext.text = nativeAd.SocialContext;
        callToAction.text = nativeAd.CallToAction;
        nativeAd.ExternalLogImpression();
    }

    public void OnGUI()
    {
        if (nativeAd != null)
        {
            if (nativeAd.IconImage)
            {
                this.iconSprite.texture = nativeAd.IconImage.texture;
            }
            if (nativeAd.CoverImage)
            {
                this.coverSprite.texture = nativeAd.CoverImage.texture;
            }
        }
    }
}
