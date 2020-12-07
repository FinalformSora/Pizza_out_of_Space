﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KlownAi : MonoBehaviour
{
    public Transform target;
    [SerializeField] Transform hold;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float captureRange = 2f;

    NavMeshAgent navMeshAgent;

    public float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    public bool attracted = false;
    public bool repelled = false;
    float timer = 30f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(target == null)
        {
            target = hold;
            isProvoked = false;
            attracted = false;
            repelled = false;
            navMeshAgent.isStopped = false;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked && (!attracted || !repelled))
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        if (attracted)
        {
            EngageAttract();
        }
        if (repelled)
        {
            isProvoked = false;
            navMeshAgent.isStopped = true;
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageAttract()
    {
        FaceTarget();
        ChaseTarget();
    }

    private void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= captureRange)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= captureRange)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;        
        GetComponent<Animator>().SetBool("attack", true);
        //Debug.Log(name + " has seeked and is destroying " + target.name);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    //Coats an area that provokes AI to chase
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, captureRange);
    }
}
