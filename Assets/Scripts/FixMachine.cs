using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class FixMachine : MonoBehaviour
{
    private Camera playerCam;
    private float distance = 4f;
    public Text interactText;
    public LayerMask layerMask;
    public GameObject connectWires;
    private Wiretask wireTask;
    private bool interactTextState = false;
    private bool inMenu = false;
    private GameObject player;
    public Text artifactCountText;
    [SerializeField] private GameObject solaireShopMenu;

    public AudioSource handsAudio;

    public int artifactCount;

    private PlayerController playerManager;

    // Progress Bar controller
    public Image progressBar;
    private bool progressBarState = false;
    private int totalArtifactCount;
    public GenerateArtifacts artifactController;

    // Start is called before the first frame update
    void Start()
    {
        artifactCount = 0;
        playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        interactText.gameObject.SetActive(interactTextState);
        player = GameObject.FindWithTag("Player");

        wireTask = connectWires.GetComponent<Wiretask>();

        playerManager = GetComponent<PlayerController>();

        totalArtifactCount = artifactController.numArtifacts;

        artifactCountText.text = artifactCount.ToString() + "/" + totalArtifactCount;
    }

    // Update is called once per frame
    void Update()
    {
        PickUpFromRay();
    }

    private void PickUpFromRay()
    {
        Ray ray = playerCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        // Can make this alot cleaner
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            interactTextState = true;
            if (hit.collider.GetComponent<Arcade>())
            {
                if (inMenu)
                    interactTextState = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    connectWires.gameObject.SetActive(true);
                    wireTask.resetWires();
                    inMenu = true;
                    player.GetComponent<PlayerController>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else if (hit.collider.GetComponent<Artifact>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    handsAudio.Play();
                    artifactCount++;
                    artifactCountText.text = artifactCount.ToString() + "/" + totalArtifactCount;
                    hit.collider.gameObject.SetActive(false);
                    interactTextState = false;
                }
            }
            else if (hit.collider.GetComponent<Spider>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Spider spider = hit.collider.GetComponent<Spider>();
                    spider.kill();
                    playerManager.modifyMoney(spider.bounty);
                    interactTextState = false;
                }
            }
            else if (hit.collider.GetComponent<PizzaMaker>())
            {
                PizzaMaker pizzaMaker = hit.collider.GetComponent<PizzaMaker>();
                progressBarState = true;
                progressBar.fillAmount = (pizzaMaker.progress % 60) / pizzaMaker.taskTime;
                if (Input.GetKey(KeyCode.E))
                {
                    pizzaMaker.makePizza();

                    if (pizzaMaker.progress % 60 >= pizzaMaker.taskTime)
                    {
                        //playerManager.modifyMoney(pizzaMaker.bounty);
                        //pizzaMaker.finishPizza();
                        pizzaMaker.progress = 0;
                        this.gameObject.GetComponent<PlayerController>().addPizza();
                        interactTextState = false;
                        progressBarState = false;
                    }

                }
            }
            else if (hit.collider.GetComponent<PizzaDeleter>())
            {
                PizzaDeleter pizzaMaker = hit.collider.GetComponent<PizzaDeleter>();
                progressBarState = true;
                progressBar.fillAmount = (pizzaMaker.progress % 60) / pizzaMaker.taskTime;
                print("Destry");
                if (Input.GetKey(KeyCode.E))
                {
                    pizzaMaker.makePizza();

                    if (pizzaMaker.progress % 60 >= pizzaMaker.taskTime)
                    {
                        playerManager.modifyMoney(pizzaMaker.bounty);
                        pizzaMaker.finishPizza();
                        this.gameObject.GetComponent<PlayerController>().addPizza();
                        interactTextState = false;
                        progressBarState = false;
                    }

                }
            }
            else if (hit.collider.GetComponent<Solaire>())
            {
                if (inMenu)
                    interactTextState = false;
                if (Input.GetKey(KeyCode.E))
                {
                    solaireShopMenu.gameObject.SetActive(true);
                    inMenu = true;
                    player.GetComponent<PlayerController>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else if (hit.collider.GetComponent<Glass>())
            {
                Glass glass = hit.collider.GetComponent<Glass>();
                progressBarState = true;
                progressBar.fillAmount = (glass.progress % 60) / glass.taskTime;
                if (Input.GetKey(KeyCode.E))
                {
                    glass.pickupGlass();

                    if (glass.progress % 60 >= glass.taskTime)
                    {
                        playerManager.modifyMoney(glass.bounty);
                        glass.finishGlass();
                    }

                }
                else
                {
                    glass.resetGlass();
                }
            }
        }

        interactText.gameObject.SetActive(interactTextState);
        progressBar.gameObject.SetActive(progressBarState);
        interactTextState = false;
        progressBarState = false;
    }

    void updateArtifactCount()
    {
        totalArtifactCount = artifactController.numArtifacts;
    }
    public void leaveMenu()
    {
        inMenu = false;
    }
}
