using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFacebookLoginService : MonoBehaviour
{
    public abstract void LoginWithFacebook(string userId, string accessToken, UnityAction<PlayerResult> onFinish);
}
