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
    private GameObject connectWires;
    private bool interactTextState = false;
    private GameObject player;
    public Text artifactCountText;

    private int artifactCount;

    // Start is called before the first frame update
    void Start()
    {
        artifactCount = 0;
        playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        interactText.gameObject.SetActive(interactTextState);
        connectWires = GameObject.FindWithTag("Wires");
        //connectWires.gameObject.SetActive(false);
        player = GameObject.FindWithTag("Player");
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

        if(Physics.Raycast(ray, out hit, distance, layerMask))
        {
            if (hit.collider.GetComponent<Arcade>())
            {
                interactTextState = true;
                if (Input.GetKeyDown(KeyCode.E)){
                    connectWires.gameObject.SetActive(true);
                    interactTextState = false;
                    player.GetComponent<PlayerController>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            if (hit.collider.GetComponent<Artifact>())
            {
                interactTextState = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    artifactCount++;
                    artifactCountText.text = artifactCount.ToString();
                    hit.collider.gameObject.SetActive(false);
                    interactTextState = false;
                }
            }
        }

        interactText.gameObject.SetActive(interactTextState);
        interactTextState = false;
    }
}
