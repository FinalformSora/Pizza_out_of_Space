using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KlownAi : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float captureRange = 2f;

    NavMeshAgent navMeshAgent;
    public AudioSource feet;
    public ZombieSounds zombieSounds;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    public bool trapped = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (trapped)
        {
            feet.Stop();
            navMeshAgent.velocity = Vector3.zero;
            GetComponent<Animator>().SetTrigger("idle");
            isProvoked = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

        if (navMeshAgent.velocity != Vector3.zero)
        {
            if (!feet.isPlaying)
            {
                feet.Play();
            }
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
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        zombieSounds.attackSound();
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

    public void setTrapped()
    {
        trapped = true;
    }

    public void release()
    {
        trapped = false;
    }
}
