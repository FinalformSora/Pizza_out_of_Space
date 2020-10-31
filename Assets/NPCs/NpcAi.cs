using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class NpcAi : MonoBehaviour
{
    [SerializeField] int forceCase;
    [SerializeField] Transform setLocation;
    [SerializeField] Transform setLocation1;
    [SerializeField] Transform setLocation2;
    [SerializeField] Transform setLocation3;
    [SerializeField] Transform setLocation4;
    [SerializeField] GameObject[] arcadeMachines;
    [SerializeField] GameObject[] walkAroundLocations;

    NavMeshAgent navMeshAgent;

    float distanceToLocation = Mathf.Infinity;
    float distanceToArcade = Mathf.Infinity;

    float turnSpeed = 5;

    bool arcadeMood, frontDesk, prizeDesk, arcadeGameMood, walkAroundMood;
    bool arcadeAvailable = false;
    bool walkDestinationAvailable = true;

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
        if(walkAroundMood)
        {
            WalkAroundEstablishment();
        }
    }

    private void WalkAroundEstablishment()
    {
        System.Random rnd = new System.Random();
        if(walkDestinationAvailable)
        {
            int num = 0;
            num = rnd.Next(1, 15);
            setLocation = walkAroundLocations[num].transform;
            //print("the cube selected " + setLocation.name);
            navMeshAgent.SetDestination(setLocation.position);
            walkDestinationAvailable = false;
        }
        //print("Final Destination " + distanceToLocation);
        if(distanceToLocation <= .42)
        {
            walkDestinationAvailable = true;
        }
    }

    private void PlayGameAnimation()
    {
        //distanceToArcade = Vector3.Distance(setLocation.position, transform.position);
        isMoving = true;
        navMeshAgent.SetDestination(currentArcade.position);
        distanceToArcade = Vector3.Distance(currentArcade.position, transform.position);
        print("Arcade " + distanceToArcade);
        if (distanceToArcade <= 3)
        {
            navMeshAgent.isStopped = true;
            GetComponent<Animator>().SetBool("gameing", true);
            isMoving = false;
        }
    }   
    
    private void SetIdleUponDestinationArcade(float distanceToLocation)
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
            //print("Arcade " + arcadeMachines[i]);
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
        num = rnd.Next(4, 5);

        arcadeMood = false;
        frontDesk = false;
        prizeDesk = false;
        arcadeGameMood = false;
        walkAroundMood = false;

        //num = forceCase;
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
                //print("Walk Around");
                walkAroundMood = true;
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

    private void ForceArcadeMood()
    {
        arcadeMood = true;
        frontDesk = false;
        prizeDesk = false;
        arcadeGameMood = false;
        walkAroundMood = false;
    }

    private void ForceWalkAroundMood()
    {
        arcadeMood = true;
        frontDesk = false;
        prizeDesk = false;
        arcadeGameMood = false;
        walkAroundMood = true;
    }



}
