using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource player;
    [SerializeField] AudioClip[] clips;

    private int curSong = -1;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1) Destroy(gameObject);

        PlaySong(0);
        DontDestroyOnLoad(this);
    }

    public void PlaySong(int id) {
        if (id != curSong) {
            curSong = id;
            player.clip = clips[id];
            player.Play();
        }
    }

    public void FadeVolume(float target, float fadeTime) {
        StartCoroutine(StartFade(player, target, fadeTime));
    }

   static IEnumerator StartFade(AudioSource player, float target, float fadeTime) {
       float start = player.volume;
       float currentTime = 0;

       while (currentTime < fadeTime) {
           currentTime += Time.deltaTime;
           player.volume = Mathf.Lerp(start, target, currentTime / fadeTime);
           yield return null;
       }

       yield break;
   }
}
