using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private float moveAmount = .025f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float offset = 0;

    private float startY = 0;

    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Float up and down
        transform.position = new Vector3(
            transform.position.x,
            startY + (Mathf.Sin((Time.time * moveSpeed) + offset) * moveAmount),
            transform.position.z
        );
    }
}
