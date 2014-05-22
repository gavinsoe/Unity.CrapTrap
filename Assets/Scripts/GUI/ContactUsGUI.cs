using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.email;

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

    // App42 Stuff
    ServiceAPI serviceAPI;
    EmailService emailService;
    Constants constants = new Constants();
    ContactUsResponse callBack = new ContactUsResponse();

    private float labelFontScaling = 0.04f;
    private float headerFontScaling = 0.075f;
    private float buttonFontScaling = 0.04f;
    private float contentFontScaling = 0.05f;

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

        // Build Email Service
        emailService = serviceAPI.BuildEmailService();
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

	// Update is called once per frame
    void OnGUI()
    {
        #region GUI stuff

        activeSkin.label.fontSize = (int)(Screen.height * labelFontScaling);
        activeSkin.textField.fontSize = (int)(Screen.height * contentFontScaling);
        activeSkin.textArea.fontSize = (int)(Screen.height * contentFontScaling);
        activeSkin.button.fontSize = (int)(Screen.height * buttonFontScaling);
        // header font scaling
        activeSkin.customStyles[0].fontSize = (int)(Screen.height * headerFontScaling);
        activeSkin.customStyles[1].fontSize = (int)(Screen.height * labelFontScaling);

        #endregion

        GUI.skin = activeSkin;
        ContactUsForm();
	}

    void ContactUsForm()
    {
        var formOffset_X = Screen.width * 0.2f;
        var formOffset_Y = Screen.height * 0.05f;
        var formWidth = Screen.width * 0.6f;
        var formHeight = Screen.height * 0.9f;

        GUILayout.BeginArea(new Rect(formOffset_X, formOffset_Y, formWidth, formHeight));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();

        GUILayout.Label("Contact Us!", activeSkin.customStyles[0]);

        if (!String.IsNullOrEmpty(error))
        {
            if (!String.IsNullOrEmpty(callBack.result))
            {
                error = callBack.result;
                callBack.result = String.Empty;
            }
            GUILayout.Label(error, activeSkin.customStyles[1]);
        }

        var paddingOffset = activeSkin.textField.padding.left + activeSkin.textField.padding.right;
        GUILayout.Label("Name");
        senderName = GUILayout.TextField(senderName, GUILayout.MaxWidth(formWidth - paddingOffset));

        GUILayout.Label("Email");
        senderEmail = GUILayout.TextField(senderEmail, GUILayout.MaxWidth(formWidth - paddingOffset));

        GUILayout.Label("Subject");
        emailSubject = GUILayout.TextField(emailSubject, GUILayout.MaxWidth(formWidth - paddingOffset));
        emailBody = GUILayout.TextArea(emailBody, GUILayout.MinHeight(100));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Back"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_TitleScreen");
        }
        // Submit button
        if (GUILayout.Button("Submit"))
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
            }
            // Check if email is valid
            Regex regex = new Regex(constants.regexEmail, RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(senderEmail) && !regex.IsMatch(senderEmail))
            {
                if (!String.IsNullOrEmpty(error)) error += "\n";
                error += "Email is invalid!";
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
                emailService.SendMail(constants.contactEmail, formattedSubject, formattedBody, constants.senderEmail, EmailMIME.HTML_TEXT_MIME_TYPE, callBack);

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
