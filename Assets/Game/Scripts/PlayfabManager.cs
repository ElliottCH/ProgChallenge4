using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabManager : Singleton<PlayfabManager>
{

    public enum LoginStates
    {
        StartUp,
        LoggingIn,
        Success,
        Failed
    };

    public LoginStates state = LoginStates.StartUp;
    public string PlayerGUID = "";
    public bool createNewPlayer = false;

    public void Awake()
    {
        PlayerGUID = PlayerPrefs.GetString("PlayerGUID", "");
        if (string.IsNullOrEmpty(PlayerGUID) || createNewPlayer == true)
        {
            createNewPlayer = false;
            PlayerGUID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayerGUID", PlayerGUID);
        }
    }

    public void Start()
    {
        state = LoginStates.LoggingIn;
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "917FC";
        }

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() { CustomId = PlayerGUID, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        state = LoginStates.Success;
        //Debug.Log("Login Success: " + result.InfoResultPayload.AccountInfo.Username);
        Debug.Log("Test");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        state = LoginStates.Failed;
        Debug.Log("Unable to log into Playfab Services");
    }
}
