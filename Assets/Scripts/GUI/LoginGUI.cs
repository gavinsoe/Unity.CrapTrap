using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;

public class LoginGUI : MonoBehaviour {

    private string username = "";
    private string password = "";
    private string email = "";

    public GUIStyle usernameStyle;
    public GUIStyle passwordStyle;

    // App42 Stuff
    ServiceAPI serviceAPI;
    UserService userService;
    Constants APIConstants = new Constants();
    UserResponse userResponse = new UserResponse();

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
        serviceAPI = new ServiceAPI(APIConstants.apiKey, APIConstants.secretKey);

        // Build User Service
        userService = serviceAPI.BuildUserService();
    }

    void OnGUI()
    {
        LoginForm();

        //username = GUI.TextField(new Rect(10, 10, 200, 20), username, 25);
        //password = GUI.PasswordField(new Rect(10, 40, 200, 20), password, '*', 25, passwordStyle);
    }

    void LoginForm()
    {
        var width = Screen.width / 2;
        GUILayout.BeginVertical();

        GUILayout.TextArea(userResponse.getResult(), GUILayout.Width(width), GUILayout.Height(175));

        // Username
        //GUILayout.BeginHorizontal();
        GUILayout.Label("Username", GUILayout.Width(width));
        username = GUILayout.TextField(username, 25, GUILayout.Width(width));
        //GUILayout.EndHorizontal();
        
        // Password
        //GUILayout.BeginHorizontal();
        GUILayout.Label("Password", GUILayout.Width(width));
        password = GUILayout.PasswordField(password, '*', 25, GUILayout.Width(width));
        //GUILayout.EndHorizontal();


        GUILayout.Label("Email", GUILayout.Width(width));
        email = GUILayout.TextField(email, 25, GUILayout.Width(width));
        
        // Submit
        if (GUILayout.Button("Submit"))
        {
            // Do something...
            userService.Authenticate(username, password,userResponse);
        }

        // Register
        if (GUILayout.Button("Register!"))
        {
            // Do something...
            IList<String> roleList = new List<String>();
            roleList.Add("Tester");

            userService.CreateUser(username, password, email,roleList, userResponse);
        }

        GUILayout.EndVertical();

    }
}

// Callback
public class UserResponse : App42CallBack
{
    private string result = "";

    public void OnSuccess(object response)
    {
        try
        {
            if (response is User)
            {
                User userObj = (User)response;
                result = response.ToString();
            }
            else
            {
                result = response.ToString();
            }

        }
        catch (App42Exception e)
        {
            result = e.ToString();

        }
    }

    public void OnException(Exception e)
    {
        result = e.ToString();
    }

    public string getResult()
    {
        return result;
    }

}