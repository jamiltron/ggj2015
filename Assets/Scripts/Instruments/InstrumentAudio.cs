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

  public void ContinousPress(int keyIndex)
  {
    AudioClip clip = sounds[keyIndex % sounds.Count];
    if (!audio.isPlaying)
    {
      audio.PlayOneShot(clip);
      audio.loop = true;
    }
  }

  public void Stop()
  {
    audio.Stop();
    audio.loop = false;
  }
}