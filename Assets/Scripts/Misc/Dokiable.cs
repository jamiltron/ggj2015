using UnityEngine;
using System.Collections;

public class Dokiable : MonoBehaviour {

  public enum DokiState {
    Grounded,
    Held,
    Dropped
  }

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

  private struct DokiRaycastOrigins {
    public Vector3 bottomRight;
    public Vector3 bottomLeft;
  }

  public class DokiCollisionState2D {
    public bool right;
    public bool left;
    public bool above;
    public bool below;
    
    public bool hasCollision() {
      return below || right || left || above;
    }
    
    public void reset() {
      right = left = above = below = false;
    }
  }

  private float _verticalDistanceBetweenRays;
  private float _horizontalDistanceBetweenRays;

  [SerializeField]
  [Range( 0.001f, 0.3f )]
  private float _skinWidth = 0.02f;


  public bool isGrounded { get { return collisionState.below; } }

  void Awake() {
    myTransform = GetComponent<Transform>();
    myCollider = GetComponent<BoxCollider2D>();
    recalculateDistanceBetweenRays();
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

  public void recalculateDistanceBetweenRays() {
    var colliderUseableWidth = myCollider.size.x * Mathf.Abs(transform.localScale.x) - (2f * _skinWidth);
    _horizontalDistanceBetweenRays = colliderUseableWidth / (totalVerticalRays - 1);
  }

  [HideInInspector]
  public DokiCollisionState2D collisionState = new DokiCollisionState2D();

  [HideInInspector]
  private DokiRaycastOrigins raycastOrigins;

  [Range( 2, 20 )]
  public int totalVerticalRays = 4;

  private const float kSkinWidthFloatFudgeFactor = 0.001f;

  private void primeRaycastOrigins(Vector3 futurePosition, Vector3 deltaMovement) {
    // our raycasts need to be fired from the bounds inset by the skinWidth
    var modifiedBounds = myCollider.bounds;
    modifiedBounds.Expand(-_skinWidth);

    raycastOrigins.bottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);
    raycastOrigins.bottomLeft = modifiedBounds.min;
  }

  void Update() {
    if (dokiState == DokiState.Held) {
      gameObject.layer = LayerMask.NameToLayer("Held");
      Vector2 newPosition = myTransform.position;
      newPosition.x = holder.transform.position.x;
      newPosition.y = heldY();
      myTransform.position = newPosition;
    } else if (dokiState == DokiState.Dropped || dokiState == DokiState.Grounded) {
      float deltaY = gravity * Time.deltaTime;
      Vector3 delta = new Vector3(0f, deltaY, 0f);
      primeRaycastOrigins(myTransform.position + delta, delta);

      if (deltaY != 0) {
        moveVertically(ref delta);
      }

      // move then update our state
      transform.Translate(delta, Space.World);
      
      // only calculate velocity if we have a non-zero deltaTime
      if (Time.deltaTime > 0) {
        velocity = delta / Time.deltaTime;
      }
    }
  }

  private void moveVertically(ref Vector3 deltaMovement) {
    var rayDistance = Mathf.Abs(deltaMovement.y) + _skinWidth;
    var rayDirection = -Vector2.up;
    var initialRayOrigin = raycastOrigins.bottomLeft;
    
    // apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
    initialRayOrigin.x += deltaMovement.x;
    
    for (var i = 0; i < totalVerticalRays; i++) {
      var ray = new Vector2(initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y);

      RaycastHit2D raycastHit = Physics2D.Raycast(ray, rayDirection, rayDistance, stoppableLayers);
      if (raycastHit) {
        // set our new deltaMovement and recalculate the rayDistance taking it into account
        deltaMovement.y = raycastHit.point.y - ray.y;
        rayDistance = Mathf.Abs(deltaMovement.y);
        
        // remember to remove the skinWidth from our deltaMovement
        deltaMovement.y += _skinWidth;
        collisionState.below = true;
        Ground();
        
        // we add a small fudge factor for the float operations here. if our rayDistance is smaller
        // than the width + fudge bail out because we have a direct impact
        if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor) {
          return;
        }
      }
    }

  }

  public void Ground() {
    if (dokiState == DokiState.Dropped) {
      dokiState = DokiState.Grounded;
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
