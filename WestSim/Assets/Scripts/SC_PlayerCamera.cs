using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerCamera : MonoBehaviour
{
    public float sensX = 100f;
    public float sensY = 100f;
    public Transform orientation;

    float xRotation = 0f;
    float yRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        // calculate rotation
        xRotation -= mouseY;
        yRotation += mouseX;

    // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    // clamp rotation
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);


    }
}
