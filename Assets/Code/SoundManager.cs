using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;
    [SerializeField] AudioSource loopaudioSource;
    [SerializeField] AudioClip[] clips;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1) Destroy(gameObject);

        musicSource.volume = PlayerPrefs.GetFloat("musicVol", .5f);
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVol", .5f);

        DontDestroyOnLoad(this);
    }

    private int GetSoundID(string name) {
        for (int i = 0; i < clips.Length; i++) {
            if (clips[i].name == name) {
                return i;
            }
        }

        return -1;
    }

    public void PlaySound(string name) {
        int soundID = GetSoundID(name);
        if (soundID > -1) {
            sfxSource.clip = clips[soundID];
            sfxSource.Play();
        }
    }

    public void PlayMusic(string name) {
        if (!musicSource.clip || musicSource.clip.name != name) {
            int soundID = GetSoundID(name);
            musicSource.clip = clips[soundID];
            musicSource.Play();
        }
    }

    public void StartLoop(string name) {
        int soundID = GetSoundID(name);
        loopaudioSource.clip = clips[soundID];
        loopaudioSource.Play();
    }

    public void EndLoop() {
        loopaudioSource.Stop();
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
