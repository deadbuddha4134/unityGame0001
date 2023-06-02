using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mouseLookCinemachine : MonoBehaviour
{   
    private CustomInputs inputs;
    [SerializeField] float mouseSenesitivity = 100f;
    [SerializeField] float speed;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform playerCamera;
    private float xRotation = 0f;
    private float yRotation;
    private Vector2 mouseAim;

    void Awake()
    {   
        inputs = new CustomInputs();
        Cursor.lockState = CursorLockMode.Locked;
    }

      void Update()
    {
        // Look();
        // LookRotate();
    }

    void LateUpdate()
    {
        Look();
    }

    void FixedUpdate()
    {
        LookRotate();
    }

    void Look()
    {
        mouseAim = inputs.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseAim.x * mouseSenesitivity;
        float mouseY = mouseAim.y * mouseSenesitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        yRotation -= mouseX;

        Quaternion targetX = Quaternion.Euler(xRotation, 0, 0);
        Quaternion targetY = Quaternion.Euler(0, -yRotation, 0);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetX, Time.deltaTime * speed);
        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetY, speed);
    }

    void LookRotate()
    {
        mouseAim = inputs.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseAim.x * mouseSenesitivity;
        
        yRotation -= mouseX;
    
        Quaternion targetY = Quaternion.Euler(0, -yRotation, 0); 
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetY, speed);
    }

    void OnEnable()
    {
        inputs.Enable();
    }

    void OnDisable()
    {
        inputs.Disable();
    }
}