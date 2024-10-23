using UnityEngine;
using UnityEngine.Rendering;

public class CharController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    public Animator animator;

    // Ground detection
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = .5f;
    [SerializeField] private LayerMask groundMask;
    private bool grounded;

    private Vector3 velocity;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check logic
        //grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Gizmos.DrawSphere(groundCheck.position, groundDistance);

        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        

        animator.SetBool("isGrounded", grounded);

        // Reset the y-velocity when grounded
        if (grounded && velocity.y < 0)
        {
            //velocity.y = 0f;//-2f; // Apply a small downward force to stay grounded
            Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.green, 0.1f);
        }
        else
            Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red, 0.1f);

        Debug.Log(groundCheck.up * -groundDistance);

        // Get the camera's forward direction
        Camera camera = Camera.main;
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // Ignore the vertical component
        cameraForward.Normalize(); // Normalize the direction

        // Get movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera orientation
        Vector3 moveDirection = (cameraForward * vertical + camera.transform.right * horizontal).normalized;

        // Check if the player is moving
        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
        //animator.SetFloat("speed", moveDirection.magnitude);

        if (isMoving)
        {
            // Rotate the player to face the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000f * Time.deltaTime);

            // Apply movement
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        // Jumping logic
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Calculate jump velocity using the formula for height
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity to vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (jumping and falling)
        controller.Move(velocity * Time.deltaTime);
    }
}
