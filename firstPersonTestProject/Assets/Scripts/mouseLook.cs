using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mouseLook : MonoBehaviour
{   
    private CustomInputs inputs;
    [SerializeField] float mouseSenesitivity = 100f;
    private float xRotation = 0f;
    private Vector2 mouseAim;
    private Transform playerBody;

    void Awake()
    {   
        playerBody = transform.parent;

        inputs = new CustomInputs();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Look()
    {
        mouseAim = inputs.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseAim.x * mouseSenesitivity * Time.deltaTime;
        float mouseY = mouseAim.y * mouseSenesitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Update is called once per frame
    void Update()
    {
        Look();
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