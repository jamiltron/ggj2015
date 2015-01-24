using UnityEngine;
using System.Collections;

public class FilterProvider : MonoBehaviour {

  public string filterName;

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      networkView.RPC("AddFilter", RPCMode.All, other.gameObject);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      networkView.RPC("RemoveFilter", RPCMode.All, other.gameObject);
    }
  }

  [RPC]
  public void AddFilter(GameObject target) {
    target.AddComponent(filterName);
  }

  [RPC]
  public void RemoveFilter(GameObject target) {
    Destroy(target.GetComponent(filterName));
  }


}
