using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyedInstrument : MonoBehaviour {

  private List<GameObject> playersInRange;

  public List<string> keys;
  public List<AudioClip> sounds;

  void Awake() {
    playersInRange = new List<GameObject>();
  }

  void Update() {
    // Not done yet
    /*foreach (var key in keys) {
      if (Input.GetButtonDown(key)) {
        int i = keys.FindIndex(key);
      }
    }*/
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
