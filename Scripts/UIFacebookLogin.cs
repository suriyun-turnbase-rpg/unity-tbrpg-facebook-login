using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Facebook.Unity;

public class UIFacebookLogin : MonoBehaviour
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
    public UnityEvent onLoginSuccess;
    public UnityEvent onLoginCancelled;
    public StringEvent onLoginFail;

    private void Start()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
        }
        else
        {
            // Show error message
            onLoginFail.Invoke("Failed to Initialize the Facebook SDK");
        }
    }

    public void OnClickFacebookLogin()
    {
        if (FB.IsLoggedIn)
        {
            RequestFacebookLogin(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString);
        }
        else
        {
            List<string> perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }

    private void RequestFacebookLogin(string id, string accessToken)
    {
        BaseFacebookLoginService loginService = FindObjectOfType<BaseFacebookLoginService>();
        loginService.LoginWithFacebook(id, accessToken, (result) =>
        {
            if (result.Success)
            {
                GameInstance.Singleton.OnGameServiceLogin(result);
                onLoginSuccess.Invoke();
            }
            else
            {
                onLoginFail.Invoke(result.error);
            }
        });
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // When facebook login success, send login request to server
            RequestFacebookLogin(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString);
        }
        else if (result.Cancelled)
        {
            // Cancelled
            onLoginCancelled.Invoke();
        }
        else if (!string.IsNullOrEmpty(result.Error))
        {
            // Show error message
            onLoginFail.Invoke(result.Error);
        }
    }
}
