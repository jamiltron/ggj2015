using UnityEngine;
using System.Collections;

public class PitchShifter : MonoBehaviour {

  private int pitchOriginal;
  private Transform transform;
  private RangedPlayable playable;
  private AudioSource audio;

  public int pitchAmount;
  public int pitchMin;
  public int pitchMax;
  public string keyName;

  void Awake() {
    transform = GetComponent<Transform>();
    GameObject parentObj = transform.parent.gameObject;
    playable = parentObj.GetComponent<RangedPlayable>();
    audio = parentObj.GetComponent<AudioSource>();
  }

  void Update() {
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
    audio.pitch += pitchAmount;

    if (audio.pitch > pitchMax) {
      audio.pitch = (audio.pitch - pitchMax) + pitchMin;
    }
  }
  
}
