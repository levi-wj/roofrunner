using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class ScoresController : MonoBehaviour
{
    [SerializeField] float scoreSpeed = 1f;
    [SerializeField] bool addZeros = true;
    [SerializeField] Text scoreText = null;
    [SerializeField] Text hiscoreText = null;
    [SerializeField] Text creditText = null;

    public int score = 0;
    public int creds = 0;
    public int hiscore = 0;

    private bool isGameGoing = false;
    private Animator scoreAnim;
    private Animator credAnim;
    float timer = 0;

    void Start()
    {
        // Initialize all the text, from values loaded from the file
        if (scoreText) {
            scoreAnim = scoreText.gameObject.GetComponent<Animator>();
        }
        if (hiscoreText != null) {
            hiscore = DataSaver.hiscore;
            hiscoreText.text = addLeadingZeros(hiscore.ToString(), 7);
        }
        if (creditText != null) {
            credAnim = creditText.gameObject.GetComponent<Animator>();
            creds = DataSaver.credits;
            creditText.text = addLeadingZeros(creds.ToString(), 4);
        }
    }

    void Update()
    {
        if (isGameGoing) {
            if (scoreText != null) {
                timer += scoreSpeed * Time.deltaTime;
                if (timer >= 1) {
                    timer = 0;
                    changeScore(1);
                }
            }
        }
    }

    public void resetScore()
    {
        score = 0;
        scoreText.text = "0000000";
        setIsScoreCounting(true);
    }

    public void changeScore(int amount)
    {
        score += amount;
        timer += (amount - 1);

        if (amount > 1) {
            // Flash text
            scoreAnim.SetTrigger("Flash");
        }

        scoreText.text = addLeadingZeros(score.ToString(), 7);

        checkHighscore();
    }

    public void changeCredits(int amount)
    {
        creds += amount;
        creditText.text = addLeadingZeros(creds.ToString(), 4);
        DataSaver.credits += amount;

        if (credAnim) {
            credAnim.SetTrigger("Flash");
        }
    }

    public void doItemCollect(string itemName, int value)
    {
        switch (itemName) {
            case "Credit":
                changeCredits(1);
                break;
            case "Gem":
                changeCredits(5);
                break;
            default:
                Debug.LogError("Itemname was not a valid pickup.");
                break;
        }

        changeScore(value);
    }

    public void setIsScoreCounting(bool isGame)
    {
        isGameGoing = isGame;
    }

    public void checkHighscore()
    {
        if (score > hiscore) {
            hiscore = score;
            if (hiscoreText != null) {
                hiscoreText.text = addLeadingZeros(hiscore.ToString(), 7);
            }
        }
    }

    private string addLeadingZeros(string original, int finalLength)
    {
        if (!addZeros) return original;
        StringBuilder builder = new StringBuilder();

        // Add the zeros
        for (int i = 0; i < finalLength - original.Length; i++) {
            builder.Append('0');
        }

        // Add the actual string
        for (int i = 0; i < original.Length; i++) {
            builder.Append(original[i]);
        }

        return builder.ToString();
    }
}
