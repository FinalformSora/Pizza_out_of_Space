using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sirenHeadAi : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform setLocation;
    [SerializeField] Transform setLocation1;
    [SerializeField] Transform setLocation2;
    [SerializeField] Transform setLocation3;
    [SerializeField] Transform setLocation4;
    [SerializeField] Transform setLocation5;
    [SerializeField] Transform setLocation6;

    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float captureRange = 2f;

    NavMeshAgent navMeshAgent;

    float distanceToTarget = Mathf.Infinity;
    float distanceToLocation = Mathf.Infinity;
    float timer = 0;
    public int secs;

    bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        distanceToLocation = Vector3.Distance(setLocation.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        else if (distanceToTarget > 8)
        {
            WalkPath();
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
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
        if (distanceToLocation >= chaseRange)
        {
            timer += Time.deltaTime;
            secs = (int)(timer % 60);
            Debug.Log("Timer " + secs);
            if (secs >= 10)
            {
                isProvoked = false;
                timer = 0;
                secs = 0;
            }
        }
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void WalkPath()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(setLocation.position);
        int num = 0;
        //Debug.Log(distanceToLocation);
        if(distanceToLocation <= 2)
        {
            System.Random rnd = new System.Random();
            num = rnd.Next(1, 7);
            //Debug.Log("The rando " + num);
            switch(num)
            {
                case 1:
                    setLocation = setLocation1;
                    break;
                case 2:
                    setLocation = setLocation2;
                    break;
                case 3:
                    setLocation = setLocation3;
                    break;
                case 4:
                    setLocation = setLocation4;
                    break;
                case 5:
                    setLocation = setLocation5;
                    break;
                case 6:
                    setLocation = setLocation6;
                    break;
            }
        }
    }

    private void AttackTarget()
    {
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
