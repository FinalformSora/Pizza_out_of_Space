using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public Transform pos;
    public NavMeshAgent agent;
    public float wanderRange = 10f;
    public float midRange;
    public Boolean forwardPreference = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent.destination = pos.position;
        midRange = wanderRange / 2;
    }

    // Update is called once per frame
    void Update()
    {
        agent.isStopped = animator.GetBool("dead");

        if (!agent.hasPath && !agent.isStopped)
        {
            Vector3 nextPosition = getRandomPos();
            pos.rotation = Quaternion.LookRotation(new Vector3(nextPosition.x, nextPosition.y, nextPosition.z));
            agent.SetDestination(nextPosition);
        }
    }
    
    Vector3 getRandomPos()
    {
        Vector3 randomPosition;
        float randomX = UnityEngine.Random.Range(midRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;
        float randomY = UnityEngine.Random.Range(midRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;
        float randomZ = UnityEngine.Random.Range(midRange, wanderRange) * UnityEngine.Random.Range(0, 2) * 2 - 1;

        if (forwardPreference)
        {
            randomPosition = pos.position + new Vector3(randomX, randomY, -randomZ);
        } else
        {
            randomPosition = pos.position + new Vector3(randomX, randomY, randomZ);
        }

        Debug.Log("position" + randomPosition);

        return randomPosition;
    }
}
