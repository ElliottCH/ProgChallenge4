using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreLeaderboard : MonoBehaviour
{
    public InputField highScoreText;
    public Button postHighScoreButton;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        highScoreText.enabled = false;
        postHighScoreButton.enabled = false;
        score = 1;
    }

    private void FixedUpdate()
    {
        if (PlayfabManager.Instance.state == PlayfabManager.LoginStates.Success)
        {
            highScoreText.enabled = true;
            postHighScoreButton.enabled = true;
        }
    }

    public void PostHighScore()
    {
        //int score = 0;
        //if (int.TryParse(highScoreText.text, out score) == false)
        //{
        //    Debug.Log("ERROR: High score is not a number");
        //    return;
        //}
        score++;
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate() { StatisticName = "playerHighScore", Value = score}
                }
            },
            OnUpdatePlayerStatisticsResponse,
            OnPlayFabError
        );
    }

    public void OnUpdatePlayerStatisticsResponse(UpdatePlayerStatisticsResult response)
    {
        Debug.Log("User statistics updated");
    }

    public void OnPlayFabError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
    }

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest()
            {
                StatisticName = "playerHighScore",
                StartPosition = 0,
                MaxResultsCount = 5      
            },
            OnLeaderboardResultResponse,
            OnPlayFabError
        );
    }

    public void OnLeaderboardResultResponse(GetLeaderboardResult response)
    {
        string leaderBoardName = (response.Request as GetLeaderboardRequest).StatisticName;
        Debug.Log("Get Leaderboard Completed " + leaderBoardName);

        foreach(PlayerLeaderboardEntry playerDetails in response.Leaderboard)
        {
            Debug.Log(string.Format("Player {0} has Rank {1} with a score of {2}",
                playerDetails.DisplayName,
                playerDetails.Position,
                playerDetails.StatValue
                ));
        }
    }
}
