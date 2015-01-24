using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

  private Rigidbody2D body;
  
	public float walkSpeed = 10f;

  void Awake() {
    body = GetComponent<Rigidbody2D>();
  }

  void Update() {
    int normalizedHorizontal = 0;
    if (Input.GetButton("Left")) {
      normalizedHorizontal -= 1;
    }

    if (Input.GetButton("Right")) {
      normalizedHorizontal += 1;
    }

    body.AddForce(new Vector2(walkSpeed * normalizedHorizontal, 0));
  }


}
