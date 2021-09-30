using UnityEngine;
using UnityEngine.Events;

public class Exploder : MonoBehaviour
{
    private SoundManager sound = null;
    private UnityAction CB;

    private void Start() {
        sound = FindObjectOfType<SoundManager>();
        sound.PlaySound("explode");
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
        Destroy(gameObject);
    }
}
