using System.Collections.Generic;
using UnityEngine;

public class InstrumentAudio : MonoBehaviour
{
	public List<AudioClip> sounds;

	private AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void OnKeyPress(int keyIndex)
	{
		AudioClip clip = sounds[keyIndex % sounds.Count];
		audioSource.PlayOneShot(clip);
	}
}