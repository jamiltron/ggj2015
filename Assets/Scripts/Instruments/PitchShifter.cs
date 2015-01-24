using UnityEngine;
using System.Collections;

public class PitchShifter : MonoBehaviour {

  private int pitchOriginal;
  private RangedPlayable playable;
  private AudioSource audioSource;

  public int pitchAmount;
  public int pitchMin;
  public int pitchMax;
  public string keyName;

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
        if (Input.GetButtonDown(keyName)) {
          networkView.RPC("ShiftPitch", RPCMode.All);
        }
      }
    }
  }

  [RPC]
  public void ShiftPitch() {
    audioSource.pitch += pitchAmount;

    if (audioSource.pitch > pitchMax) {
      audioSource.pitch = (audioSource.pitch - pitchMax) + pitchMin;
    }
  }
  
}
