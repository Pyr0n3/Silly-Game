using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private Vector3 jumpVelocity;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float rotationRate = 0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool grounded;
    private Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        grounded = controller.isGrounded;

        if (grounded && jumpVelocity.y < 0)
        {
            jumpVelocity.y = 0f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical);

        bool isMoving = (moveDirection.x != 0 || moveDirection.z != 0);

        animator.SetBool("isMoving", isMoving);

        moveDirection *= speed;

        if (isMoving)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationRate);
        }

        controller.Move(moveDirection * Time.deltaTime);
   
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
           jumpVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        jumpVelocity.y += gravity * Time.deltaTime;
        controller.Move(jumpVelocity * Time.deltaTime);
    }
}
