using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class JumpedInstrument: RangedPlayable {
  
  private List<GameObject> playersInRange;
  
  override public List<GameObject> GetPlayers() {
    return playersInRange;
  }
  
  void Awake() {
    playersInRange = new List<GameObject>();
  }
  
  void Update() {
    foreach (var player in playersInRange) {
      PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
      if (playerMovement.collisionState.becameGroundedThisFrame) {
        networkView.RPC("SendOnKeyPress", RPCMode.All, 0);
      }
    }
  }
  
  [RPC]
  void SendOnKeyPress(int keyIndex) {
    BroadcastMessage("OnKeyPress", keyIndex, SendMessageOptions.DontRequireReceiver);
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