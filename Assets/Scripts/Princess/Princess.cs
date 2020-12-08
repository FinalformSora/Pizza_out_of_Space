using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Princess : MonoBehaviour
{
    [SerializeField] Transform hold;

    public GameObject player;
    public NavMeshAgent agent;
    public Animator animator;

    public Transform pTransform;

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
    private CodyHealth pc_health;

    // Controls mouth audio
    private AudioSource mouth;

    // Controls movement audio
    public AudioSource feet;
    public AudioSource whispers;
    public AudioClip scream;
    public AudioClip crying;
    public AudioClip feetNormal;
    public AudioClip feetRunning;

    // Controls behavior
    private bool angry;
    private bool resting;

    // Keeps track of rest time
    private float restTimer;
    public float maxRestTime;

    private float defaultSpeed;
    private int layerMask;

    public float sightRange = 5f;

    public bool attracted = false;
    public bool repelled = false;
    public bool endGame = false;

    public bool trapped = false;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        chasing = false;
        attackLengthTimer = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        hold = player.transform;
        pTransform = player.transform;

        playerFear = player.GetComponent<Fear>();

        distanceMultiplier = 1;

        scale = agent.transform.localScale;

        pc = player.GetComponent<PlayerController>();
        pc_health = player.GetComponent<CodyHealth>();
        mouth = GetComponent<AudioSource>();

        angry = false;
        resting = false;
        angryTimer = 0;
        defaultSpeed = agent.speed;

        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {

        if (trapped)
        {
            agent.velocity = Vector3.zero;
        }

        if (endGame)
        {
            moving = true;
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


                // When she's done being angry
                angry = true;
                animator.SetBool("angry", angry);
                mouth.clip = crying;
                feet.clip = feetNormal;

                EndChase();
                agent.speed = angry ? defaultSpeed * angrySpeedMultiplier : defaultSpeed;
                agent.acceleration = agent.speed * 10;
                

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
            else
            {
                agent.transform.localScale = scale;
                distanceMultiplier = 100 - playerFear.fear + 10;
                chasing = true;
                attackTimer = 0;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(hold.position - (hold.forward * distanceMultiplier), out hit, 1000f, NavMesh.AllAreas))
                {
                    Stalk(hit.position);
                }
            }

            agent.isStopped = false;
        }
        else
        {
            if (pTransform == null)
            {
                pTransform = hold;
                attracted = false;
                repelled = false;
            }
            if (playerFear.fear < 25)
            {
                Disable();
                return;
            }

            moving = agent.velocity != Vector3.zero;
            animator.SetBool("moving", moving);
            if (moving && !feet.isPlaying)
            {
                feet.Play();
            }
            if (attracted)
            {
                ChaseAttracted();
            }
            else if (repelled)
            {
                Disable();
            }
            else if (chasing)
            {

                if (!mouth.isPlaying && mouth.clip == crying)
                    mouth.Play();

                if (!whispers.isPlaying)
                    whispers.Play();

                if (angry && angryTimer % 60 <= angryTime)
                {
                    angryTimer += Time.deltaTime;
                }
                else
                {
                    if (angry)
                    {
                        calmDown();
                    }
                    // When she's done being angry
                    angry = false;
                    animator.SetBool("angry", angry);
                    angryTimer = 0;
                    mouth.clip = crying;
                    feet.clip = feetNormal;
                }

                if (resting)
                {
                    agent.speed = 0;
                    agent.velocity = Vector3.zero;
                    restTimer += Time.deltaTime;
                    agent.isStopped = true;
                    if (restTimer % 60 >= maxRestTime)
                    {
                        resting = false;
                        animator.SetBool("resting", false);
                        restTimer = 0;
                        agent.isStopped = false;
                    }
                }
                else
                {
                    Chase();
                    agent.speed = angry ? defaultSpeed * angrySpeedMultiplier : defaultSpeed;
                    agent.acceleration = agent.speed * 10;
                }

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
            else if (resting)
            {
                chasing = false;
                restTimer += Time.deltaTime;
                agent.isStopped = true;

                if (restTimer % 60 >= maxRestTime)
                {
                    resting = false;
                    animator.SetBool("resting", false);
                    restTimer = 0;
                    chasing = true;
                }
            }
            // Starts the attack
            else if (!chasing && attackTimer % 60 >= attackInterval)
            {
                agent.transform.localScale = scale;
                distanceMultiplier = 100 - playerFear.fear + 10;
                chasing = true;
                attackTimer = 0;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(pTransform.position - (pTransform.forward * distanceMultiplier), out hit, 1000f, NavMesh.AllAreas))
                {
                    Stalk(hit.position);
                }
            }
            else
            {
                // Dissapear after attack time is over.
                Disable();
            }

            agent.isStopped = !chasing;
        }
    }
    private void EndChase()
    {
        agent.SetDestination(hold.transform.position);
        if (angry)
        {
            playerFear.invokeFear(3f);
        }
    }

    private void Chase()
    {
        agent.SetDestination(pTransform.transform.position);
        if (angry)
        {
            playerFear.invokeFear(3f);
        }
    }

    private void ChaseAttracted()
    {
        agent.SetDestination(pTransform.transform.position);
    }

    // Teleports behind the player
    private void Stalk(Vector3 position)
    {
        agent.Warp(position);
    }

    public void getAngry()
    {
        if (!angry && !resting)
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

    public void calmDown()
    {
        resting = true;
        animator.SetBool("resting", true);
        feet.Stop();
        agent.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == pc.GetComponent<Collider>())
        {
            pc_health.TakeDamage(1f);
        }
    }

    private void Disable()
    {
        resting = false;
        feet.Stop();
        agent.velocity = Vector3.zero;
        agent.transform.localScale = Vector3.zero;
        angry = false;
        attackLengthTimer = 0;
        attackTimer += Time.deltaTime;
    }
}
