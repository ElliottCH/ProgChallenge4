using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    public LocationServicesController location;
    public HighScoreLeaderboard highScore;
    public PokemonController controller;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Catch()
    {
        highScore.PostHighScore();
        score++;
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest
        {
            EventName = "Pokemon_Caught",
            Body = new Dictionary<string, object>()
            {
                { "id", controller.pokemon.id },
                { "longitude", location.longitudeText },
                { "latitude", location.latitudeText }
            }
        },
result => Debug.Log("Success"),
error => Debug.LogError(error.GenerateErrorReport()));
    }
}
