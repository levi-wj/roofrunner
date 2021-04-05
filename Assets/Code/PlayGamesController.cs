using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayGamesController : MonoBehaviour
{
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject LBRow;
    [SerializeField] private Transform ScrollContent;

    private static bool authenticated = false;
    private bool showFriends = false;
    private bool showWeek = false;

    private void Start()
    {
        AuthenticateUser();
    }

    private void PopulateLBUI()
    {
        foreach (Transform child in ScrollContent) {
            GameObject.Destroy(child.gameObject);
        }

        PlayGamesPlatform.Instance.LoadScores(
            GPGSIds.leaderboard_highscore,
            LeaderboardStart.PlayerCentered,
            25,
            showFriends ? LeaderboardCollection.Social : LeaderboardCollection.Public,
            showWeek ? LeaderboardTimeSpan.Weekly : LeaderboardTimeSpan.AllTime,
            (LeaderboardScoreData data) => {
                string[] userIDs = new string[data.Scores.Length];
                for (int i = 0; i < data.Scores.Length; i++) {
                    userIDs[i] = data.Scores[i].userID;
                }

                Social.LoadUsers(userIDs, profiles => {
                    for (int i = 0; i < data.Scores.Length; i++) {
                        CreateLBRow(data.Scores[i].rank, profiles[i].userName, data.Scores[i].value);
                    }
                });
            }
        );
    }

    void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success => {
            if (success) {
                authenticated = true;
                PostToLeaderboard(DataSaver.hiscore);
                PopulateLBUI();
            } else {
                failPanel.SetActive(true);
            }
        });
    }

    public static void PostToLeaderboard(long newScore) {
        if (authenticated) {
            Social.ReportScore(newScore, GPGSIds.leaderboard_highscore, (bool success) => {
                Debug.Log("Post of " + newScore.ToString() + " to leaderboard came back " + success.ToString());
            });
        }
    }

    private void CreateLBRow(int place, string name, long score)
    {
        GameObject row = Instantiate(LBRow, ScrollContent);
        Text[] cells = row.transform.GetComponentsInChildren<Text>();
        cells[0].text = place.ToString(); cells[1].text = name; cells[2].text = score.ToString();
    }

    public void showLB()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscore);
    }

    public void SetFriendFilter(bool showOnlyFriends)
    {
        if (showOnlyFriends != showFriends) {
            showFriends = showOnlyFriends; 
            PopulateLBUI();
        }
    }

    public void SetWeekFilter(bool showOnlyWeek)
    {
        if (showOnlyWeek != showWeek) {
            showWeek = showOnlyWeek;
            PopulateLBUI();
        }
    }
}
