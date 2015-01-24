using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class KeyedInstrument : MonoBehaviour {

  private List<GameObject> playersInRange;
  private AudioSource audioSource;

  public List<string> keys;
  public List<AudioClip> sounds;
  public AudioClip testClip;

  void Awake() {
    playersInRange = new List<GameObject>();
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {
    foreach (var player in playersInRange) {
      NetworkView playerView = player.GetComponent<NetworkView>();
      if (playerView.isMine) {
        Debug.Log("playerView is mine!");
        if (Input.GetButtonDown("Play Instrument")) {
          Debug.Log("Playing instrument!");
          audioSource.clip = testClip;
          audioSource.Play();
        }
      }
    }
  }

  [RPC]
  public void PlaySound(string keyName) {
    audioSource.clip = testClip;
    audioSource.Play();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Player") {
      Debug.Log("a player has entered instrument range");
      playersInRange.Add(other.gameObject);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Player") {
      Debug.Log("a player has left instrument range");
      playersInRange.Remove(other.gameObject);
    }
  }
}
