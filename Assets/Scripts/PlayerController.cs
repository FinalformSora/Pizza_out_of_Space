using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController player;
    private GameObject eyes;

    private float moveSpeed = 6f;
    private float mouseSensitivity = 100f;
    private float gravity = -29.4f;
    private float jumpHeight = 1.5f;
    private float moveX = 0f;
    private float moveZ = 0f;

    private float xRotation = 0f;
    private bool jumping;

    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<CharacterController>();
        eyes = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded && velocity.y < 0)
        {
            jumping = false;
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            jumping = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        if (!jumping)
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 70f);
        velocity.y += gravity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        eyes.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Move(move * moveSpeed * Time.deltaTime);
        player.Move(velocity * Time.deltaTime);

    }
}
