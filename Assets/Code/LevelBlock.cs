using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    private Spawner levelGenerator = null;
    private Mover move = null;
    private float gapSize = .15f;
    private float gapIncreaseMultiplier = .7f;
    private bool hasSpawnedNewBlock = false;

    private void Start()
    {
        levelGenerator = FindObjectOfType<Spawner>();
        move = GetComponent<Mover>();
    }

    void Update() 
    {
        float speed = levelGenerator.getSpeed();

        if (levelGenerator.getIsMoving()) {
            move.setSpeed(speed);
        } else {
            move.setSpeed(0);
        }

        if (transform.position.x <= (gapSize * -1) - (speed * gapIncreaseMultiplier * Time.deltaTime) && !hasSpawnedNewBlock) {
            hasSpawnedNewBlock = true;
            levelGenerator.spawnObj();
        }

        // If the object has moved offscreen
        if (transform.position.x <= -4) {
            Destroy(gameObject);
        }
    }
}
