using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject startPlatform = null;
    [SerializeField] GameObject[] objects = null;
    [SerializeField] int[] repickChance = null;
    [SerializeField] Transform parent = null;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float speedIncrease = 1f;

    private bool moving = false;

    private void Start()
    {
        spawnStartPlatform();
    }

    void Update()
    {
        if (moving) {
            moveSpeed += (speedIncrease / 100) * Time.deltaTime;
        }
    }

    public void spawnObj()
    {
        // Create the object and set the moving script's speed
        int i = PickNumber();
        while ((5 - (int)Random.Range(0, repickChance[i])) != 5) {
            i = PickNumber();
        }

        Instantiate(objects[i], new Vector2(transform.position.x, 0), transform.rotation, parent);
    }

    public void Restart()
    {
        // Delete all existing level pieces
        for (int i = 0; i < parent.childCount; i++) {
            Destroy(parent.GetChild(i).gameObject);
        }

        moveSpeed = 1.2f;

        setMoving(true);
        spawnStartPlatform();
    }

    public float getSpeed()
    {
        return moveSpeed;
    }

    public void setMoving(bool isMoving)
    {
        moving = isMoving;
        setMovers(isMoving);
    }

    public bool getIsMoving()
    {
        return moving;
    }

    private int PickNumber()
    {
        return (int)Random.Range(0f, objects.Length);
    }

    private void spawnStartPlatform()
    {
        Instantiate(startPlatform, new Vector2(0, 0), transform.rotation, parent);
    }

    public void setMovers(bool moving)
    {
        Mover[] movers = FindObjectsOfType<Mover>();

        for (int i = 0; i < movers.Length; i++) {
            movers[i].setMoving(moving);
        }
    }
}
