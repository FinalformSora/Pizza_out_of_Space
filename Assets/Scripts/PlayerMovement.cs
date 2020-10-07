using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float jumpSpeed = 400f;
    public float gravity = 9.8f;

    private float vSpeed = 0f;

    // Movespeed
    public float speed = 12f;

    public float runspeed = 24f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            vSpeed = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed * Time.fixedDeltaTime;
            }
        }

        vSpeed -= gravity * Time.fixedDeltaTime;

        Vector3 movement;

        if (Input.GetKey(KeyCode.LeftShift)) {
            movement = transform.right * x * runspeed * Time.deltaTime + transform.forward * z * runspeed * Time.deltaTime;
        } else
        {
            movement = transform.right * x * speed * Time.deltaTime + transform.forward * z * speed * Time.deltaTime;
        }

        movement.y = vSpeed * Time.fixedDeltaTime;

        controller.Move(movement);
    }
}
