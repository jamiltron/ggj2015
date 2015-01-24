using System.Collections.Generic;
using UnityEngine;

public class InstrumentAudio : MonoBehaviour
{
	public List<AudioClip> sounds;

	public void OnKeyPress(int keyIndex)
	{
		AudioClip clip = sounds[keyIndex % sounds.Count];
		audio.PlayOneShot(clip);
	}
}