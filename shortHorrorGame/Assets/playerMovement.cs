using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform orientation;
    public bool canMove = true;

    // Ground Movement
    public Rigidbody rb;
    [SerializeField] private float constantMoveSpeed = 8f;
    [SerializeField] private float crouchingMoveSpeed = 5f;
    [SerializeField]private float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;
    [SerializeField] private float sphereRadiusGroundCheck;
    [SerializeField] private float sphereMaxDistance;
    [SerializeField] private float maxWalkableSlope = 80f;
    [SerializeField] private float slopeStickForce = 10f;
    [SerializeField] private float jumpHoldTimer = 0f;
    [SerializeField] private float crouchingHeight = 1f;
    private CapsuleCollider capColidder;
    private float originalCapsuleHeight;
    private bool isCrouching = false;

    // Jumping
    public float jumpForce = 10f;
    [SerializeField] private float OriginalFallMultipler = 2.5f;
    [SerializeField] private float maxFallMultiplier = -30f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;
    private float surfaceAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        capColidder = GetComponent<CapsuleCollider>();
        originalCapsuleHeight = capColidder.height;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");
    }


    void FixedUpdate()
    {
        MovePlayer();
        ApplyDrag();
        GroundCheck();

    }
    void GroundCheck()
    {
        if (groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.3f;
            RaycastHit hit;

            bool hitSomething = Physics.SphereCast(rayOrigin, sphereRadiusGroundCheck, Vector3.down, out hit, sphereMaxDistance, groundLayer);
            bool floorDetected = Physics.Raycast(rayOrigin, Vector3.down, sphereMaxDistance, groundLayer);

            if (hitSomething)
            {
                if (hit.distance <= 0f || hit.normal == Vector3.zero)
                {
                    isGrounded = false;

                }
                surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (surfaceAngle <= maxWalkableSlope)
                {
                    isGrounded = true;
                    Vector3 oppositeVectorToSlope = -hit.normal;
                    rb.AddForce(oppositeVectorToSlope * slopeStickForce, ForceMode.Acceleration);
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
    }
    void ApplyDrag()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = horizontalVel.normalized * MoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

    }
    void MovePlayer()
    {
        Vector3 movementDirection = (orientation.forward * moveForward + orientation.right * moveHorizontal).normalized;
        Vector3 targetVelocity = movementDirection * MoveSpeed;

        rb.AddForce(movementDirection.normalized * MoveSpeed * 10f, ForceMode.Acceleration);
        float currentVelocity = rb.linearVelocity.magnitude;
    }

}