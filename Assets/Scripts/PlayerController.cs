using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

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

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private bool isPaused = false;
    [SerializeField] public bool isGameOver = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GetComponent<CharacterController>();
        eyes = GameObject.FindWithTag("MainCamera");
        light = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Light>();
        light.enabled = lighton;

        // Money
        money = startMoney;
        updateMoneyUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (slowed == false)
        {
            WalkNormal();
        }
        else if (slowed == true)
        {
            SlowTarget(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused && !isGameOver)
        {
            ActivateMenu();
        }
        else if (!isPaused && !isGameOver)
        {
            DeactivateMenu();
        }
        else if (isGameOver)
        {
            HandleDeath();
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
        }
        else
        {
            walking = false;
            idle = true;
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
    }

    public void SlowTarget(float slowSpeed)
    {

        player.Move(new Vector3(0, 0, 0) * Time.deltaTime);

    }

    public void modifyMoney(int amount)
    {
        money += amount;
        updateMoneyUI();
        makeMoneySound();
    }

    void updateMoneyUI()
    {
        moneyValueText.text = "$ " + money;
    }

    void makeMoneySound()
    {
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

    void HandleDeath()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverCanvas.SetActive(true);
    }
}


