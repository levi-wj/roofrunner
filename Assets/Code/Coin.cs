using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] string itemName= "";
    [SerializeField] int value = 2;
    [SerializeField] int dissapearChance = 5;

    private ScoresController score;

    void Start()
    {
        if ((int)Random.Range(dissapearChance, 10) == 10) {
            Destroy(gameObject);
        }

        score = FindObjectOfType<ScoresController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            score.doItemCollect(itemName, value);
            Destroy(gameObject);
        }
    }
}
