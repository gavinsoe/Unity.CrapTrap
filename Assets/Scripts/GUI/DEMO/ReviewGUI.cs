using UnityEngine;
using System;
using System.Collections;
using System.Net;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.log;

public class ReviewGUI : MonoBehaviour
{
    private string deviceId = "";//Guid.NewGuid().ToString();
    
    private string itemId = "CTvDEMO";

    // App42 Stuff
    ServiceAPI serviceAPI;
    LogService logService;
    StorageService storageService;
    LogResponse logCallBack = new LogResponse();
    ReviewResponse callBack = new ReviewResponse();

    // GUI Skin
    public GUISkin activeSkin;

    // GUI variables
    private Rect formRect;

    private string error = String.Empty;
    private string header = "Let us know what you think!";
    private string question_1 = "What did you LIKE about the game?";
    private string question_2 = "What did you HATE about the game?";
    private string question_3 = "Help us make the game better!";
    private string question_4 = "Any additional comments? :D";
    private string label_rate = "Rate the demo!";
    private string feedback_1 = String.Empty;
    private string feedback_2 = String.Empty;
    private string feedback_3 = String.Empty;
    private string feedback_4 = String.Empty;
    private int toolbarInt = 2;
    private string[] toolbarStrings = new string[] { "1", "2", "3", "4", "5" };
    private Vector2 scrollPosition = new Vector2(0, 0);
    private bool newAlert = false;

    #region GUI Styling

    // Container and inner frame
    private Rect containerRect;
    private Rect innerFrameRect;
    private float containerWidth = 0.75f; // As a percentage of whole screen
    private float containerHeight = 0.9f; // As a percentage of whole screen
    private float innerFrameWidth = 0.725f; // As a percentage of the whole screen
    private float innerFrameHeight; // Stores the actual height of the inner container

    // Headers
    private GUIStyle headerStyle;
    public float headerFontScaling = 0.075f;
    private Rect headerRect;

    // Scaling for text
    private float fontHeightScale = 1.2f; // The height of each line based on the font size
    // Normal and alert messages
    private GUIStyle labelStyle;
    private GUIStyle alertMsgStyle;
    public float labelFontScaling = 0.06f;
    private float labelHorPadding = 0.01f;
    private float labelHeight;

    private Rect label_Feedback1Rect;
    private Rect label_Feedback2Rect;
    private Rect label_Feedback3Rect;
    private Rect label_Feedback4Rect;
    private Rect label_RateRect;
    private Rect label_AlertRect;

    // TextArea
    private GUIStyle txtAreaStyle;
    private float txtAreaFontScaling = 0.05f;
    private float txtAreaTopPadding = 0.5f;
    private float txtAreaBtmPadding = 0.5f;
    private float txtAreaHeight;

    private Rect txtArea_Feedback1;
    private Rect txtArea_Feedback2;
    private Rect txtArea_Feedback3;
    private Rect txtArea_Feedback4;

    // radion/toolbar buttons
    private GUIStyle radioBtnStyle;
    private Rect radioBtnRect;
    private float radioButtonFontScaling = 0.06f;
    private float radioButtonSize = 2.5f;

    // buttons
    private float buttonFontScaling = 0.5f;
    private float buttonTopPadding = 0.39f;

    /* ╔══════════════╗   ╔══════════════╗
     * ║   Button A   ║   ║   Button B   ║
     * ╚══════════════╝   ╚══════════════╝
     */

    public float btnWidth;
    public float btnHeight;

    public Rect A_btnRect;
    public Rect B_btnRect;

    private ButtonHandler A_btnScale;
    private ButtonHandler B_btnScale;

    private GUIStyle A_btnStyle;
    private GUIStyle B_btnStyle;

    #endregion

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
        serviceAPI = new ServiceAPI(Constants.apiKey, Constants.secretKey);

        // Build the log service
        logService = serviceAPI.BuildLogService();

        // Build the storage service
        storageService = serviceAPI.BuildStorageService();

        // Get the device number
        deviceId = SystemInfo.deviceUniqueIdentifier;

        // Log the event
        logService.SetEvent("[DEMO] Feedback", "Landed", logCallBack);

        #region GUI Styling

        containerRect = new Rect(Screen.width * ((1 - containerWidth) / 2),
                                 Screen.height * ((1 - containerHeight) / 2),
                                 Screen.width * containerWidth,
                                 Screen.height * containerHeight);

        // Set the header style
        headerStyle = new GUIStyle(activeSkin.customStyles[0]);
        headerStyle.fontSize = (int)(Screen.height * headerFontScaling);
        headerStyle.padding.left = (int)(containerRect.width * labelHorPadding);
        headerStyle.padding.right = (int)(containerRect.width * labelHorPadding);

        // Set the label style
        labelStyle = activeSkin.label;
        labelStyle.fontSize = (int)(Screen.height * labelFontScaling);
        labelStyle.padding.left = (int)(containerRect.width * labelHorPadding);
        labelStyle.padding.right = (int)(containerRect.width * labelHorPadding);

        // set the error msg style
        alertMsgStyle = new GUIStyle(activeSkin.customStyles[1]);
        alertMsgStyle.fontSize = labelStyle.fontSize;
        alertMsgStyle.padding.left = (int)(containerRect.width * labelHorPadding);
        alertMsgStyle.padding.right = (int)(containerRect.width * labelHorPadding);

        // Set the textArea style
        txtAreaStyle = activeSkin.textArea;
        txtAreaStyle.fontSize = (int)(Screen.height * txtAreaFontScaling);
        txtAreaStyle.padding.top = (int)(txtAreaStyle.fontSize * txtAreaTopPadding);
        txtAreaStyle.padding.bottom = (int)(txtAreaStyle.fontSize * txtAreaBtmPadding);

        // Set the radio button styles
        radioBtnStyle = new GUIStyle(activeSkin.customStyles[2]);
        radioBtnStyle.fontSize = (int)(Screen.height * radioButtonFontScaling);
        var dimension = (int)(radioBtnStyle.fontSize * radioButtonSize);
        radioBtnStyle.fixedHeight = dimension;
        radioBtnStyle.fixedWidth = dimension;
        radioBtnStyle.margin.right = (int)((float)dimension * 0.25f);
        radioBtnStyle.margin.left = (int)((float)dimension * 0.25f);

        // Set the button styles
        btnWidth = Screen.width * 0.3f;
        btnHeight = btnWidth * ((float)activeSkin.button.normal.background.height /
                                (float)activeSkin.button.normal.background.width);

        // Font scaling
        activeSkin.button.fontSize = (int)(btnHeight * buttonFontScaling);
        // Padding Scaling
        activeSkin.button.padding.top = (int)(activeSkin.button.fontSize * buttonTopPadding);

        A_btnStyle = new GUIStyle(activeSkin.button);
        B_btnStyle = new GUIStyle(activeSkin.button);


        #region positioning

        // initialise variable to calculate total height
        float totalHeight = 0;
        float frameWidth = Screen.width * innerFrameWidth;

        // header label
        labelHeight = headerStyle.CalcHeight(new GUIContent(header), frameWidth);
        headerRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // feedback 1 label
        labelHeight = labelStyle.CalcHeight(new GUIContent(feedback_1), frameWidth);
        label_Feedback1Rect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // feedback 1 text area
        txtAreaHeight = (txtAreaStyle.fontSize * fontHeightScale) * 3 + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_Feedback1 = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // feedback 2 label
        labelHeight = labelStyle.CalcHeight(new GUIContent(feedback_2), frameWidth);
        label_Feedback2Rect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // feedback 2 text area
        txtAreaHeight = (txtAreaStyle.fontSize * fontHeightScale) * 3 + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_Feedback2 = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // feedback 3 label
        labelHeight = labelStyle.CalcHeight(new GUIContent(feedback_3), frameWidth);
        label_Feedback3Rect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // feedback 3 text area
        txtAreaHeight = (txtAreaStyle.fontSize * fontHeightScale) * 3 + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_Feedback3 = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // feedback 4 label
        labelHeight = labelStyle.CalcHeight(new GUIContent(feedback_4), frameWidth);
        label_Feedback4Rect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // feedback 4 text area
        txtAreaHeight = (txtAreaStyle.fontSize * fontHeightScale) * 3 + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_Feedback4 = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // feedback 5 label
        labelHeight = labelStyle.CalcHeight(new GUIContent(label_rate), frameWidth);
        label_RateRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // radio button
        radioBtnRect = new Rect(0, totalHeight, frameWidth, dimension);
        totalHeight += dimension;

        // main menu and submit buttons
        A_btnRect = new Rect(0, totalHeight, btnWidth, btnHeight);
        B_btnRect = new Rect(frameWidth - btnWidth, totalHeight, btnWidth, btnHeight);
        totalHeight += btnHeight;

        // Initialise button scalers
        A_btnScale = new ButtonHandler(A_btnRect, gameObject, 0.9f, "A_ScaleButton");
        B_btnScale = new ButtonHandler(B_btnRect, gameObject, 0.9f, "B_ScaleButton");

        #endregion

        innerFrameRect = new Rect(0, 0, frameWidth, totalHeight);
        innerFrameHeight = totalHeight;

        #endregion
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                scrollPosition.y += (touch.deltaPosition.y * 3);        // dragging
            }
        }

        #region Alert Message
        if (newAlert)
        {
            // Scroll to very bottom
            scrollPosition.y = Mathf.Infinity;
            // set flag to false
            newAlert = false;
        }

        if (!String.IsNullOrEmpty(callBack.result))
        {
            error = callBack.result;
            callBack.result = String.Empty;
            // Scroll to very bottom
            scrollPosition.y = Mathf.Infinity;
        }
        if (!String.IsNullOrEmpty(error))
        {
            labelHeight = alertMsgStyle.CalcHeight(new GUIContent(error), innerFrameRect.width);
            label_AlertRect = new Rect(0, innerFrameHeight, innerFrameRect.width, labelHeight);
            innerFrameRect = new Rect(0, 0, innerFrameRect.width, innerFrameHeight + labelHeight);
        }
        else
        {
            innerFrameRect = new Rect(0, 0, innerFrameRect.width, innerFrameHeight);
        }
        #endregion
    }

    void OnGUI()
    {
        GUI.skin = activeSkin;

        scrollPosition = GUI.BeginScrollView(containerRect, scrollPosition, innerFrameRect);

        if (!String.IsNullOrEmpty(error))
        {
            GUI.Label(label_AlertRect, error, alertMsgStyle);
        }

        GUI.Label(headerRect, header, headerStyle);

        GUI.Label(label_Feedback1Rect, question_1, labelStyle);
        feedback_1 = GUI.TextArea(txtArea_Feedback1, feedback_1);

        GUI.Label(label_Feedback2Rect, question_2, labelStyle);
        feedback_2 = GUI.TextArea(txtArea_Feedback2, feedback_2);

        GUI.Label(label_Feedback3Rect, question_3, labelStyle);
        feedback_3 = GUI.TextArea(txtArea_Feedback3, feedback_3);

        GUI.Label(label_Feedback4Rect, question_4, labelStyle);
        feedback_4 = GUI.TextArea(txtArea_Feedback4, feedback_4);

        GUI.Label(label_RateRect, label_rate, labelStyle);
        GUILayout.BeginArea(radioBtnRect);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, radioBtnStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        //toolbarInt = GUI.Toolbar(radioBtnRect,toolbarInt, toolbarStrings, radioBtnStyle);

        if (GUI.Button(A_btnRect,"main menu", A_btnStyle))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_TitleScreen");
        }

        // Submit
        if (GUI.Button(B_btnRect,"submit",B_btnStyle))
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
                newAlert = true;
            }
            #endregion
            #region Format and send
            if (String.IsNullOrEmpty(error))
            {
                // Package the review
                SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
                json.Add("Device id", deviceId);
                json.Add("Likes", feedback_1);
                json.Add("Hates", feedback_2);
                json.Add("Suggestions", feedback_3);
                json.Add("Other", feedback_4);
                json.Add("Rating", (toolbarInt + 1).ToString());

                // Do something...
                storageService.InsertJSONDocument(Constants.dbName, Constants.collectionReviews, json, callBack);

                // Log the event
                logService.SetEvent("[DEMO] Feedback Submitted", logCallBack);

                error = "Sending...";
                newAlert = true;
            }
            #endregion
        }

        A_btnScale.OnMouseOver(A_btnRect);
        B_btnScale.OnMouseOver(B_btnRect);
        GUI.EndScrollView();
        //DemoReviewForm();
    }

    void OnDestroy()
    {
        // Log the event
        logService.SetEvent("[DEMO] Feedback", "Escaped", logCallBack);
    }

    //applies the values from iTween:
    void A_ScaleButton(Rect size)
    {
        A_btnRect = size;
        // Font Scaling
        A_btnStyle.fontSize = (int)(A_btnRect.height * buttonFontScaling);
        // Padding Scaling
        A_btnStyle.padding.top = (int)(A_btnStyle.fontSize * buttonTopPadding);
    }

    //applies the values from iTween:
    void B_ScaleButton(Rect size)
    {
        B_btnRect = size;
        // Font Scaling
        B_btnStyle.fontSize = (int)(B_btnRect.height * buttonFontScaling);
        // Padding Scaling
        B_btnStyle.padding.top = (int)(B_btnStyle.fontSize * buttonTopPadding);
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
}

