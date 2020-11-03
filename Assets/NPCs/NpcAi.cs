﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class NpcAi : MonoBehaviour
{
    [SerializeField] int forceCase;
    [SerializeField] Transform setLocation;
    [SerializeField] GameObject[] frontDeskLine;
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

    public enum MoodSates { arcadeMood, frontDesk, prizeDesk, arcadeGameMood, walkAroundMood };
    bool arcadeAvailable = false;
    bool walkDestinationAvailable = true;
    bool isMoving = true;
    bool reached = false;

    Transform currentArcade;

    public MoodSates state;
    // Time components for walkTimer()
    float timer = 0;
    public int secs;

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
       //enum MoodSates { arcadeMood, frontDesk, prizeDesk, arcadeGameMood, walkAroundMood };

        switch(state)
        {
            case MoodSates.arcadeMood:
                //Debug.Log("Arcade");
                navMeshAgent.SetDestination(setLocation.position);
                SetIdleUponDestinationArcade(distanceToLocation);
                break;
            case MoodSates.frontDesk:
                //Debug.Log("Needs tikets");
                navMeshAgent.SetDestination(setLocation.position);
                if (!reached)
                {
                    SetIdleUponDestination(distanceToLocation);
                }
                FaceTargetXpos();
                break;
            case MoodSates.prizeDesk:
                //Debug.Log("Selling Toys");
                navMeshAgent.SetDestination(setLocation.position);
                if(!reached)
                {
                    SetIdleUponDestination(distanceToLocation);
                }
                FaceTargetXneg();
                break;
            case MoodSates.arcadeGameMood:
                //print("Chose an arcade");
                PlayGameAnimation();
                break;
            case MoodSates.walkAroundMood:
                WalkAroundEstablishment();
                StateChangeTimer();
                break;
        }
        
    }

    private void WalkAroundEstablishment()
    {
        isMoving = true;
        System.Random rnd = new System.Random();
        print(distanceToLocation);
        if(walkDestinationAvailable)
        {
            int num = 0;
            num = rnd.Next(0, 14);
            setLocation = walkAroundLocations[num].transform;
            print("the cube selected " + setLocation.name);
            navMeshAgent.SetDestination(setLocation.position);
            walkDestinationAvailable = false;
            print("Going to my destination");
        }
        //print("Final Destination " + distanceToLocation);
        if(HasReachedWalkedAroundLocation())
        {
            print("Walked to my location");
            reached = true;
        }
    }

    private void PlayGameAnimation()
    {
        //distanceToArcade = Vector3.Distance(setLocation.position, transform.position);
        isMoving = true;
        navMeshAgent.SetDestination(currentArcade.position);
        distanceToArcade = Vector3.Distance(currentArcade.position, transform.position);
        //print("Arcade " + distanceToArcade);
        if (distanceToArcade <= 3)
        {
            navMeshAgent.isStopped = true;
            GetComponent<Animator>().SetBool("gameing", true);
            isMoving = false;
            StateChangeTimer();
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
                state = MoodSates.arcadeGameMood;
            }
            i++;
        }
    }

    private void SetIdleUponDestination(float distanceToLocation)
    {
        if (HasReachedLocation())
        {
            print("NPC got here");
            isMoving = false;
        }
    }

    
    private void GettingState()
    {
        System.Random rnd = new System.Random();
        int num = 0;
        num = rnd.Next(1, 5);

        //num = forceCase;
        switch (num)
        {
            case 1:
                state = MoodSates.arcadeMood;
                setLocation = setLocation1;
                break;
            case 2:
                state = MoodSates.frontDesk;
                FrontDeskLineMaker();
                break;
            case 3:
                state = MoodSates.prizeDesk;
                setLocation = setLocation3;
                break;
            case 4:
                //print("Walk Around");
                state = MoodSates.walkAroundMood;
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

    private bool HasReachedLocation()
    {
        if (distanceToLocation < .4)
        {
            reached = true;
            tag = "Unsatisfied";
            return true;
        }
        else
            return false;
    }

    private bool HasReachedWalkedAroundLocation()
    {
        if (distanceToLocation < .43)
        {
            walkDestinationAvailable = true;
            tag = "Unsatisfied";
            return true;
        }
        else
        {
            walkDestinationAvailable = false;
            return false;
        }
    }

    private void ForceWalkAroundMood()
    {
        state = MoodSates.walkAroundMood;
    }

    private void StateChangeTimer()
    {
        System.Random rnd = new System.Random();
        int num = 0;
        num = rnd.Next(2, 4);

        timer += Time.deltaTime;
        secs = (int)(timer % 60);
        int finalWalkTime = secs * num;
        Debug.Log("Timer " + secs);

        if (secs >= finalWalkTime)
        {
            timer = 0;
            GettingState();
        }

    }

    private void FrontDeskLineMaker()
    {
        foreach(GameObject line in frontDeskLine)
        {
            if(line.tag == "Available")
            {
                setLocation = line.transform;
                navMeshAgent.SetDestination(line.transform.position);
                line.tag = "Unavialable";
                break;
            }
        }   
    }
}