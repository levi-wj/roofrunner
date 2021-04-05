using UnityEngine;

public class KillBlock : MonoBehaviour
{
    [SerializeField] bool destroyOnTouch = false;
    [SerializeField] bool playerFall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (playerFall) {
                player.Fall();
            } else {
                player.Die();

                if (destroyOnTouch) {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
