using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Smash : MonoBehaviour
{

    public Camera playerCam;
    private float distance = 4f;
    public LayerMask layerMask;
    public Text interactText;
    private bool interactTextState = false;

    private bool dead = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("dead", dead);
        interactText.gameObject.SetActive(interactTextState);
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

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            if (hit.collider.gameObject.name.Contains("Spider"))
            {
                Debug.Log(hit.collider.gameObject.name);

                interactTextState = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetBool("dead", !dead);
                    interactTextState = false;
                }
            }
        }

        interactText.gameObject.SetActive(interactTextState);
        interactTextState = false;
    }
}
