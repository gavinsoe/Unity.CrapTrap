using UnityEngine;
using System.Collections;

public class Constants {
    // App42
    public string apiKey = "13a8d1c04a563ab7a0bd7754a94fd9bb52424941af56825a9503c37ab1a07b43";
    public string secretKey = "183c6d27697ae750895610fb1569d580b52f44b0d4e06b10735232ae46a6e8d0";

    // App42 main database
    public string dbName = "CRAPTRAP";
    public string collectionReviews = "Reviews";
    public string collectionStageStats = "Stage Stats";

    // Emails
    public string senderEmail = "admin@articonnect.com";
    public string contactEmail = "contact@articonnect.com";

    // Validation
    public string regexEmail = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" 
                              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" 
                              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    // Logging suffixes
    public string logStageComplete = " Complete";
    public string logStageFailed = " Failed";
    public string logStageRetry = " Retry";
    public string logStageQuit = " Quit";

    // IOS App ID and Signature
    public string appId_IOS = "";
    public string appSignature_IOS = "";

    // Android App ID and Signature
    public string appId_Android = "";
    public string appSignature_Android = "";
}
