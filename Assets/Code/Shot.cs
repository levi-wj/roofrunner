using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float shootSpeed = 1f;

    void Update()
    {
        transform.Translate(Vector2.right * shootSpeed * Time.deltaTime);
        if (transform.position.x >= 2.2f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
