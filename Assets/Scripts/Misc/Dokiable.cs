using UnityEngine;
using System.Collections;

public class Dokiable : MonoBehaviour {

  public enum DokiState {
    Grounded,
    Held,
    Dropped
  }

  public Vector2 throwSpeed;
  public float gravity;
  public float verticalBuffer = 0.1f;
  public float horizontalBuffer = 0.1f;
  public DokiState dokiState;
  public Vector2 velocity;

  public LayerMask stoppableLayers;
  public LayerMask oneWayLayers;

  private GameObject holder;
  private Transform myTransform;
  private BoxCollider2D myCollider;


  void Awake() {
    myTransform = GetComponent<Transform>();
    myCollider = GetComponent<BoxCollider2D>();
  }

  public void Pickup(GameObject grabber) {
    if (dokiState == DokiState.Grounded) {
      holder = grabber;
      Vector2 newPosition = myTransform.position;
      newPosition.y = heldY();
      newPosition.x = holder.transform.position.x;
      myTransform.position = newPosition;
      dokiState = DokiState.Held;
    }
  }

  void Update() {
    if (dokiState == DokiState.Held) {
      gameObject.layer = LayerMask.NameToLayer("Held");
      Vector2 newPosition = myTransform.position;
      newPosition.x = holder.transform.position.x;
      newPosition.y = heldY();
      myTransform.position = newPosition;
    } else if (dokiState == DokiState.Dropped) {
    }
  }

  public void Drop() {
    if (dokiState == DokiState.Held) {
      dokiState = DokiState.Dropped;
      gameObject.layer = LayerMask.NameToLayer("Pushable");
      Vector2 newPosition = myTransform.position;
      if (holder.transform.localScale.x < 0) {
        newPosition.x = droppedX(-1);
      } else {
        newPosition.x = droppedX(1);
      }
      myTransform.position = newPosition;
      holder = null;
    }
  }

  private float heldY() {
    return (holder.transform.position.y + holder.collider2D.bounds.extents.y + myCollider.bounds.extents.y + verticalBuffer);
  }

  private float droppedX(int normalizedX) {
    if (normalizedX > 0) {
      return (holder.transform.position.x + holder.collider2D.bounds.extents.x + myCollider.bounds.extents.x + horizontalBuffer);
    } else {
      return (holder.transform.position.x - holder.collider2D.bounds.extents.x - myCollider.bounds.extents.x - horizontalBuffer);
    }
  }

}
