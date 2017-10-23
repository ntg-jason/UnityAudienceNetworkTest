using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class NativeAdTest : MonoBehaviour
{
    private NativeAd nativeAd;

    // UI elements in scene
    [Header("Text:")]
    public Text
        title;
    public Text socialContext;
    public Text status; // For testing purposes
    [Header("Images:")]
    public Image
        coverImage;
    public Image iconImage;
    [Header("Buttons:")]
    public Text
        callToAction;
    public Button callToActionButton;

    void Awake()
    {
        this.Log("Native ad ready to load.");
    }

    void OnGUI()
    {
        // Update GUI from native ad
        if (nativeAd != null && nativeAd.CoverImage != null)
        {
            coverImage.sprite = nativeAd.CoverImage;
        }
        if (nativeAd != null && nativeAd.IconImage != null)
        {
            iconImage.sprite = nativeAd.IconImage;
        }
    }

    void OnDestroy()
    {
        // Dispose of native ad when the scene is destroyed
        if (this.nativeAd)
        {
            this.nativeAd.Dispose();
        }
        Debug.Log("NativeAdTest was destroyed!");
    }

    // Load Ad button
    public void LoadAd()
    {
        try
        {
            this.Log("Native ad begin loading...");
            // Create a native ad request with a unique placement ID (generate your own on the Facebook app settings).
            // Use different ID for each ad placement in your app.
            //NativeAd nativeAd = new AudienceNetwork.NativeAd("YOUR_PLACEMENT_ID");
            NativeAd nativeAd = new AudienceNetwork.NativeAd("293038261190429_293039751190280");
            this.nativeAd = nativeAd;

            // Wire up GameObject with the native ad; the specified buttons will be clickable.
            nativeAd.RegisterGameObjectForImpression(gameObject, new Button[] { callToActionButton });


            // Set delegates to get notified on changes or when the user interacts with the ad.
            nativeAd.NativeAdDidLoad = (delegate ()
            {
                this.Log("Native ad loaded.");
                Debug.Log("Loading images...");
                // Use helper methods to load images from native ad URLs
                StartCoroutine(nativeAd.LoadIconImage(nativeAd.IconImageURL));
                StartCoroutine(nativeAd.LoadCoverImage(nativeAd.CoverImageURL));
                Debug.Log("Images loaded.");
                title.text = nativeAd.Title;
                socialContext.text = nativeAd.SocialContext;
                callToAction.text = nativeAd.CallToAction;
                Debug.Log("--------------------------------------AdChoicesImageURL:" + nativeAd.AdChoicesImageURL);
                Debug.Log("--------------------------------------AdChoicesLinkURL:" + nativeAd.AdChoicesLinkURL);
                Debug.Log("--------------------------------------AdChoicesText:" + nativeAd.AdChoicesText);
                Debug.Log("--------------------------------------SocialContext:" + nativeAd.SocialContext);
                Debug.Log("--------------------------------------Subtitle:" + nativeAd.Subtitle);
                Debug.Log("--------------------------------------Title:" + nativeAd.Title);
                Debug.Log("--------------------------------------IconImageURL:" + nativeAd.IconImageURL);
                Debug.Log("--------------------------------------Body:" + nativeAd.Body);
            });
            nativeAd.NativeAdDidFailWithError = (delegate (string error)
            {
                this.Log("Native ad failed to load with error: " + error);
            });
            nativeAd.NativeAdWillLogImpression = (delegate ()
            {
                this.Log("Native ad logged impression.");
            });
            nativeAd.NativeAdDidClick = (delegate ()
            {
                this.Log("Native ad clicked.");
            });

            // Initiate a request to load an ad.
            nativeAd.LoadAd();

            this.Log("Native ad loading...");
        }
        catch (Exception e)
        {
            this.Log(e.ToString());
        }
        
    }

    private string logStr;
    private void Log(string s)
    {
        logStr += s + "\n";
        this.status.text = logStr;
        Debug.Log(s);
    }

    // Next button
    public void NextScene()
    {
        SceneManager.LoadScene("RewardedVideoAdScene");
    }
}
