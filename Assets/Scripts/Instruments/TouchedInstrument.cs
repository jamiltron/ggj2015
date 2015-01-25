using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class TouchedInstrument: RangedPlayable {
  
  private List<GameObject> playersInRange;
  
  override public List<GameObject> GetPlayers() {
    return playersInRange;
  }
  
  void Awake() {
    playersInRange = new List<GameObject>();
  }
  
  void Update() {
    List<GameObject> playersToRemove = new List<GameObject>();
    foreach (var player in playersInRange) {
      if (player == null) {
        playersToRemove.Add(player);
      }
    }
    foreach (var player in playersToRemove) {
      playersInRange.Remove(player);
    }

    if (playersInRange.Count > 0) {
      networkView.RPC("SendTouched", RPCMode.All, 0);
    } else {
      networkView.RPC("SendStop", RPCMode.All);
    }
  }
  
  [RPC]
  void SendTouched(int keyIndex) {
    SendMessage("ContinousPress", keyIndex, SendMessageOptions.DontRequireReceiver);
  }

  [RPC]
  void SendStop() {
    SendMessage("Stop", SendMessageOptions.DontRequireReceiver);
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