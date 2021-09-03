using UnityEngine;

public class SoundCaller : MonoBehaviour {
	private SoundManager sound = null;

	private void Start () {
		sound = FindObjectOfType<SoundManager>(); 
	}

	public void PlaySound (string name) {
		sound.PlaySound(name);
	}

	public void PlayMusic (string name) {
		sound.PlayMusic(name);
	}
}