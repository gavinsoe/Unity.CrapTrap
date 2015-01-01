using UnityEngine;
using System.Collections;

public static class Constants {
    // App42
    public static string apiKey = "13a8d1c04a563ab7a0bd7754a94fd9bb52424941af56825a9503c37ab1a07b43";
    public static string secretKey = "183c6d27697ae750895610fb1569d580b52f44b0d4e06b10735232ae46a6e8d0";

    // App42 main database
    public static string dbName = "CRAPTRAP";
    public static string collectionReviews = "Reviews";
    public static string collectionStageStats = "Stage Stats";

    // Emails
    public static string senderEmail = "admin@articonnect.com";
    public static string contactEmail = "contact@articonnect.com";

    // Validation
    public static string regexEmail = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" 
                              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" 
                              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    // Logging suffixes
    public static string logStageComplete = " Complete";
    public static string logStageFailed = " Failed";
    public static string logStageRetry = " Retry";
    public static string logStageQuit = " Quit";

    #region Ads

    public static string adBuddizPublisherKeyAndroid = "7c87c2e1-0341-4e0b-850e-016c228b11f6";
    public static string adBuddizPublisherKeyiOS = "";


    #endregion

    // IOS App ID and Signature
    public static string appId_IOS = "";
    public static string appSignature_IOS = "";

    // Android App ID and Signature
    public static string appId_Android = "";
    public static string appSignature_Android = "";
}
