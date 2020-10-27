using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAi : MonoBehaviour
{
    [SerializeField] Transform setLocation;
    [SerializeField] Transform setLocation1;
    [SerializeField] Transform setLocation2;
    [SerializeField] Transform setLocation3;
    [SerializeField] Transform setLocation4;
    [SerializeField] GameObject[] arcadeMachines;

    NavMeshAgent navMeshAgent;

    float distanceToLocation = Mathf.Infinity;
    float turnSpeed = 9;

    bool arcadeMood, frontDesk, prizeDesk, arcadeGameMood;
    bool arcadeAvailable = false;

    bool isMoving = true;

    Transform currentArcade;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GettingState();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToLocation = Vector3.Distance(setLocation.position, transform.position);
        GetComponent<Animator>().SetBool("move", isMoving);

        //Debug.Log("Dist is " + distanceToLocation);
        if (arcadeMood)
        {
            //Debug.Log("Arcade");
            navMeshAgent.SetDestination(setLocation.position);
            SetIdleUponDestinationArcade(distanceToLocation);
        }
        if (frontDesk)
        {
            //Debug.Log("Needs tikets");
            navMeshAgent.SetDestination(setLocation.position);
            SetIdleUponDestination(distanceToLocation);
            FaceTargetXpos();
        }
        if (prizeDesk)
        {
            //Debug.Log("Selling Toys");
            navMeshAgent.SetDestination(setLocation.position);
            SetIdleUponDestination(distanceToLocation);
            FaceTargetXneg();
        }
        if(arcadeGameMood)
        {
            //print("Chose an arcade");
            PlayGameAnimation();
        }
    }

    private void PlayGameAnimation()
    {
        //distanceToArcade = Vector3.Distance(setLocation.position, transform.position);
        isMoving = true;
        navMeshAgent.SetDestination(currentArcade.position);
        if (distanceToLocation <= 2.3)
        {
            isMoving = false;
        }
    }    private void SetIdleUponDestinationArcade(float distanceToLocation)
    {
        if (distanceToLocation <= .4)
        {
            //GetComponent<Animator>().SetBool("idle", true);
            isMoving = false;
            arcadeAvailable = true;
            CheckAvailableArcade();
        }
    }

    private void CheckAvailableArcade()
    {
        int i = 0;
        while(arcadeAvailable)
        {
            print("Arcade " + arcadeMachines[i]);
            if(arcadeMachines[i].tag == "Available")
            {
                arcadeAvailable = false;
                
                currentArcade = arcadeMachines[i].transform;
                setLocation = currentArcade.transform;
                //navMeshAgent.SetDestination(arcadeMachines[i].transform.position);
                arcadeMachines[i].tag = "Unavailable";
                arcadeMood = false;
                arcadeGameMood = true;
            }
            i++;
        }
    }

    private void SetIdleUponDestination(float distanceToLocation)
    {
        if (distanceToLocation <= .4)
        {
            isMoving = false;
        }
    }

    private void GettingState()
    {
        System.Random rnd = new System.Random();
        int num = 0;
        num = rnd.Next(1, 4);

        arcadeMood = false;
        frontDesk = false;
        prizeDesk = false;
        arcadeGameMood = false;
        
        switch (num)
        {
            case 1:
                arcadeMood = true;
                setLocation = setLocation1;
                break;
            case 2:
                frontDesk = true;
                setLocation = setLocation2;
                break;
            case 3:
                prizeDesk = true;
                setLocation = setLocation3;
                break;
            case 4:
                setLocation = setLocation4;
                break;
        }
    }

    private void FaceTargetXneg()
    {
        Vector3 direction = (setLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x - 1, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    private void FaceTargetXpos()
    {
        Vector3 direction = (setLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 1, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    private void FaceTargetZneg()
    {
        Vector3 direction = (setLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}
