using UnityEngine;
using System.Collections;

public class Constants {
    // App42
    public string apiKey = "ea64f1c53a17e32313f7145f3adc28f10df79706f7fc004a1cd2c3754a855c8d";
    public string secretKey = "4a7d6066fc132c000ecaaa5b08c377e43075d858e9832fe71c9fae9f8fcf1795";

    // App42 main database
    public string dbName = "craptrap";
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
