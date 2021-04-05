using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    private Spawner level;
    private TextureScroller[] bgs;
    private PlayerController player;
    private ScoresController scores;
    private bool isGamePaused;

    private void Start()
    {
        level = FindObjectOfType<Spawner>();
        scores = FindObjectOfType<ScoresController>();
        bgs = FindObjectsOfType<TextureScroller>();
        player = FindObjectOfType<PlayerController>();
    }

    public void pauseGame()
    {
        isGamePaused = true;

        level.setMoving(false);
        setBackgroundScoll(false);
        scores.setIsScoreCounting(false);
        player.setGameGoing(false);

        save();
    }

    public void restartGame()
    {
        isGamePaused = false;

        setBackgroundScoll(true);
        level.Restart();
        player.Restart();
        scores.resetScore();
    }

    public void save()
    {
        DataSaver.credits = scores.creds;
        DataSaver.hiscore = scores.hiscore;
        PlayGamesController.PostToLeaderboard(scores.hiscore);
        DataSaver.SaveData();
    }

    public void resumeGame()
    {
        if (DataSaver.seenTutorial) {
            isGamePaused = false;
            player.setTutorial(false);
            player.setGameGoing(true);
            level.setMoving(true);
            scores.setIsScoreCounting(true);
            setBackgroundScoll(true);
        } else {
            player.setTutorial(true);
            tutorial.SetActive(true);
            DataSaver.seenTutorial = true;
            DataSaver.SaveData();
        }
    }

    public void stopDive()
    {
        player.StopDive(true);
    }

    private void setBackgroundScoll(bool isScrolling)
    {
        for (int i = 0; i < bgs.Length; i++) {
            bgs[i].setIsMoving(isScrolling);
        }
    }
}
