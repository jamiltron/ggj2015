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

    transform.Translate(new Vector3(xMovement * Time.deltaTime, 0f, 0f));

    // TODO: check for collisions with walls
  }
}
