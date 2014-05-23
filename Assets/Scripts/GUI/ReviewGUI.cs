using UnityEngine;
using System;
using System.Collections;
using System.Net;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.review;

public class ReviewGUI : MonoBehaviour
{
    private string username = "";//Guid.NewGuid().ToString();
    
    private string itemId = "CTvDEMO";

    // App42 Stuff
    ServiceAPI serviceAPI;
    ReviewService reviewService;
    Constants constants = new Constants();
    ReviewResponse callBack = new ReviewResponse();

    // GUI Skin
    public GUISkin activeSkin;

    // GUI variables
    private Rect formRect;

    private string error = String.Empty;
    private string feedback_1 = String.Empty;
    private string feedback_2 = String.Empty;
    private string feedback_3 = String.Empty;
    private string feedback_4 = String.Empty;
    private int toolbarInt = 0;
    private string[] toolbarStrings = new string[] { "1", "2", "3", "4", "5" };
    private Vector2 scrollPosition = new Vector2(0, 0);

    private float labelFontScaling = 0.04f;
    private float headerFontScaling = 0.075f;
    private float buttonFontScaling = 0.04f;
    public float buttonVertPadding = 0.8f;
    public float buttonHorPadding = 1.8f;
    private float contentFontScaling = 0.05f;
    private float radioButtonFontScaling = 0.06f;
    private float radioButtonSize = 2.5f;

    #if UNITY_EDITOR
        public static bool Validator(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        { return true; }
    #endif
    void Start()
    {
        #if UNITY_EDITOR
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        #endif

        // Connect to the app service
        serviceAPI = new ServiceAPI(constants.apiKey, constants.secretKey);

        // Build Review Service
        reviewService = serviceAPI.BuildReviewService();

        // Get the device number
        username = SystemInfo.deviceUniqueIdentifier;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                     scrollPosition.y += touch.deltaPosition.y;        // dragging
            }
        }
    }

    void OnGUI()
    {
        #region GUI stuff
        var formOffset_X = Screen.width * 0.1f;
        var formOffset_Y = Screen.height * 0.05f;
        var formWidth = Screen.width * 0.8f;
        var formHeight = Screen.height * 0.9f;
        formRect = new Rect(formOffset_X, formOffset_Y, formWidth, formHeight);

        activeSkin.label.fontSize = (int)(Screen.height * labelFontScaling);
        activeSkin.textField.fontSize = (int)(Screen.height * contentFontScaling);
        activeSkin.textArea.fontSize = (int)(Screen.height * contentFontScaling);
        activeSkin.button.fontSize = (int)(Screen.height * buttonFontScaling);
        
        RectOffset btnPadding = new RectOffset((int)(activeSkin.button.fontSize*buttonHorPadding),
                                               (int)(activeSkin.button.fontSize*buttonHorPadding),
                                               (int)(activeSkin.button.fontSize*buttonVertPadding),
                                               (int)(activeSkin.button.fontSize*buttonVertPadding));
        activeSkin.button.padding = btnPadding;
        // header font scaling
        activeSkin.customStyles[0].fontSize = (int)(Screen.height * headerFontScaling);
        activeSkin.customStyles[1].fontSize = (int)(Screen.height * labelFontScaling);
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * radioButtonFontScaling);
        var dimension = (int)(activeSkin.customStyles[2].fontSize * radioButtonSize);
        activeSkin.customStyles[2].fixedHeight = dimension;
        activeSkin.customStyles[2].fixedWidth = dimension;
        activeSkin.customStyles[2].margin.right = (int)((float)dimension*0.05);

        #endregion

        GUI.skin = activeSkin;
        DemoReviewForm();
    }

    void DemoReviewForm()
    {

        GUILayout.BeginArea(formRect);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        
        GUILayout.Label("Thank you for trying out our game! \nLet us know what you think!", activeSkin.customStyles[0]);

        if (!String.IsNullOrEmpty(error))
        {
            if (!String.IsNullOrEmpty(callBack.result))
            {
                error = callBack.result;
                callBack.result = String.Empty;
            }
            GUILayout.Label(error, activeSkin.customStyles[1]);
        }
        GUILayout.Label("What did you LIKE about the game?");
        feedback_1 = GUILayout.TextArea(feedback_1);
        
        GUILayout.Label("What did you HATE about the game?");
        feedback_2 = GUILayout.TextArea(feedback_2);

        GUILayout.Label("Help us make the game better!");
        feedback_3 = GUILayout.TextArea(feedback_3);
        
        GUILayout.Label("Any additional comments? :D");
        feedback_4 = GUILayout.TextArea(feedback_4);

        GUILayout.Label("Rate the game!");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, activeSkin.customStyles[2]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("main menu"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_TitleScreen");
        }

        GUILayout.FlexibleSpace();

        // Submit
        if (GUILayout.Button("submit"))
        {
            // Clear Error message
            error = String.Empty;

            #region Validate fields
            // Check if any field is empty
            if (String.IsNullOrEmpty(feedback_1) ||
                String.IsNullOrEmpty(feedback_2) ||
                String.IsNullOrEmpty(feedback_3) ||
                String.IsNullOrEmpty(feedback_4))
            {
                if (!String.IsNullOrEmpty(error)) error += "\n";
                error += "Please fill in all fields :)";
            }
            #endregion
            #region Format and send
            if (String.IsNullOrEmpty(error))
            {
                // Parse together the feedback
                string feedback = "[ LIKES ] " + feedback_1 + " " +
                                  "[ HATES ] " + feedback_2 + " " +
                                  "[ TOADD ] " + feedback_3 + " " +
                                  "[ OTHER ] " + feedback_4;
                            
                // Do something...
                reviewService.CreateReview(username, itemId, feedback, toolbarInt + 1, callBack);
                
                error = "Sending...";
            }
            #endregion
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}

// Callback
public class ReviewResponse : App42CallBack
{
    public string result { get; set; }

    public void OnSuccess(object response)
    {
        try
        {
            result = "Thank you for making our game better! :D";
        }
        catch (App42Exception e)
        {
            result = e.ToString();
        }
    }

    public void OnException(Exception e)
    {
        result = "Unfortunately something weird happened :( \nFeel free to email us your thoughts at contact@articonnect.com if it continues to not work!";
    }
}
