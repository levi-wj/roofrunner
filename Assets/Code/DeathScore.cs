using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScore : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text hiscoreText = null;

    private ScoresController score = null;

    private void Awake()
    {
        score = FindObjectOfType<ScoresController>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        scoreText.text = "Score: " + score.score;
        hiscoreText.text = "Hiscore: " + score.hiscore;
    }
}
