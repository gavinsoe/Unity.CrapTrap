using Chartboost;
using UnityEngine;
using System.Collections;

public class ChartboostManager : MonoBehaviour {

    Constants constants = new Constants();

	// Use this for initialization
	void Start () {

	}

    void OnEnable()
    {
        // Initialize the Chartboost plugin
        #if UNITY_ANDROID
        // Replace these with your own Android app ID and signature from the Chartboost web portal
        CBBinding.init(constants.appId_Android, constants.appSignature_Android);
        
        #elif UNITY_IPHONE
        // Replace these with your own iOS app ID and signature from the Chartboost web portal
        CBBinding.init(constants.appId_IOS, constants.appSignature_IOS);

        #endif
    }

    // On Android, you must also override two more MonoBehaviour methods to help
    // control the lifecycle of the Chartboost plugin.
    #if UNITY_ANDROID
    void OnApplicationPause(bool paused)
    {
        // Manage Chartboost plugin lifecycle
        CBBinding.pause(paused);
    }

    void OnDisable()
    {
        // Shut down the Chartboost plugin
        CBBinding.destroy();
    }
    #endif

}
