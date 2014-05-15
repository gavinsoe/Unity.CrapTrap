using UnityEngine;
using System.Collections;

public class Constants {
    // App42
    public string apiKey = "92ebb600c27cc47273d4b59ebc6fec5027d6059c43633b6e4ac68b63f3635c26";
    public string secretKey = "657df3402a2569aedae419a6af0f2c225cfbbd07458f91ff89a1b62960da6e4b";

    // Emails
    public string senderEmail = "admin@articonnect.com";
    public string contactEmail = "contact@articonnect.com";

    // Validation
    public string regexEmail = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" 
                              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" 
                              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
}
