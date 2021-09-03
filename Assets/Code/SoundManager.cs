using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioClip[] sfx;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1) Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void PlaySound(string name) {
        for (int i = 0; i < sfx.Length; i++) {
            if (sfx[i].name == name) {
                sfxSource.clip = sfx[i];
                sfxSource.Play();
                return;
            }
        }
    }

    public void PlayMusic(string name) {
        if (!musicSource.clip || musicSource.clip.name != name) {
            for (int i = 0; i < music.Length; i++) {
                if (music[i].name == name) {
                    musicSource.clip = music[i];
                    musicSource.Play();
                    return;
                }
            }
        }
    }

    public void FadeMusic(float target, float fadeTime) {
        StartCoroutine(StartFade(musicSource, target, fadeTime));
    }

   static IEnumerator StartFade(AudioSource musicSource, float target, float fadeTime) {
       float start = musicSource.volume;
       float currentTime = 0;

       while (currentTime < fadeTime) {
           currentTime += Time.deltaTime;
           musicSource.volume = Mathf.Lerp(start, target, currentTime / fadeTime);
           yield return null;
       }

       yield break;
   }
}
