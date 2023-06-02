using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerRigidBodyController : MonoBehaviour
{   
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float sprintMultiplier = 2f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float smoothBlend = 0.1f;
    [SerializeField] float groundDistance = 0.1f;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float maxSlopeAngle = 55f;
    [SerializeField] float deathFloorHeight = -100f;

    [SerializeField] Animator customAnimator;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform orientation;
    [SerializeField] Transform groundCheck;

    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    
    private Vector2 movementInput;
    private Vector3 movementDirection;
    private RaycastHit slopeHit;

    private float sprint = 1f;
    private float currentSlopeAngle;
    private bool isGrounded;
    private bool canJump;
    private bool exitSlope;

    private void Start() 
    {   
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerCollider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {   
        SpeedControl();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        customAnimator.SetBool("isGrounded", isGrounded);
        
        if (isGrounded)
        {   
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        if (transform.position.y < deathFloorHeight)
        {   
            Time.timeScale = 0;
            gameOverMenu.SetActive(true);
        }
        customAnimator.SetFloat("MoveX", movementInput.x, smoothBlend, Time.deltaTime);
        customAnimator.SetFloat("MoveZ", movementInput.y, smoothBlend, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision other)
    {   
        canJump = true;
        exitSlope = false;

        customAnimator.ResetTrigger("jump");
        customAnimator.SetBool("isGrounded", true);
    }

    private void OnCollisionExit(Collision other)
    {
        exitSlope = true;
    }

    private void Movement()
    {   
        movementDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        if (isGrounded)
        {
            rb.AddForce(movementDirection.normalized * movementDirection.magnitude * playerSpeed * sprint * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(movementDirection.normalized * playerSpeed * sprint * 10f * airMultiplier, ForceMode.Force);
        }

        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * playerSpeed * sprint * 10f, ForceMode.Force);

            if (rb.velocity.y > 0  && !exitSlope)
            {
                rb.AddForce(Vector3.down * 50f, ForceMode.Force);
            }
        }

        if (currentSlopeAngle < maxSlopeAngle)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position,Vector3.down, out slopeHit, 0.15f))
        {
            currentSlopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return currentSlopeAngle < maxSlopeAngle && currentSlopeAngle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
         return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
    }

    private void SpeedControl()
    {   
        if (OnSlope() && !exitSlope)
        {
            if (rb.velocity.magnitude > playerSpeed && rb.velocity.y > 0)
            {
                rb.velocity = rb.velocity.normalized * playerSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (isGrounded && flatVelocity.magnitude > playerSpeed && !customAnimator.GetBool("isRunning"))
            {
                Vector3 limitedVeloctiy = flatVelocity.normalized * playerSpeed;
                rb.velocity = new Vector3(limitedVeloctiy.x, rb.velocity.y, limitedVeloctiy.z);
            }
        }   
    }

    private void OnMove(InputValue value)
    {   
        movementInput = value.Get<Vector2>();
    }

    private void OnJump()
    {   
        if (canJump)
        {   

            canJump = false;
            exitSlope = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            customAnimator.SetTrigger("jump");
            customAnimator.SetBool("isGrounded", false);
        }
    }

    private void OnSprint()
    {
        if (!customAnimator.GetBool("isRunning"))
        {
            customAnimator.SetBool("isRunning", true);
            sprint = sprint * sprintMultiplier;

        }
        else
        {
            customAnimator.SetBool("isRunning", false);
            sprint = 1f;
        }
    }

    private void OnPause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}