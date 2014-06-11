using UnityEngine;
using System;
using System.Collections;
using System.Net;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.log;
using com.shephertz.app42.paas.sdk.csharp.storage;

public class DemoTitleScreenGUI : MonoBehaviour {

    // App42 Stuff
    ServiceAPI serviceAPI;
    LogService logService;
    StorageService storageService;
    Constants constants = new Constants();
    LogResponse logCallBack = new LogResponse();

    // GUI Skin
    public GUISkin activeSkin;

    // Page variables
    public Rect logoContainerRect;
    public float logoContainerWidth;
    public float logoContainerHeight;

    public Rect logoRect;
    public Texture logoTexture;
    public float logoWidth;
    public float logoHeight;
    public float logoXOffset;
    public float logoYOffset;

    public Rect demoTextRect;
    private float demoTextFontScale = 0.13f;

    public Rect navContainerRect;
    public float navContainerWidth;
    public float navContainerHeight;
    public float navContainerXOffset;
    public float navContainerYOffset;
    public float navFontScaling = 0.5f;
    public float navTopPaddingScaling = 0.39f;
    public float navBottomPaddingScaling = 0f;

    #region Navigation Buttons

    /* Buttons
     * ╔══════════════╗
     * ║   Button A   ║
     * ╠══════════════╣
     * ║   Button B   ║
     * ╠══════════════╣
     * ║   Button C   ║
     * ╠══════════════╣
     * ║   Button D   ║
     * ╠══════════════╣
     * ║   Button E   ║
     * ╚══════════════╝
     */

    public float btnWidth;
    public float btnHeight;

    public Rect A_btnRect;
    public Rect B_btnRect;
    public Rect C_btnRect;
    public Rect D_btnRect;
    public Rect E_btnRect;

    private ButtonHandler A_btnScale;
    private ButtonHandler B_btnScale;
    private ButtonHandler C_btnScale;
    private ButtonHandler D_btnScale;
    private ButtonHandler E_btnScale;

    private GUIStyle A_btnStyle;
    private GUIStyle B_btnStyle;
    private GUIStyle C_btnStyle;
    private GUIStyle D_btnStyle;
    private GUIStyle E_btnStyle;

    #endregion
#if UNITY_EDITOR
    public static bool Validator(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
    { return true; }
#endif

    void Awake()
    {
        #region App42

        #if UNITY_EDITOR
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        #endif

        // Connect to the app service
        serviceAPI = new ServiceAPI(constants.apiKey, constants.secretKey);

        // Build the log service
        logService = serviceAPI.BuildLogService();
        
        // Build the storage service
        storageService = serviceAPI.BuildStorageService();

        // Log the event
        logService.SetEvent("[DEMO] Title Screen", "Landed", logCallBack);

        #endregion

        #region GUI
        logoContainerWidth = Screen.width * 0.6f;
        logoContainerHeight = Screen.height;
        logoContainerRect = new Rect(0, 0, logoContainerWidth, logoContainerHeight);

        logoTexture = activeSkin.customStyles[0].normal.background;
        logoWidth = logoContainerWidth;
        logoHeight = logoWidth * ((float)logoTexture.height / (float)logoTexture.width);
        logoXOffset = 0;
        logoYOffset = 0;
        logoRect = new Rect(logoXOffset, logoYOffset, logoWidth, logoHeight);

        // Font auto scaling
        activeSkin.label.fontSize = (int)(Screen.height * demoTextFontScale);
        demoTextRect = new Rect(0, logoHeight * 0.8f, logoContainerWidth, activeSkin.label.fontSize * 2);

        navContainerWidth = Screen.width * 0.3f;
        navContainerXOffset = logoContainerWidth;

        btnWidth = navContainerWidth;
        btnHeight = navContainerWidth * ((float)activeSkin.button.normal.background.height /
                                         (float)activeSkin.button.normal.background.width);

        navContainerHeight = btnHeight * 5;
        navContainerYOffset = (Screen.height - navContainerHeight) * 0.5f;

        navContainerRect = new Rect(navContainerXOffset, navContainerYOffset, navContainerWidth, navContainerHeight);
        A_btnRect = new Rect(0, 0, btnWidth, btnHeight);
        B_btnRect = new Rect(0, btnHeight, btnWidth, btnHeight);
        C_btnRect = new Rect(0, btnHeight * 2, btnWidth, btnHeight);
        D_btnRect = new Rect(0, btnHeight * 3, btnWidth, btnHeight);
        E_btnRect = new Rect(0, btnHeight * 4, btnWidth, btnHeight);

        // Initialise button scalers
        A_btnScale = new ButtonHandler(A_btnRect, gameObject, 0.9f, "A_ScaleButton");
        B_btnScale = new ButtonHandler(B_btnRect, gameObject, 0.9f, "B_ScaleButton");
        C_btnScale = new ButtonHandler(C_btnRect, gameObject, 0.9f, "C_ScaleButton");
        D_btnScale = new ButtonHandler(D_btnRect, gameObject, 0.9f, "D_ScaleButton");
        E_btnScale = new ButtonHandler(E_btnRect, gameObject, 0.9f, "E_ScaleButton");

        // Nav button font scaling
        activeSkin.button.fontSize = (int)(btnHeight * navFontScaling);
        // Padding Scaling
        activeSkin.button.padding.top = (int)(activeSkin.button.fontSize * navTopPaddingScaling);
        activeSkin.button.padding.bottom = (int)(activeSkin.button.fontSize * navBottomPaddingScaling);
        // Set the styles
        A_btnStyle = new GUIStyle(activeSkin.button);
        B_btnStyle = new GUIStyle(activeSkin.button);
        C_btnStyle = new GUIStyle(activeSkin.button);
        D_btnStyle = new GUIStyle(activeSkin.button);
        E_btnStyle = new GUIStyle(activeSkin.button);
        #endregion
    }

    void OnGUI()
    {
        GUI.skin = activeSkin;
        DemoTitleScreen();
    }

    private void DemoTitleScreen()
    {

        GUI.BeginGroup(logoContainerRect);
        
        GUI.DrawTexture(logoRect, logoTexture);
        GUI.Label(demoTextRect,"demo");

        GUI.EndGroup();

        GUI.BeginGroup(navContainerRect);

        // Tutorial Stage
        if (GUI.Button(A_btnRect, "tutorial", A_btnStyle))
        {
            // Redirect to Tutorial Stage
            Application.LoadLevel("[DEMO] Tutorial");
        }

        // Settings Button
        if (GUI.Button(B_btnRect, "stage 1", B_btnStyle))
        {
            // Redirect to Stage 1
            Application.LoadLevel("[DEMO] Stage 1");
        }

        // Settings Button
        if (GUI.Button(C_btnRect, "stage 2", C_btnStyle))
        {
            // Redirect to Stage 2
            Application.LoadLevel("[DEMO] Stage 2");
        }

        // Credits Button
        if (GUI.Button(D_btnRect, "feedback", D_btnStyle))
        {
            // Open Contact us modal
            Application.LoadLevel("[DEMO] Feedback");
        }

        // Credits Button
        if (GUI.Button(E_btnRect, "contact us!", E_btnStyle))
        {
            // Open Contact us modal
            Application.LoadLevel("[DEMO] Contact");
        }

        A_btnScale.OnMouseOver(A_btnRect);
        B_btnScale.OnMouseOver(B_btnRect);
        C_btnScale.OnMouseOver(C_btnRect);
        D_btnScale.OnMouseOver(D_btnRect);
        E_btnScale.OnMouseOver(E_btnRect);

        GUI.EndGroup();

    }

    void OnDestroy()
    {
        // Log the event
        logService.SetEvent("[DEMO] Title Screen", "Escaped", logCallBack);
    }

    //applies the values from iTween:
    void A_ScaleButton(Rect size)
    {
        A_btnRect = size;
        // Font Scaling
        A_btnStyle.fontSize = (int)(A_btnRect.height * navFontScaling);
        // Padding Scaling
        A_btnStyle.padding.top = (int)(A_btnStyle.fontSize * navTopPaddingScaling);
        A_btnStyle.padding.bottom = (int)(A_btnStyle.fontSize * navBottomPaddingScaling);

    }

    //applies the values from iTween:
    void B_ScaleButton(Rect size)
    {
        B_btnRect = size;
        // Font Scaling
        B_btnStyle.fontSize = (int)(B_btnRect.height * navFontScaling);
        // Padding Scaling
        B_btnStyle.padding.top = (int)(B_btnStyle.fontSize * navTopPaddingScaling);
        B_btnStyle.padding.bottom = (int)(B_btnStyle.fontSize * navBottomPaddingScaling);
    }

    //applies the values from iTween:
    void C_ScaleButton(Rect size)
    {
        C_btnRect = size;
        // Font Scaling
        C_btnStyle.fontSize = (int)(C_btnRect.height * navFontScaling);
        // Padding Scaling
        C_btnStyle.padding.top = (int)(C_btnStyle.fontSize * navTopPaddingScaling);
        C_btnStyle.padding.bottom = (int)(C_btnStyle.fontSize * navBottomPaddingScaling);
    }

    //applies the values from iTween:
    void D_ScaleButton(Rect size)
    {
        D_btnRect = size;
        // Font Scaling
        D_btnStyle.fontSize = (int)(D_btnRect.height * navFontScaling);
        // Padding Scaling
        D_btnStyle.padding.top = (int)(D_btnStyle.fontSize * navTopPaddingScaling);
        D_btnStyle.padding.bottom = (int)(D_btnStyle.fontSize * navBottomPaddingScaling);
    }

    //applies the values from iTween:
    void E_ScaleButton(Rect size)
    {
        E_btnRect = size;
        // Font Scaling
        E_btnStyle.fontSize = (int)(E_btnRect.height * navFontScaling);
        // Padding Scaling
        E_btnStyle.padding.top = (int)(E_btnStyle.fontSize * navTopPaddingScaling);
        E_btnStyle.padding.bottom = (int)(E_btnStyle.fontSize * navBottomPaddingScaling);
    }
}