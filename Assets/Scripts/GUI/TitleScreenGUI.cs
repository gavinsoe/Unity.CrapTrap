using UnityEngine;
using System;
using System.Collections;

public class TitleScreenGUI : MonoBehaviour {

    // Transparency
    private float guiAlpha = 1;

    // Prologue
    private PrologueController prologue;

    // Splash Screen
    public Texture SplashTexture;
    private bool splashActive = true;
    private Rect splashRect;
    private float splashDuration = 2;
    
    // GUI Skin
    public GUISkin activeSkin;

    // Page variables
    private Rect bgRect;
    private Texture bgTexture;
    private float screenPaddingPercentage = 0.01f; // percentage of screen size;

    // The game logo
    private Rect logoRect;
    private Texture logoTexture;

    // Buttons
    private Rect btnCreditsRect;
    private Rect btnSoundRect;
    private Rect btnStartRect;
    private Rect btnTwitterRect;
    private Rect btnFacebookRect;

    private ButtonHandler btnCreditsScale;
    private ButtonHandler btnSoundScale;
    private ButtonHandler btnStartScale;
    private ButtonHandler btnFacebookScale;
    private ButtonHandler btnTwitterScale;

    private float btnFontScaling = 0.5f;
    private float btnTopPaddingScaling = 0.39f;
    private float btnBottomPaddingScaling = 0f;

    private float btnStartSize = 0.25f; // Percentage of screen height
    private float btnStartYOffset = 0.435f;

    // Artic symbol
    private Texture articSymbolTexture;
    private Rect articSymbolRect;

    // The Boy
    private Texture boyTexture;
    private Rect boyRect;
    private float boySize = 0.72f;
    private float boyXOffset = 0.12f;
    private float boyYOffset = 0.435f;

    // Version
    private Rect versionRect;
    private float versionFontScale = 0.04f;
    private string version = "v0.0.0.1";

    // Loading
    private Rect loadingRect;
    private GUIStyle loadingStyle;
    private float loadingFontScale = 0.04f;
    private string loading = "[loading]";

    void Awake()
    {
        prologue = GetComponent<PrologueController>();
    }

    void Start()
    {
        #region GUI

        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[1].normal.background;

        // Splash Rect
        splashRect = new Rect(0, 0, Screen.width, Screen.height);

        // Calculate screen padding
        float screenPadding = Screen.height * screenPaddingPercentage;

        // Logo
        logoTexture = activeSkin.customStyles[0].normal.background;
        float logoHeight = Screen.height * 0.6f;
        float logoWidth = logoHeight * ((float)logoTexture.width / (float)logoTexture.height);
        float logoXOffset = (Screen.width - logoWidth) * 0.5f;
        float logoYOffset = -logoHeight * 0.15f;
        logoRect = new Rect(logoXOffset, logoYOffset, logoWidth, logoHeight);

        // Buttons
        float btnHeight = Screen.height * 0.1f;
        float btnSettingsWidth = btnHeight * ((float)activeSkin.button.normal.background.width /
                                              (float)activeSkin.button.normal.background.height);
        float btnSoundWidth = btnHeight * ((float)activeSkin.customStyles[2].normal.background.width /
                                           (float)activeSkin.customStyles[2].normal.background.height);

        btnCreditsRect = new Rect(Screen.width - btnSettingsWidth - screenPadding, screenPadding, btnSettingsWidth, btnHeight);
        btnSoundRect = new Rect(Screen.width - btnSoundWidth - screenPadding, btnHeight + screenPadding, btnSoundWidth, btnHeight);

        // Button font auto scaling
        activeSkin.button.fontSize = (int)(btnHeight * btnFontScaling);
        // Button padding Scaling
        activeSkin.button.padding.top = (int)(activeSkin.button.fontSize * btnTopPaddingScaling);
        activeSkin.button.padding.bottom = (int)(activeSkin.button.fontSize * btnBottomPaddingScaling);

        // Artic Symbol
        articSymbolTexture = activeSkin.customStyles[3].normal.background;
        float symbolWidth = btnHeight * ((float)articSymbolTexture.width / (float)articSymbolTexture.height);
        articSymbolRect = new Rect(screenPadding, screenPadding, symbolWidth, btnHeight);

        // Start button
        float btnStartHeight = Screen.height * btnStartSize;
        float btnStartWidth = btnStartHeight * ((float)activeSkin.customStyles[4].normal.background.width /
                                               (float)activeSkin.customStyles[4].normal.background.height);
        btnStartRect = new Rect((Screen.width - btnStartWidth) * 0.5f, Screen.height * btnStartYOffset, btnStartWidth, btnStartHeight);

        // Facebook and twitter
        btnFacebookRect = new Rect(Screen.width - btnHeight - screenPadding,
                                   Screen.height - btnHeight - screenPadding,
                                   btnHeight, btnHeight);
        btnTwitterRect = new Rect(Screen.width - btnHeight - screenPadding,
                                  Screen.height - btnHeight - screenPadding - btnFacebookRect.height,
                                  btnHeight, btnHeight);

        // Initialise button scalers
        btnCreditsScale = new ButtonHandler(btnCreditsRect, gameObject, 0.9f, "Credits_ScaleButton");
        btnSoundScale = new ButtonHandler(btnSoundRect, gameObject, 0.9f, "Sound_ScaleButton");
        btnStartScale = new ButtonHandler(btnStartRect, gameObject, 0.9f, "Start_ScaleButton");
        btnFacebookScale = new ButtonHandler(btnFacebookRect, gameObject, 0.9f, "Facebook_ScaleButton");
        btnTwitterScale = new ButtonHandler(btnTwitterRect, gameObject, 0.9f, "Twitter_ScaleButton");

        // The boy
        boyTexture = activeSkin.customStyles[5].normal.background;
        float boyHeight = Screen.height * boySize;
        float boyWidth = boyHeight * ((float)boyTexture.width /
                                      (float)boyTexture.height);
        boyRect = new Rect(Screen.width * boyXOffset, Screen.height * boyYOffset, boyWidth, boyHeight);

        // Version
        activeSkin.label.fontSize = (int)(Screen.height * versionFontScale);
        versionRect = new Rect(4 * screenPadding,
                               Screen.height - activeSkin.label.fontSize - screenPadding * 2,
                               Screen.width * 0.4f,
                               activeSkin.label.fontSize);

        loadingStyle = activeSkin.customStyles[8];
        loadingStyle.fontSize = (int)(Screen.height * loadingFontScale);
        loadingRect = new Rect(0,
                               Screen.height - loadingStyle.fontSize - screenPadding * 2,
                               Screen.width,
                               activeSkin.label.fontSize);

        #endregion
    }

    void Update()
    {
        splashDuration -= Time.deltaTime;
        if (splashDuration < 0)
        {
            GetComponent<PrologueController>().enabled = true;
        }
    }

    void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;

        GUI.skin = activeSkin;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);
        
        if (splashActive)
        {
            GUI.DrawTexture(splashRect, SplashTexture,ScaleMode.ScaleAndCrop);
        }
        else
        {
            TitleScreen();
        }
    }

    private void TitleScreen()
    {
        // Background
        GUI.DrawTexture(bgRect, bgTexture,ScaleMode.ScaleAndCrop);

        // Logo
        GUI.DrawTexture(logoRect, logoTexture);

        // Symbol
        GUI.DrawTexture(articSymbolRect, articSymbolTexture);

        // Boy
        GUI.DrawTexture(boyRect, boyTexture);

        // Version
        GUI.Label(versionRect, version);

        // Version
        GUI.Label(loadingRect, loading, loadingStyle);

        // credits button
        if (GUI.Button(btnCreditsRect, "credits"))
        {
            // Redirect to credits page
        }

        // sound button
        Game.instance.audio = GUI.Toggle(btnSoundRect, Game.instance.audio, "", activeSkin.customStyles[2]);
        MainGameController.instance.ToggleSound(Game.instance.audio);

        // start button
        if (GUI.Button(btnStartRect, "", activeSkin.customStyles[4]))
        {
            NavigationManager.instance.NavToChapterSelect();
            // do something
        }

        // twitter button
        if (GUI.Button(btnTwitterRect, "", activeSkin.customStyles[6]))
        {
        }

        // facebook button
        if (GUI.Button(btnFacebookRect, "", activeSkin.customStyles[7]))
        {
        }

        btnCreditsScale.OnMouseOver(btnCreditsRect);
        btnSoundScale.OnMouseOver(btnSoundRect);
        btnStartScale.OnMouseOver(btnStartRect);
        btnTwitterScale.OnMouseOver(btnTwitterRect);
        btnFacebookScale.OnMouseOver(btnFacebookRect);
    }

    public void Show()
    {
        iTween.ValueTo(gameObject,
            iTween.Hash("from", guiAlpha,
                        "to", 1,
                        "onupdate", "AnimateTransparency",
                        "easetype", iTween.EaseType.linear,
                        "time", 0.5f));
        this.enabled = true;
    }

    public void Hide()
    {
        iTween.ValueTo(gameObject,
            iTween.Hash("from", guiAlpha,
                        "to", 0,
                        "onupdate", "AnimateTransparency",
                        "oncomplete", "HideSplash",
                        "easetype", iTween.EaseType.linear,
                        "time", 0.5f));
    }

    // applies values from iTween
    void Credits_ScaleButton(Rect size)
    {
        btnCreditsRect = size;
        // Button font auto scaling
        activeSkin.button.fontSize = (int)(size.height * btnFontScaling);
        // Button padding Scaling
        activeSkin.button.padding.top = (int)(activeSkin.button.fontSize * btnTopPaddingScaling);
        activeSkin.button.padding.bottom = (int)(activeSkin.button.fontSize * btnBottomPaddingScaling);
    }

    // applies values from iTween
    void Sound_ScaleButton(Rect size)
    {
        btnSoundRect = size;
    }

    // applies values from iTween
    void Start_ScaleButton(Rect size)
    {
        btnStartRect = size;
    }

    // applies values from iTween
    void Twitter_ScaleButton(Rect size)
    {
        btnTwitterRect = size;
    }

    // applies values from iTween
    void Facebook_ScaleButton(Rect size)
    {
        btnFacebookRect = size;
    }

    void AnimateTransparency(float alpha)
    {
        guiAlpha = alpha;
    }

    void HideSplash()
    {
        splashActive = false;
        this.enabled = false;
    }
}

