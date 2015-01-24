using UnityEngine;
using System.Collections;

public class FilterProvider : MonoBehaviour {

  public string filterName;

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      Debug.Log("Applying filter to instrument!");
      other.gameObject.AddComponent(filterName);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Instrument") {
      Debug.Log("Removing filter from instrument!");
      Destroy(other.gameObject.GetComponent(filterName));
    }

  }

}
