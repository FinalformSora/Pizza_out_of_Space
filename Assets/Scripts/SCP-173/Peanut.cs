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
    public float hitRadius = 2f;

    public bool isBeingLookedAt;
    private NavMeshAgent agent;
    private bool seesPlayer;
    private float distanceToTarget;

    private int layerMask;
    private Fear playerFear;

    public AudioClip audioClip;
    private AudioSource audioSource;

    public bool attracted = false;
    public bool repelled = false;
    public bool endGame = false;
    public bool trapped = false;

    [SerializeField] float damage = 40f;
    [SerializeField] Transform hold;

    // Start is called before the first frame update
    void Start()
    {
        hold = GameObject.FindGameObjectWithTag("MainCamera").transform;
        target = hold;
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
        if (endGame)
        {
            distanceToTarget = Vector3.Distance(hold.position, transform.position);
            agent.SetDestination(hold.position);
            if(distanceToTarget <= 5)
            {
                hold.GetComponentInParent<CodyHealth>().TakeDamage(damage);
            }
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {

            if (target == null)
            {
                target = hold;
                attracted = false;
                repelled = false;
            }

            Vector3 screenPoint = eyes.WorldToViewportPoint(target.position);
            seesPlayer = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (isBeingLookedAt || trapped)
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                playerFear.invokeFear();
            }
            else
            {
                agent.isStopped = false;
                if (attracted)
                {
                    agent.SetDestination(target.position);
                }
                else if (repelled)
                {
                    agent.isStopped = true;
                }
                else if (seesPlayer)
                {
                    attackPlayer();
                }
            }

            isBeingLookedAt = false;
        }
    }

    private void attackPlayer()
    {
        RaycastHit hit;
        Debug.DrawRay(eyes.transform.position, target.position - eyes.transform.position, Color.red);

        if (Physics.Raycast(eyes.transform.position, target.position - eyes.transform.position, out hit, sightRange, layerMask))
        {

            if (hit.collider.GetComponent<PlayerController>())
            {
                if (hit.distance < 2f)
                {
                    hit.collider.GetComponent<CodyHealth>().TakeDamage(damage);
                }
                if (!audioSource.isPlaying)
                    audioSource.Play();
                playerFear.invokeFear();
                agent.SetDestination(target.position);
            }
        }
    }
}
