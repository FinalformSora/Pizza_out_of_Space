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
    private Light flashlight;
    private Inventory inventory;

    public bool lighton = false;

    private float moveSpeed = 6f;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    private float mouseSensitivity = 100f;
    private float gravity = -29.4f;
    private float jumpHeight = 1.5f;
    private float moveX = 0f;
    private float moveZ = 0f;
    private float batteryLife = 100.0f;
     

    private float xRotation = 0f;
    private bool jumping = false;

 
    private bool walking = false;
    private bool running = false;
    private bool idle = true;
    public bool slowed = false;

    public AudioSource flashlightAudio;
    public AudioSource moneySound;

    // Keeps track of money
    public int startMoney = 50;
    public Text moneyValueText;
    private int money;

    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private bool isPaused = false;
    [SerializeField] public bool isGameOver = false;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject attractSpray;
    [SerializeField] private GameObject repelSpray;
    [SerializeField] private GameObject trapSet;
    [SerializeField] private GameObject itemSpawn;
    [SerializeField] private GameObject batteryLifeBar;

    void Start()
    {
        animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GetComponent<CharacterController>();
        eyes = GameObject.FindWithTag("MainCamera");
        flashlight = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Light>();
        flashlight.enabled = lighton;

        // Money
        money = startMoney;
        updateMoneyUI();
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        if (lighton && !isPaused)
        {
            batteryLife -= Time.deltaTime;
            if(batteryLife <= 0)
            {
                flashlightAudio.Play();
                lighton = false;
                flashlight.enabled = lighton;
                batteryLife = 0.0f;
            }
            batteryLifeBar.transform.localScale = new Vector3(batteryLife / 100f, 1, 1);
        }
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

        if (Input.GetMouseButtonDown(1))
        {
            int i = uiInventory.index % inventory.GetItemList().Count;
            if (inventory.GetItemList()[i].amount > 0 && inventory.GetItemList()[i].itemType != Item.ItemType.Pizza)
            {
                inventory.GetItemList()[i].amount--;
                useItem(inventory.GetItemList()[i]);
            }
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

        if (Input.GetKeyDown(KeyCode.F) && batteryLife > 0)
        {
            flashlightAudio.Play();
            lighton = !lighton;
            flashlight.enabled = lighton;
        }

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
        moneySound.Play();
        updateMoneyUI();
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
        AudioListener.pause = false;
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

    public void addAttract()
    {
        if (money >= 35)
        {
            Item item = new Item { itemType = Item.ItemType.Attract };
            inventory.AddItem(item);
            modifyMoney(-35);
        }
    }

    public void addRepel()
    {
        if (money >= 30)
        {
            Item item = new Item { itemType = Item.ItemType.Repel };
            inventory.AddItem(item);
            modifyMoney(-30);
        }
    }
    public void addTrap()
    {
        if (money >= 20)
        {
            Item item = new Item { itemType = Item.ItemType.Trap };
            inventory.AddItem(item);
            modifyMoney(-20);
        }
    }
    public void addBattery()
    {
        if (money >= 5)
        {
            Item item = new Item { itemType = Item.ItemType.Battery };
            inventory.AddItem(item);
            modifyMoney(-5);
        }
    }

    public void addPills()
    {
        if(money >= 10)
        {
            Item item = new Item { itemType = Item.ItemType.Pills };
            inventory.AddItem(item);
            modifyMoney(-10);
        }
    }

    public void addPizza()
    {
        Item item = new Item { itemType = Item.ItemType.Pizza };
        inventory.AddItem(item);
    }

    private void useItem(Item x)
    {
        switch (x.itemType)
        {
            case Item.ItemType.Attract: useAttract(); break;
            case Item.ItemType.Repel: useRepel();  break;
            case Item.ItemType.Trap: useTrap(); break;
            case Item.ItemType.Battery: useBattery();  break;
            case Item.ItemType.Pills: usePills(); break;
        }
    }

    private void useAttract()
    {
        Instantiate(attractSpray, new Vector3(itemSpawn.transform.position.x, 
            itemSpawn.transform.position.y, itemSpawn.transform.position.z), Quaternion.identity);
    }
    private void useRepel()
    {
        Instantiate(repelSpray, new Vector3(itemSpawn.transform.position.x,
            itemSpawn.transform.position.y, itemSpawn.transform.position.z), Quaternion.identity);
    }
    private void useTrap()
    {
        Instantiate(trapSet, new Vector3(itemSpawn.transform.position.x,
            itemSpawn.transform.position.y, itemSpawn.transform.position.z), Quaternion.identity);
    }
    private void useBattery()
    {
        batteryLife += 50.0f;
        if (batteryLife > 100.0f)
            batteryLife = 100.0f;
        batteryLifeBar.transform.localScale = new Vector3(batteryLife / 100f, 1, 1);
    }

    private void usePills()
    {
        Fear x = gameObject.GetComponent<Fear>();
        x.fear -= 50;
        if(x.fear < 0)
        {
            x.fear = 0;
        }

    }
}