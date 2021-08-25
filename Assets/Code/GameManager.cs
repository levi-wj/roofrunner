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
    private MusicManager music;
    private bool isGamePaused;

    private void Start()
    {
        level = FindObjectOfType<Spawner>();
        scores = FindObjectOfType<ScoresController>();
        bgs = FindObjectsOfType<TextureScroller>();
        player = FindObjectOfType<PlayerController>();
        music = FindObjectOfType<MusicManager>();
    }

    public void pauseGame()
    {
        isGamePaused = true;

        level.setMoving(false);
        music.FadeVolume(.2f, .2f);
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
        music.FadeVolume(1, .5f);
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
        music.FadeVolume(1, .5f);
        music.PlaySong(1);
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
