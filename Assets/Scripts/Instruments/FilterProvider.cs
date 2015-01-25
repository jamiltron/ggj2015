using UnityEngine;
using System.Collections;

public class FilterProvider : MonoBehaviour {

  public string filterName;

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      Debug.Log("Adding filter!");
      networkView.RPC("AddFilter", RPCMode.All, other.gameObject.GetComponent<NetworkView>().viewID);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      Debug.Log("Removing filter!");
      networkView.RPC("RemoveFilter", RPCMode.All, other.gameObject.GetComponent<NetworkView>().viewID);
    }
  }

  [RPC]
  public void AddFilter(NetworkViewID id) {
    GameObject target = NetworkView.Find(id).gameObject;
    target.gameObject.AddComponent(filterName);
  }

  [RPC]
  public void RemoveFilter(NetworkViewID id) {
    GameObject target = NetworkView.Find(id).gameObject;
    Destroy(target.gameObject.GetComponent(filterName));
  }


}
