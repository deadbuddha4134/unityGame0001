using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRigidBodyController : MonoBehaviour
{   
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float sprintMultiplyer = 2f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -30f;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] float smoothBlend = 0.1f;

    [SerializeField] Animator customAnimator;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private Vector3 velocity;
    private Vector3 movementInput;

    private float sprint = 1f;
    private bool isGrounded;

    void Awake()
    {
        
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        customAnimator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            customAnimator.SetBool("isJumping", false);
            controller.height = 2.05f;
            controller.center = new Vector3(0, 1f, 0);
        }

        float x = movementInput.x;
        float z = movementInput.z;
        Vector3 move = transform.right * x + transform.forward * z;
        
        controller.Move(move * playerSpeed * sprint * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        customAnimator.SetFloat("MoveX", x, smoothBlend, Time.deltaTime);
        customAnimator.SetFloat("MoveZ", z, smoothBlend, Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {   
        movementInput = value.Get<Vector3>();
    }

    private void OnJump()
    {
        if (!customAnimator.GetBool("isJumping"))
        {   
            customAnimator.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            controller.height = 1.5f;
            controller.center = new Vector3(0, 1.75f, 0);
        }
    }

    private void OnSprint()
    {
        if (!customAnimator.GetBool("isRunning"))
        {
            customAnimator.SetBool("isRunning", true);
            sprint = sprint * sprintMultiplyer;

        }
        else
        {
            customAnimator.SetBool("isRunning", false);
            sprint = 1f;
        }
    }
}