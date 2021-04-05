using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exploder : MonoBehaviour
{
    private UnityAction CB;

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
        Destroy(gameObject);
    }
}
