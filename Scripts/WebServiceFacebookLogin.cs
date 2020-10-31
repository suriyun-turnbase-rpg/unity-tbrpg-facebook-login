using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WebServiceClient))]
public class WebServiceFacebookLogin : BaseFacebookLoginService
{
    private WebServiceClient webServiceClient;

    private void Awake()
    {
        webServiceClient = GetComponent<WebServiceClient>();
    }

    public override void LoginWithFacebook(string userId, string accessToken, UnityAction<PlayerResult> onFinish)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("userId", userId);
        dict.Add("accessToken", accessToken);
        webServiceClient.PostAsDecodedJSON<PlayerResult>("/login-with-facebook", (www, result) =>
        {
            onFinish(result);
        }, dict);
    }
}
