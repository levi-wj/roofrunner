using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exploder : MonoBehaviour
{
    private SoundManager sound = null;
    private UnityAction CB;

    private void Start() {
        sound = FindObjectOfType<SoundManager>();
    }

    public void SetCB(UnityAction cb)
    {
        CB = cb;
    }

    public void CallCB()
    {
        CB.Invoke();
    }

    public void KillYourself()
    {
        sound.PlaySound("explode");
        Destroy(gameObject);
    }
}
