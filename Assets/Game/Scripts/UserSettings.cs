using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettings : MonoBehaviour
{
    // Get the button and the input text
    public Button updateUserNameButton;
    public InputField userNameText;

    // At start disable the UI elements
    private void Start()
    {
        updateUserNameButton.enabled = false;
        userNameText.enabled = false;   
    }

    // Through an update check if we are logged in and if so enable to UI elements
    private void FixedUpdate()
    {
        if (PlayfabManager.Instance.state == PlayfabManager.LoginStates.Success)
        {
            updateUserNameButton.enabled = true;
            userNameText.enabled = true;
        }
    }

    // Add a method to change the Username
    public void OnUpdateUserName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
                        new UpdateUserTitleDisplayNameRequest() { DisplayName = userNameText.text },
                        result =>
                        {
                            Debug.Log("Success");
                        },
                        error =>
                        {
                            Debug.Log("Failed to Update Username");
                        }
                    );

        PlayFabClientAPI.WritePlayerEvent(
            new WriteClientPlayerEventRequest()
            {
                EventName = "player_update_displayname",
                Body = new Dictionary<string, object>() { { "name", userNameText.text } }
            },
            result =>
            {
                Debug.Log("Success sending event: player_update_displayname");
            },
            error =>
            {
                Debug.Log("Failed to send event: player_update_displayname");
            }         
        );

    }

    // Use the PlayFabClientAPI to change the DisplayName for the User
}
