using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] float time = 5;
    [SerializeField] GameObject respawnUI = null;
    [SerializeField] DeathScore endgameUI = null;
    [SerializeField] Button respawnButton;

    private Slider slide = null;
    private bool isCounting = false;
    private ScoresController score = null;

    void Awake()
    {
        score = FindObjectOfType<ScoresController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting) {
            slide.value += (10.1f - time) * Time.deltaTime;
            if (slide.value >= 1) {
                respawnUI.SetActive(false);
                endgameUI.Show();
                isCounting = false;
            }
        }
    }

    public void StartCounting()
    {
        slide = GetComponent<Slider>();
        slide.value = 0f;
        isCounting = true;
        if (score.creds < 15) {
            respawnButton.interactable = false;
        }
    }
}
