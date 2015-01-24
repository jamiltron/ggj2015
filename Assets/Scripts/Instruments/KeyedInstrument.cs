using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class KeyedInstrument : RangedPlayable {

  private List<GameObject> playersInRange;

  public List<string> keys;

  override public List<GameObject> GetPlayers() {
    return playersInRange;
  }

  void Awake() {
    playersInRange = new List<GameObject>();
  }

  void Update() {
    foreach (var player in playersInRange) {
      NetworkView playerView = player.GetComponent<NetworkView>();
      if (playerView.isMine) {
        foreach (var key in keys) {
          if (Input.GetButtonDown(key)) {
			      networkView.RPC("SendOnKeyPress", RPCMode.All, keys.IndexOf(key));
          }
        }
      }
    }
  }

  [RPC]
  void SendOnKeyPress(int keyIndex) {
    SendMessage("OnKeyPress", keyIndex, SendMessageOptions.DontRequireReceiver);
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
