using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;

    private float xRotation = 0.0f;

    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // 카메라의 초기위치를 플레이어의 정면 방향으로 설정
        transform.localRotation = Quaternion.Euler(0.0f, playerBody.eulerAngles.y, 0.0f);
    }

    void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate the new rotation around the x-axis
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        // Rotate the camera around the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Rotate the player body around the y-axis
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
