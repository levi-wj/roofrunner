using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool startMoving = false;
    [SerializeField] bool increaseSpeedWithLevel = false;
    [SerializeField] float speedIncreaseMultiplier = 1;

    private Spawner level = null;

    private bool isMoving = false;

    private void Start()
    {
        if(increaseSpeedWithLevel) {
            level = FindObjectOfType<Spawner>();
            speed += level.getSpeed() * speedIncreaseMultiplier;
        }
        isMoving = startMoving;
    }

    public void setSpeed(float amount)
    {
        speed = amount;
    }

    public void setMoving(bool moving)
    {
        isMoving = moving;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
    }
}
