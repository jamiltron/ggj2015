using UnityEngine;
using System.Collections;

public class PitchShifter : MonoBehaviour {

  private int pitchOriginal;
  private RangedPlayable playable;
  private AudioSource audioSource;

  public int pitchAmount;
  public int pitchMin;
  public int pitchMax;
  public string upKeyName;
  public string downKeyName;

  void Awake() {
    GameObject parentObj = transform.parent.gameObject;
    playable = parentObj.GetComponent<RangedPlayable>();
    audioSource = parentObj.GetComponent<AudioSource>();
  }

  void Update() {
    Debug.Log("Updating!");
    foreach (var player in playable.GetPlayers()) {
      NetworkView playerView = player.GetComponent<NetworkView>();
      if (playerView.isMine) {
        if (upKeyName != null && Input.GetButtonDown(upKeyName)) {
          networkView.RPC("ShiftPitch", RPCMode.All, pitchAmount);
        }
        if (downKeyName != null && Input.GetButtonDown(downKeyName)) {
          networkView.RPC("ShiftPitch", RPCMode.All, -pitchAmount);
        }
      }
    }
  }

  [RPC]
  public void ShiftPitch(int amount) {
    audioSource.pitch += amount;

    if (audioSource.pitch > pitchMax) {
      audioSource.pitch = pitchMin;
    } else if (audioSource.pitch < pitchMin) {
      audioSource.pitch = pitchMax;
    }
  }
  
}
