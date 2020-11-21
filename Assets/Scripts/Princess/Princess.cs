using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Princess : MonoBehaviour
{

    public GameObject player;
    public NavMeshAgent agent;
    public Animator animator;

    private Transform pTransform;

    private Fear playerFear;

    // How many times can she attack every second.
    public float attackInterval;
    // How long she attacks for.
    public float attackTime;

    public float angryTime;
    private float angryTimer;
    public float angrySpeedMultiplier = 3;

    // Keeps track of attacking
    private float attackTimer;
    public bool chasing;
    private bool moving;
    private float attackLengthTimer;

    // Distance multiplier
    private float distanceMultiplier;
    private Vector3 scale;
    private PlayerController pc;

    // Controls mouth audio
    private AudioSource mouth;

    // Controls movement audio
    public AudioSource feet;
    public AudioSource whispers;
    public AudioClip scream;
    public AudioClip crying;
    public AudioClip feetNormal;
    public AudioClip feetRunning;

    private bool angry;

    private float defaultSpeed;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        chasing = false;
        attackLengthTimer = 0;

        pTransform = player.transform;

        playerFear = player.GetComponent<Fear>();

        distanceMultiplier = 1;

        scale = agent.transform.localScale;

        pc = player.GetComponent<PlayerController>();
        mouth = GetComponent<AudioSource>();

        angry = false;
        angryTimer = 0;
        defaultSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        moving = agent.velocity != Vector3.zero;
        animator.SetBool("moving", moving);
        if (moving && !feet.isPlaying)
        {
            feet.Play();
        }

        if (chasing)
        {
            if (!mouth.isPlaying && mouth.clip == crying)
                mouth.Play();

            if (!whispers.isPlaying)
                whispers.Play();

            if (angry && angryTimer % 60 <= angryTime)
            {
                angryTimer += Time.deltaTime;
            } else
            {
                angry = false;
                animator.SetBool("angry", angry);
                angryTimer = 0;
                mouth.clip = crying;
                feet.clip = feetNormal;
            }

            agent.speed = angry ? defaultSpeed * angrySpeedMultiplier: defaultSpeed;
            agent.acceleration = agent.speed * 10;

            Chase();

            if (attackLengthTimer % 60 >= attackTime)
            {
                mouth.Stop();
                whispers.Stop();
                chasing = false;
                playerFear.StartFlicker();
                attackLengthTimer = 0;
            }

            attackLengthTimer += Time.deltaTime;
        }
        // Starts the attack
        else if (!chasing && attackTimer % 60 >= attackInterval)
        {
            agent.transform.localScale = scale;
            distanceMultiplier = 100 - playerFear.fear;
            chasing = true;
            attackTimer = 0;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pTransform.position - (pTransform.forward * distanceMultiplier), out hit, 1000f, NavMesh.AllAreas))
            {
                Stalk(hit.position);
            }
        } else
        {
            feet.Stop();
            agent.velocity = Vector3.zero;
            agent.transform.localScale = Vector3.zero;
            attackTimer += Time.deltaTime;
        }

        agent.isStopped = !chasing;
    }

    private void Chase()
    {
        agent.SetDestination(pTransform.transform.position);
        playerFear.invokeFear();
    }

    // Teleports behind the player
    private void Stalk(Vector3 position)
    {
        agent.Warp(position);
    }

    public void getAngry()
    {
        if (!angry)
        {
            angry = true;
            animator.SetBool("angry", angry);
            mouth.Stop();
            mouth.clip = scream;
            mouth.Play();
            feet.Stop();
            feet.clip = feetRunning;
        }
    }

}
