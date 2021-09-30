using UnityEngine;
using UnityEngine.UI;

public class SoundCaller : MonoBehaviour {
	[SerializeField] Slider musicSlider;
	[SerializeField] Slider sfxSlider;

	private SoundManager soundManager = null;

	private void Start() {
		if (musicSlider && sfxSlider) {
			musicSlider.value = PlayerPrefs.GetFloat("musicVol", .5f);
			sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", .5f);
		}
	}

	private void GetSoundManager() {
		if (!soundManager) {
			soundManager = FindObjectOfType<SoundManager>();
		}
	}

	public void PlaySound (string name) {
		GetSoundManager();
		soundManager.PlaySound(name);
	}

	public void PlayMusic (string name) {
		GetSoundManager();
		soundManager.PlayMusic(name);
	}

	public void ChangeMusicVol(float value) {
		GetSoundManager();
		soundManager.musicSource.volume = value;
	}

	public void ChangeSFXVol(float value) {
		GetSoundManager();
		soundManager.sfxSource.volume = value;
	}

    public void SaveMusicPref(Slider volSlider) {
        PlayerPrefs.SetFloat("musicVol", volSlider.value);
    }

    public void SaveSFXPref(Slider volSlider) {
        PlayerPrefs.SetFloat("sfxVol", volSlider.value);
    }
}