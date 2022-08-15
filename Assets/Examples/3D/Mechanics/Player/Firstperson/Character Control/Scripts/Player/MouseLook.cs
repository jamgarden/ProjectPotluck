using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Camera mainCamera;

    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private float cameraHeight = 1.4f;

    public bool canTurn = true;
    private float mouseX, mouseY;
    private float xRotation;

    private void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        mainCamera.transform.position = transform.position + new Vector3(0, cameraHeight, 0);
        mainCamera.transform.parent = transform;

        xRotation = 0;
    }

    private void Update()
    {
        if (canTurn)
        {
            RotateCamera();
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
    }

    void RotateCamera()
    {
        xRotation -= mouseY;

        // Limit mouse
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
