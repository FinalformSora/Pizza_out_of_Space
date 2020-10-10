using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactCollection : MonoBehaviour
{

    public Transform playerCam;
    public float rcDistance = 4f;
    public Text artifactCountText;
    public Text artifactCollectText;
    public LayerMask layerMask;

    private bool interactTextState = false;

    private int artifactCount;

    // Start is called before the first frame update
    void Start()
    {
        artifactCount = 0;
        artifactCollectText.gameObject.SetActive(interactTextState);
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(playerCam.position, playerCam.TransformDirection(Vector3.forward), out hit, rcDistance, layerMask))
        {
            // Debug.DrawRay(playerCam.position, playerCam.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

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

        artifactCollectText.gameObject.SetActive(interactTextState);
    }
}
