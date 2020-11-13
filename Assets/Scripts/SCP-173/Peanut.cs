using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Peanut : MonoBehaviour
{
    public Camera eyes;
    public Transform target;
    public Transform player;
    public float sightRange = 50f;

    public bool isBeingLookedAt;
    private NavMeshAgent agent;
    private bool seesPlayer;

    private int layerMask;
    private Fear playerFear;

    public AudioClip audioClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        isBeingLookedAt = false;
        agent = GetComponent<NavMeshAgent>();
        layerMask = 1 << 8;
        layerMask = ~layerMask;
        playerFear = player.GetComponent<Fear>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 screenPoint = eyes.WorldToViewportPoint(target.position);
        seesPlayer = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (isBeingLookedAt)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            playerFear.invokeFear();
        } else
        {
            agent.isStopped = false;
            if (seesPlayer)
            {
                RaycastHit hit;
                Debug.DrawRay(eyes.transform.position, target.position - eyes.transform.position, Color.red);

                if (Physics.Raycast(eyes.transform.position, target.position - eyes.transform.position, out hit, sightRange, layerMask))
                {
                    if (hit.collider.GetComponent<PlayerController>())
                    {
                        if (!audioSource.isPlaying)
                            audioSource.Play();
                        playerFear.invokeFear();
                        agent.SetDestination(target.position);
                    }
                }
            }
        }
    }
}
