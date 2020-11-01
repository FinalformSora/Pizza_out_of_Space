using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private CharacterController player;
    private GameObject eyes;
    private Light light;

    private bool lighton = false;

    private float moveSpeed = 6f;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    private float mouseSensitivity = 100f;
    private float gravity = -29.4f;
    private float jumpHeight = 1.5f;
    private float moveX = 0f;
    private float moveZ = 0f;

    private float xRotation = 0f;
    private bool jumping = false;


    private bool walking = false;
    private bool running = false;
    private bool idle = true;
    public bool slowed = false;

    // Keeps track of money
    public int startMoney = 50;
    public Text moneyValueText;
    private int money;

    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    void Start()
    {
        animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<CharacterController>();
        eyes = GameObject.FindWithTag("MainCamera");
        light = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Light>();
        light.enabled = lighton;
        animator.SetBool("walking", walking);
        animator.SetBool("running", running);
        animator.SetBool("idle", true);

        // Money
        money = startMoney;
        updateMoneyUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(slowed==false)
        {
            WalkNormal();
        }
        else if(slowed == true)
        {
            SlowTarget(2);
        }
    }

    private void WalkNormal()
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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            walking = true;
            idle = false;
            animator.SetBool("walking", walking);
            animator.SetBool("idle", idle);
        }
        else
        {
            walking = false;
            idle = true;
            animator.SetBool("walking", walking);
            animator.SetBool("idle", idle);
        }
        player.Move(new Vector3(move.x * moveSpeed, velocity.y, move.z * moveSpeed) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            lighton = !lighton;
        }
        light.enabled = lighton;

        if (Input.GetKey(KeyCode.LeftShift) && walking)
        {
            moveSpeed = runSpeed;
            running = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            running = false;
        }
        animator.SetBool("running", running);
    }

    public void SlowTarget(float slowSpeed)
    {

        player.Move(new Vector3(0, 0, 0) * Time.deltaTime);

    }

    public void modifyMoney(int amount)
    {
        money += amount;
        updateMoneyUI();
    }

    void updateMoneyUI()
    {
        moneyValueText.text = "$ " + money;
    }
}


