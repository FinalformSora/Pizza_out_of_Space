using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;
    private Transform tf;

    // How far can the spider wander from its current position
    public float wanderRange = 5f;
    public float minRange = 0f;

    public float destroyDelay = 5f;

    public int bounty = 20;

    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the spider around randomly if it's not dead.
        moveRandomly();
    }

    public void kill()
    {
        dead = true;
        animator.SetBool("dead", dead);
        agent.velocity = Vector3.zero;
        agent.isStopped = dead;

        this.gameObject.GetComponentInParent<DynamicSpiderCreation>().killASpider();

        Destroy(this.gameObject, destroyDelay);
    }

    void moveRandomly()
    {

        if (!agent.hasPath && !dead)
        {
            Vector3 nextPosition = getRandomPos();
            tf.rotation = Quaternion.LookRotation(new Vector3(nextPosition.x, nextPosition.y, nextPosition.z));
            agent.SetDestination(nextPosition);
        }
    }

    Vector3 getRandomPos()
    {
        Vector3 randomPosition;
        float randomX = UnityEngine.Random.Range(minRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;
        float randomY = UnityEngine.Random.Range(minRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;
        float randomZ = UnityEngine.Random.Range(minRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;

        randomPosition = tf.position + new Vector3(randomX, randomY, -randomZ);

        return randomPosition;
    }

}
