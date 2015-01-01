using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.email;
using com.shephertz.app42.paas.sdk.csharp.log;

public class ContactUsGUI : MonoBehaviour {

    // GUI Skin
    public GUISkin activeSkin;

    // Contact Us page variables
    private string error = String.Empty;
    private string senderName = String.Empty;
    private string senderEmail = String.Empty;
    private string emailSubject = String.Empty;
    private string emailBody = String.Empty;
    private Vector2 scrollPosition = new Vector2(0, 0);
    private bool newAlert = false;

    // App42 Stuff
    ServiceAPI serviceAPI;
    EmailService emailService;
    LogService logService;
    ContactUsResponse callBack = new ContactUsResponse();
    LogResponse logCallBack = new LogResponse();

    #region GUI styling
    // Container and inner frame
    private Rect containerRect;
    private Rect innerFrameRect;
    private float containerWidth = 0.75f; // As a percentage of whole screen
    private float containerHeight = 0.9f; // As a percentage of whole screen
    private float innerFrameWidth = 0.725f; // As a percentage of the whole screen
    private float innerFrameHeight; // Stores the actual height of the inner container

    // Headers
    private GUIStyle headerStyle;
    private float headerFontScaling = 0.075f;
    private Rect headerRect;

    // Scaling for text
    private float fontHeightScale = 1.2f; // The height of each line based on the font size
    // Normal and alert messages
    private GUIStyle labelStyle;
    private GUIStyle alertMsgStyle;
    private float labelFontScaling = 0.06f;
    private float labelHorPadding = 0.01f;
    private float labelHeight;

    private Rect label_NameRect;
    private Rect label_EmailRect;
    private Rect label_EmailSubjectRect;
    private Rect label_AlertRect;

    // TextArea
    private GUIStyle txtAreaStyle;
    private float txtAreaFontScaling = 0.05f;
    private float txtAreaTopPadding = 0.5f;
    private float txtAreaBtmPadding = 0.5f;
    private float txtAreaHeight;

    private Rect txtArea_NameRect;
    private Rect txtArea_EmailRect;
    private Rect txtArea_EmailSubjectRect;
    private Rect txtArea_EmailBodyRect;

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

        // Build Email Service
        emailService = serviceAPI.BuildEmailService();

        // Log the event
        logService.SetEvent("[DEMO] Contact", "Landed", logCallBack);
        #region GUI stuff
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

        // Calculate positions
        #region positioning

        // initialise variable to calculate total height
        float totalHeight = 0;
        float frameWidth = Screen.width * innerFrameWidth;

        // header label
        labelHeight = headerStyle.fontSize * fontHeightScale + headerStyle.padding.top + headerStyle.padding.bottom;
        headerRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // name label
        labelHeight = labelStyle.fontSize * fontHeightScale + labelStyle.padding.top + labelStyle.padding.bottom;
        label_NameRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // sender's name
        txtAreaHeight = txtAreaStyle.fontSize * fontHeightScale + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_NameRect = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // email label
        labelHeight = labelStyle.fontSize * fontHeightScale + labelStyle.padding.top + labelStyle.padding.bottom;
        label_EmailRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // sender's email
        txtAreaHeight = txtAreaStyle.fontSize * fontHeightScale + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_EmailRect = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // email subject label
        labelHeight = labelStyle.fontSize * fontHeightScale + labelStyle.padding.top + labelStyle.padding.bottom;
        label_EmailSubjectRect = new Rect(0, totalHeight, frameWidth, labelHeight);
        totalHeight += labelHeight;

        // email subject
        txtAreaHeight = txtAreaStyle.fontSize * fontHeightScale + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_EmailSubjectRect = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

        // email body
        txtAreaHeight = (txtAreaStyle.fontSize * fontHeightScale) * 5 + txtAreaStyle.padding.top + txtAreaStyle.padding.bottom;
        txtArea_EmailBodyRect = new Rect(0, totalHeight, frameWidth, txtAreaHeight);
        totalHeight += txtAreaHeight;

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

	// Update is called once per frame
    void OnGUI()
    {
        GUI.skin = activeSkin;

        scrollPosition = GUI.BeginScrollView(containerRect, scrollPosition, innerFrameRect);

        if (!String.IsNullOrEmpty(error))
        {
            GUI.Label(label_AlertRect, error, alertMsgStyle);
        }

        GUI.Label(headerRect, "Contact Us!", headerStyle);

        GUI.Label(label_NameRect, "Name", labelStyle);
        senderName = GUI.TextArea(txtArea_NameRect, senderName);

        GUI.Label(label_EmailRect, "Email", labelStyle);
        senderEmail = GUI.TextArea(txtArea_EmailRect, senderEmail);

        GUI.Label(label_EmailSubjectRect, "Subject", labelStyle);
        emailSubject = GUI.TextArea(txtArea_EmailSubjectRect, emailSubject);
        emailBody = GUI.TextArea(txtArea_EmailBodyRect, emailBody);

        if (GUI.Button(A_btnRect, "main menu", A_btnStyle))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_TitleScreen");
        }

        if (GUI.Button(B_btnRect, "submit", B_btnStyle))
        {
            // Clear Error message
            error = String.Empty;

            #region Validate fields
            // Check if any field is empty
            if (String.IsNullOrEmpty(senderName) ||
                 String.IsNullOrEmpty(senderEmail) ||
                 String.IsNullOrEmpty(emailSubject) ||
                 String.IsNullOrEmpty(emailBody))
            {
                if (!String.IsNullOrEmpty(error)) error += "\n";
                error += "Please fill in all fields :)";
                newAlert = true;
            }
            // Check if email is valid
            Regex regex = new Regex(Constants.regexEmail, RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(senderEmail) && !regex.IsMatch(senderEmail))
            {
                if (!String.IsNullOrEmpty(error)) error += "\n";
                error += "Email is invalid!";
                newAlert = true;
            }

            #endregion
            #region Format and send email
            if (String.IsNullOrEmpty(error))
            {
                // Format the message
                var formattedSubject = "[ DEMO | Contact Us ] " + senderName + " - " + emailSubject;
                var formattedBody = "<div>Name: " + senderName + "</div>" +
                                    "<div>Email: " + senderEmail + "</div>" +
                                    "<div><br></div>" +
                                    "<div><u><font size='4'>" + emailSubject + "</font></u></div>" +
                                    "<div>" + emailBody + "</div>";

                // Send email
                emailService.SendMail(Constants.contactEmail, formattedSubject, formattedBody, Constants.senderEmail, EmailMIME.HTML_TEXT_MIME_TYPE, callBack);

                // Log event
                logService.SetEvent("[DEMO] Contact Submitted", logCallBack);

                error = "Sending...";
                newAlert = true;
            }
            #endregion
        }

        A_btnScale.OnMouseOver(A_btnRect);
        B_btnScale.OnMouseOver(B_btnRect);
        GUI.EndScrollView();
	}

    void OnDestroy()
    {
        // Log the event
        logService.SetEvent("[DEMO] Contact", "Escaped", logCallBack);
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

}

public class ContactUsResponse : App42CallBack
{
    public string result { get; set; }

    public void OnSuccess(object response)
    {
        try
        {
            result = "Message succesfully sent! Thank you for contacting us! :D";
        }
        catch (App42Exception e)
        {
            result = e.ToString();
        }
    }

    public void OnException(Exception e)
    {
        result = "Unfortunately something weird happened :( \nFeel free to email us directly at contact@articonnect.com if it continues to not work!";
    }
}
