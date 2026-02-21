using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [Header("Carry")]
    public Transform holdPoint;
    public float pickupRange = 3f;
    public float carrySmoothSpeed = 15f;
    public float throwForce = 10f;
    public LayerMask carryLayer;
    private Rigidbody carriedObject;
    private Collider carriedCollider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (TimeManager.Instance.CurrentState == TimeState.Rewinding)
            return;
        Move();

        if (InputManager.Instance.Carry())
        {
            if (carriedObject == null)
                Pickup();
            else
                Drop();
        }
        if (carriedObject != null)
        {
            MoveCarriedObject();
        }

    }
    void Move()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }
        Vector2 input = InputManager.Instance.GetPlayerMovement();
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;
        move = Vector3.ClampMagnitude(move, 1f);
        float delta = TimeManager.Instance.CurrentState == TimeState.Paused
            ? Time.unscaledDeltaTime
            : Time.deltaTime;
        if (groundedPlayer && InputManager.Instance.Jump())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
        playerVelocity.y += gravityValue * delta;

        Vector3 finalMove = move * speed + playerVelocity;
        controller.Move(finalMove * delta);
    }
    void Pickup()
    {
        Debug.Log("Carry");
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, carryLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                carriedObject = rb;
                carriedCollider = rb.GetComponent<Collider>();

                rb.isKinematic = true;
                rb.useGravity = false;
                carriedCollider.enabled = false;
            }
        }
    }
    void MoveCarriedObject()
    {
        carriedObject.transform.position = holdPoint.position;
        carriedObject.transform.rotation = holdPoint.rotation;
    }

    void Drop()
    {
        carriedObject.isKinematic = false;
        carriedObject.useGravity = true;
        carriedCollider.enabled = true;

        carriedObject = null;
        carriedCollider = null;
    }
}
