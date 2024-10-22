using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private Vector3 jumpVelocity;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float rotationRate = 5f; // Adjust as necessary
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    private Animator animator;

    // Ground detection
    [SerializeField] private Transform groundCheck;     // The object to check the ground from
    [SerializeField] private float groundDistance = 0.3f; // Distance for ground detection
    [SerializeField] private LayerMask groundMask;      // Which layers count as ground
    private bool grounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground check using a sphere cast
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Set the isGrounded parameter in the Animator
        animator.SetBool("isGrounded", grounded);

        // Reset jump velocity when grounded
        if (grounded && jumpVelocity.y < 0)
        {
            jumpVelocity.y = 0f;
        }

        // Get player input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Set moveDirection based on input
        moveDirection = new Vector3(horizontal, 0, vertical);

        // Check if the player is moving (with a small threshold)
        bool isMoving = moveDirection.magnitude > 0.1f;

        // Set the isMoving parameter in the Animator
        animator.SetBool("isMoving", isMoving);

        // Only rotate and move if the player is actually moving
        if (isMoving)
        {
            // Normalize moveDirection to keep it consistent
            moveDirection.Normalize();

            // Rotate the character towards the move direction smoothly
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationRate * Time.deltaTime);
            }

            // Apply movement speed
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
        }

        // Jumping logic
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Set jump velocity using the formula for jump height
            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // Apply gravity to the jump velocity
        jumpVelocity.y += gravity * Time.deltaTime;

        // Move the character vertically (jumping and falling)
        controller.Move(jumpVelocity * Time.deltaTime);
    }
}
