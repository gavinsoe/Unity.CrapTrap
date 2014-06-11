using UnityEngine;
using System;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.review;
using com.shephertz.app42.paas.sdk.csharp.log;

/*public class UnityCallBack : App42CallBack
{
    public void OnSuccess(object response)
    {
        App42Log.Console("Success : " + response);
    }

    public void OnException(Exception e)
    {
        App42Log.Console("Exception : " + e);
    }
}*/

public class LogResponse : App42CallBack
{
    public void OnSuccess(object response)
    {
        if (Debug.isDebugBuild) Debug.Log(response.ToString());
    }

    public void OnException(Exception e)
    {
        if (Debug.isDebugBuild) Debug.Log(e.Message);
    }
}