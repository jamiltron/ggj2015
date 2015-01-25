﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour {
  public float gravity = -25f;
  public float runSpeed = 8f;
  public float groundDamping = 20f; // how fast do we change direction? higher means faster
  public float inAirDamping = 5f;
  public float jumpHeight = 3f;
  public LayerMask pickupLayer;
  public LayerMask heldLayer;
  public int jumps = 2;
  
  [HideInInspector]
  private float normalizedHorizontalSpeed = 0;

  private int _jumpsLeft;
  private PlayerMovement _controller;
  private RaycastHit2D _lastControllerColliderHit;
  private Vector3 _velocity;
  private Transform _transform;
  private bool _holding;
  
  void Awake() {
    _controller = GetComponent<PlayerMovement>();
    _transform = GetComponent<Transform>();
    _jumpsLeft = jumps;
  }
  
  // the Update loop contains a very simple example of moving the character around and controlling the animation
  void Update() {

    if(!networkView.isMine)
      return;

    if (Input.GetButtonDown("Quit")) {
      Network.Disconnect();
      Application.Quit();
    }

    // grab our current _velocity to use as a base for all calculations
    _velocity = _controller.velocity;
    
    if(_controller.isGrounded) {
      _velocity.y = 0;
    }
    
    if(Input.GetButton("Right")) {
      normalizedHorizontalSpeed = 1;
      if(transform.localScale.x < 0f) {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
      }
    } else if (Input.GetButton("Left")) {
      normalizedHorizontalSpeed = -1;
      if (transform.localScale.x > 0f) {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
      }
    } else {
      normalizedHorizontalSpeed = 0;
    }

    if( _jumpsLeft > 0 && Input.GetButtonDown("Jump")) {
      _jumpsLeft -= 1;
      _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
    }



    if (Input.GetButtonDown("Pickup") && !_holding) {
      bool pickedUp = false;
      pickedUp = TryToPickupObject(-Vector2.up);
      if (!pickedUp) {
        pickedUp = TryToPickupObject(Vector2.right);
      }
      if (!pickedUp) {
        pickedUp = TryToPickupObject(-Vector2.right);
      }
    } else if (Input.GetButtonDown("Pickup")) {
      RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector2.up, 1f, heldLayer);
      if (hit.collider != null && hit.collider.gameObject.tag == "Filter") {
        Dokiable doki = hit.collider.gameObject.GetComponent<Dokiable>();
        doki.Drop();
        _holding = false;
      }
    }
        


    // apply horizontal speed smoothing it
    var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
    _velocity.y += gravity * Time.deltaTime;
        
    _controller.move(_velocity * Time.deltaTime);
    if (_controller.isGrounded) {
      _jumpsLeft = jumps;
    }
  }
  
  private bool TryToPickupObject(Vector2 direction) {
    RaycastHit2D hit = Physics2D.Raycast(_transform.position, direction, 1f, pickupLayer);
    if (hit.collider != null && hit.collider.gameObject.tag == "Filter") {
      Dokiable doki = hit.collider.gameObject.GetComponent<Dokiable>();
      doki.Pickup(gameObject);
      _holding = true;
      return true;
    }
    return false;
  }
}