using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] GameObject explosion = null;

    private ScoresController score = null;
    private CameraShake cam = null;

    private void Start()
    {
        score = FindObjectOfType<ScoresController>();
        cam = FindObjectOfType<CameraShake>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shot") {
            GameObject instantiated = Instantiate(explosion, transform.position, transform.rotation, transform.parent);
            instantiated.GetComponent<Exploder>().SetCB(DestroyObj);
            cam.SetCameraShake(.08f, .03f);
            score.changeScore(20);
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
