using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

  public float raycastLength;
  public float pushSpeed;
  public LayerMask pushinghLayers;
  public LayerMask stoppingLayers;

  private RaycastHit2D hit;
  private Transform myTransform;

  void Awake() {
    myTransform = GetComponent<Transform>();
  }

  void Update() {
    float xMovement = 0;
    hit = Physics2D.Raycast(myTransform.position, -Vector2.right, raycastLength, pushinghLayers);

    if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
      xMovement = pushSpeed;
    }

    hit = Physics2D.Raycast(myTransform.position, Vector2.right, raycastLength, pushinghLayers);
    if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
      xMovement = -pushSpeed;
    }

    Vector2 dir = xMovement > 0 ? Vector2.right : -Vector2.right;
    hit = Physics2D.Raycast(myTransform.position, dir, raycastLength, stoppingLayers);

    if (hit.collider == null) {
      transform.Translate(new Vector3(xMovement * Time.deltaTime, 0f, 0f));
    }
  }

}
