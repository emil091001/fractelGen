using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    public float rotationSpeed = 2f;

    private CharacterController characterController;

    private float pitch = 0f;

    private float rotation = 0f;

    private FractalSettings fractalSettings;

    void Start()
    {
        Vector3 p1 = new Vector3(1, 2, 1);
        Vector3 p2 = new Vector3(3, 1, -2);

        Debug.Log(Vector3.Max(p1, p2));

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fractalSettings = GetComponent<FractalSettings>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float distence = fractalSettings.getDistence();

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Ignore the vertical component
        Vector3 cameraRight = Camera.main.transform.right;

        Vector3 movement =
            (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Apply movement
        characterController
            .Move(movement * movementSpeed * Time.deltaTime * distence);

        // Handle vertical movement (up and down)
        float upDownInput = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            upDownInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            upDownInput = -1f;
        }

        Vector3 verticalMovement = new Vector3(0f, upDownInput, 0f);
        characterController
            .Move(verticalMovement * movementSpeed * Time.deltaTime * distence);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera horisontaly
        rotation += mouseX * rotationSpeed;

        // Rotate the camera vertically
        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        Camera.main.transform.localRotation =
            Quaternion.Euler(pitch, rotation, 0f);
    }
}
