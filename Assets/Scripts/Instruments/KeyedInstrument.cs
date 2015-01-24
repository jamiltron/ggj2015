using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class KeyedInstrument : MonoBehaviour {

  private List<GameObject> playersInRange;
  private AudioSource audioSource;

  public List<string> keys;
  public List<AudioClip> sounds;

  void Awake() {
    playersInRange = new List<GameObject>();
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {
    foreach (var player in playersInRange) {
      NetworkView playerView = player.GetComponent<NetworkView>();
      if (playerView.isMine) {
        foreach (var key in keys) {
          if (Input.GetButtonDown(key)) {
            networkView.RPC("PlaySoundFromKey", RPCMode.All, key);
          }
        }
      }
    }
  }

  [RPC]
  public void PlaySoundFromKey(string keyName) {
    int i = keys.IndexOf(keyName);
    audioSource.clip = sounds[i];
    audioSource.Play();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Player") {
      playersInRange.Add(other.gameObject);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Player") {
      playersInRange.Remove(other.gameObject);
    }
  }
}
