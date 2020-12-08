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
    //[SerializeField] Transform setLocation4;
    [SerializeField] GameObject[] frontDeskLine;
    [SerializeField] GameObject[] arcadeMachines;
    [SerializeField] GameObject[] walkAroundLocations;
    [SerializeField] GameObject[] availableTables;
    [SerializeField] GameObject[] steelTables;
    [SerializeField] GameObject[] prizeLineMaker;

    [SerializeField] Transform pizza;
    [SerializeField] Transform pizzaSpaner;

    NavMeshAgent navMeshAgent;

    float distanceToLocation = Mathf.Infinity;
    float distanceToArcade = Mathf.Infinity;

    float turnSpeed = 20;

    public enum MoodSates { arcadeMood, frontDesk, prizeDesk, arcadeGameMood, walkAroundMood, pizzaMood, sittingDown, waitingForService };
    bool arcadeAvailable = false;
    bool walkDestinationAvailable = true;
    bool isMoving = true;
    bool reached = false;

    //Components fr Arcade
    Transform currentArcade;
    int arcadeIndex = 0;

    //Components for FontLine
    GameObject lineController;
    int lineControllerIndex;

    public MoodSates state;
    // Time components for walkTimer()
    float minutes = 5;

    //Components for SittingDown
    public int availableTableIndex = 0;
    private GameObject table;
    bool goToWoodTable = false;
    float pizzaCounter = 0;
    float pizzaEatingTime = 10;

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

        switch (state)
        {
            case MoodSates.arcadeMood:
                /*                print(setLocation1);
                */
                navMeshAgent.SetDestination(setLocation.position);
                SetIdleUponDestinationArcade(distanceToLocation);
                minutes = 10;
                break;
            case MoodSates.frontDesk:
                //Debug.Log("Needs tikets");
                navMeshAgent.SetDestination(setLocation.position);
                if (!reached)
                {
                    SetIdleUponDestination(distanceToLocation);
                }
                else
                {
                    WaitForHelp();
                }
                FaceTargetXpos();
                break;
            case MoodSates.prizeDesk:
                //Debug.Log("Selling Toys");
                navMeshAgent.SetDestination(setLocation.position);
                if (!reached)
                {
                    SetIdleUponDestination(distanceToLocation);
                }
                else
                {
                    WaitForHelpPrizeDesk();
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
            case MoodSates.pizzaMood:
                if (!reached)
                {
                    if (goToWoodTable)
                    {
                        GetToTable();
                    }
                    else if (!goToWoodTable)
                    {
                        GetToSteelTable();
                    }
                }
                else if (reached)
                {
                    //navMeshAgent.isStopped = true;
                    GetComponent<Animator>().SetBool("sitting", true);
                    isMoving = false;
                    SittingDown();
                }
                break;
            case MoodSates.sittingDown:
                {
                    if ((availableTableIndex % 2) == 0)
                    {
                        FaceTargetZneg();
                        transform.position = transform.position + new Vector3(0, 0, .1f);
                    }
                    else
                    {
                        FaceTargetZpos();
                        transform.position = transform.position - new Vector3(0, 0, .1f);
                    }
                    //transform.position = transform.position + new Vector3(0, .2f, 0);
                    WaitingForService();
                    break;
                }
            case MoodSates.waitingForService:
                {
                    if ((availableTableIndex % 2) == 0)
                    {
                        FaceTargetZneg();
                    }
                    else
                    {
                        FaceTargetZpos();
                    }
                    if (gameObject.tag == "Satisfied")
                    {
                        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pizza.x - 1, 0, direction.z));
                        if (pizzaCounter == 0)
                        {
                            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);
                            Transform newPizza = PizzaDeleter.Instantiate(pizza, pizzaSpaner.position, spawnRotation);
                            newPizza.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                            pizzaCounter = 1;

                        }
                        if (pizzaCounter == 1)
                        {
                            pizzaEatingTime -= Time.deltaTime;
                            if (pizzaEatingTime <= 0)
                            {
                                ForceWalkAroundMood();
                                pizzaCounter = 0;
                                minutes = 60;
                                pizzaEatingTime = 10;
                                if (table.layer == 12)
                                {
                                    print("The index " + availableTableIndex);
                                    availableTables[availableTableIndex].tag = "Available";
                                    availableTableIndex = 0;
                                }
                                else
                                {
                                    steelTables[availableTableIndex].tag = "Available";
                                    availableTableIndex = 0;
                                }
                            }


                        }
                    }
                    break;
                }
        }

    }

    private void WaitForHelp()
    {
        GetComponent<Animator>().SetBool("idle", true);
        if (gameObject.tag == "Satisfied")
        {
            ForceWalkAroundMood();
            frontDeskLine[lineControllerIndex].tag = "Available";
            lineControllerIndex = 0;
        }
        else
        {
            if (frontDeskLine[lineControllerIndex - 1].tag == "Available")
            {
                isMoving = true;
                setLocation = frontDeskLine[lineControllerIndex - 1].transform;
                frontDeskLine[lineControllerIndex - 1].tag = "Unavailable";
                frontDeskLine[lineControllerIndex].tag = "Available";
                lineControllerIndex--;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    private void WaitForHelpPrizeDesk()
    {
        GetComponent<Animator>().SetBool("idle", true);
        if (gameObject.tag == "Satisfied")
        {
            ForceWalkAroundMood();
            prizeLineMaker[lineControllerIndex].tag = "Available";
            lineControllerIndex = 0;
        }
        else
        {
            if (prizeLineMaker[lineControllerIndex - 1].tag == "Available")
            {
                isMoving = true;
                setLocation = prizeLineMaker[lineControllerIndex - 1].transform;
                prizeLineMaker[lineControllerIndex - 1].tag = "Unavailable";
                prizeLineMaker[lineControllerIndex].tag = "Available";
                lineControllerIndex--;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    private void SelectALocation()
    {
        System.Random rnd = new System.Random();
        int locationTheme = rnd.Next(1, 3);
        if (locationTheme == 1)
        {
            print("Going wood");
            goToWoodTable = true;
        }
        else
        {
            goToWoodTable = false;
        }
    }

    private void WaitingForService()
    {
        GetComponent<Animator>().SetBool("needService", true);
        state = MoodSates.waitingForService;
    }

    private void GetToTable()
    {
        GettingATable();

        if (distanceToLocation <= .78)
        {
            reached = true;
            gameObject.tag = "Unsatisfied";
        }

    }

    private void GetToSteelTable()
    {
        GettingASteelTable();

        if (distanceToLocation <= .78)
        {
            reached = true;
            gameObject.tag = "Unsatisfied";
        }
    }

    private void GettingATable()
    {
        System.Random rnd = new System.Random();
        if (availableTableIndex == 0)
        {
            int i = rnd.Next(1, availableTables.Length);
            //int i = availableTableIndex;
            if (availableTables[i].tag != "Unavailable")
            {
                setLocation = availableTables[i].transform;
                availableTableIndex = i;
                print(availableTableIndex);
                availableTables[i].tag = "Unavailable";
                table = availableTables[i];
            }
            else
                i = 0;
        }
        //print(distanceToLocation);
        navMeshAgent.SetDestination(setLocation.position);
    }

    private void GettingASteelTable()
    {

        System.Random rnd = new System.Random();
        if (availableTableIndex == 0)
        {
            int i = rnd.Next(1, steelTables.Length);
            //int i = availableTableIndex;
            if (steelTables[i].tag != "Unavailable")
            {
                setLocation = steelTables[i].transform;
                availableTableIndex = i;
                print(i);
                steelTables[i].tag = "Unavailable";
                table = steelTables[i];
            }
            else
                i = 0;
        }
        //print(distanceToLocation);
        navMeshAgent.SetDestination(setLocation.position);
    }
    private void SittingDown()
    {
        state = MoodSates.sittingDown;
    }

    private void WalkAroundEstablishment()
    {
        isMoving = true;
        navMeshAgent.isStopped = false;

        System.Random rnd = new System.Random();
        //print(distanceToLocation);
        if (walkDestinationAvailable)
        {
            int num = 0;
            num = rnd.Next(0, walkAroundLocations.Length);
            setLocation = walkAroundLocations[num].transform;
            //print("New set Location" + setLocation);
            //print("the cube selected " + setLocation.name);
            navMeshAgent.SetDestination(setLocation.position);
            walkDestinationAvailable = false;
            //print("Going to my destination");
        }
        //print("Final Destination " + distanceToLocation);
        if (HasReachedWalkedAroundLocation())
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
        if (distanceToArcade <= 1.3)
        {
            navMeshAgent.isStopped = true;
            isMoving = false;
            minutes -= Time.deltaTime;
            GetComponent<Animator>().SetBool("gameing", true);
            print(arcadeIndex);
            if (arcadeIndex % 2 == 0)
            {
                FaceTargetZpos();
            }
            else
            {
                FaceTargetZneg();
            }
            minutes -= Time.deltaTime;
            print(minutes);
            if (minutes <= 0)
            {
                print("Work");
                ForceWalkAroundMood();
                arcadeMachines[arcadeIndex].tag = "Available";
                minutes = 20;
            }
        }
    }

    private void SetIdleUponDestinationArcade(float distanceToLocation)
    {
        if (distanceToLocation <= 2)
        {
            //GetComponent<Animator>().SetBool("idle", true);
            isMoving = false;
            arcadeAvailable = true;
            CheckAvailableArcade();
        }
    }

    private void CheckAvailableArcade()
    {
        int num = 0;
        while (arcadeAvailable)
        {
            System.Random rnd = new System.Random();
            num = rnd.Next(1, arcadeMachines.Length);
            print("Arcade " + arcadeMachines[num]);

            if (arcadeMachines[num].tag == "Available")
            {
                arcadeAvailable = false;

                currentArcade = arcadeMachines[num].transform;
                setLocation = currentArcade.transform;
                navMeshAgent.SetDestination(arcadeMachines[num].transform.position);
                arcadeMachines[num].tag = "Unavailable";
                state = MoodSates.arcadeGameMood;
                arcadeIndex = num;
            }
            else
                arcadeAvailable = true;
        }
    }

    private void SetIdleUponDestination(float distanceToLocation)
    {
        if (HasReachedLocation())
        {
            //print("NPC got here");
            isMoving = false;
        }
    }

    private void GettingState()
    {
        System.Random rnd = new System.Random();
        int num = 0;
        num = rnd.Next(1, 6);
        reached = false;
        walkDestinationAvailable = true;
        arcadeAvailable = true;

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
                PrizeDeskLineMaker();
                break;
            case 4:
                //print("Walk Around");
                state = MoodSates.walkAroundMood;
                break;
            case 5:
                SelectALocation();
                state = MoodSates.pizzaMood;
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
    private void FaceTargetZpos()
    {
        Vector3 direction = (setLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z + 10));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    private void FaceTargetZneg()
    {
        Vector3 direction = (setLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z - 10));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private bool HasReachedLocation()
    {
        if (distanceToLocation < .5)
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
        if (distanceToLocation < .46)
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
        reached = false;
        GetComponent<Animator>().SetBool("sitting", false);
        GetComponent<Animator>().SetBool("needService", false);
        GetComponent<Animator>().SetBool("gameing", false);
        walkDestinationAvailable = true;

        System.Random rnd = new System.Random();
        int num = 0;
        num = rnd.Next(1, 3);

        switch (num)
        {
            case 1:
                minutes = 5;
                break;
            case 2:
                minutes = 120;
                break;
            case 3:
                minutes = 300;
                break;
        }
    }

    private void StateChangeTimer()
    {
        minutes -= Time.deltaTime;
        print(minutes);
        if (minutes <= 0)
        {
            GettingState();
            minutes = 20;
        }
    }

    private void FrontDeskLineMaker()
    {
        int i = 0;
        while (lineControllerIndex == 0)
        {
            //int i = availableTableIndex;
            if (frontDeskLine[i].tag != "Unavailable")
            {
                setLocation = frontDeskLine[i].transform;
                lineControllerIndex = i;
                print(lineControllerIndex);
                frontDeskLine[i].tag = "Unavailable";
                lineController = frontDeskLine[i];
            }
            else
            {
                i++;
                lineControllerIndex = 0;
            }
        }
    }

    private void PrizeDeskLineMaker()
    {
        int i = 0;
        while (lineControllerIndex == 0)
        {
            //int i = availableTableIndex;
            if (prizeLineMaker[i].tag != "Unavailable")
            {
                setLocation = prizeLineMaker[i].transform;
                lineControllerIndex = i;
                print(lineControllerIndex);
                prizeLineMaker[i].tag = "Unavailable";
                lineController = prizeLineMaker[i];
            }
            else
            {
                i++;
                lineControllerIndex = 0;
            }
        }
    }
}