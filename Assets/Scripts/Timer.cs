using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject sirenHead;
    [SerializeField] GameObject scp173;
    [SerializeField] GameObject witch;
    [SerializeField] GameObject klown;

    public float startTime = 361f;
    [SerializeField] float time;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] GameObject sun;
    [SerializeField] Text timer;
    [SerializeField] bool night = false;
    [SerializeField] GameObject solaire;
    GameObject[] monsters;
    private MonsterSpawnLocations spawn;
    private bool monsterSpawned = false;
    private bool doorsUnlocked = false;

    public Transform zombies;

    public LockDoors[] doors = new LockDoors[2];
    // Start is called before the first frame update
    void Start()
    {
        time = startTime;
        sun = GameObject.Find("Sun");
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        spawn = new MonsterSpawnLocations();
        SpawnLowerMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 2;
        time %= 1440;
        hour = (int)time / 60;
        minute = (int)time % 60;
        if (time > 360 && time < 1080)
        {
            sun.SetActive(true);
            lockDoors(doors, true);
            if (night)
            {
                checkWinCondition();
            }
        }
        else
        {
            if (!doorsUnlocked)
            {
                doorsUnlocked = true;

            }
            sun.SetActive(false);
            lockDoors(doors, false);
            night = true;
            solaire.SetActive(true);
            if(time > 1200 && !monsterSpawned)
            {
                monsterSpawned = true;
                SpawnUpperMonsters();
            }
        }
        if(minute < 10)
        {
            timer.text = hour + ":0" + minute; 
        }
        else
        {
            timer.text = hour + ":" + minute;
        }

        //Debug.Log("Hour: " + hour + "Minutes:" + minute + "time:" + time);
    }

    void lockDoors(LockDoors[] doors, bool locked)
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (locked)
            {
                doors[i].Lock();
            } else
            {
                doors[i].Unlock();
            }
        }
    }

    void checkWinCondition()
    {
        int artifactsCollected = gameObject.GetComponent<FixMachine>().artifactCount;
        if(artifactsCollected == 10)
        {
            winGame();
        }
        else
        {
            loseGame();
        }
    }

    void winGame()
    {
        SceneManager.LoadScene(2);
    }

    void loseGame()
    {
        gameObject.GetComponent<Fear>().fear = 100;
        foreach(GameObject x in monsters)
        {
            EndGame(x);
        }
    }

    void EndGame(GameObject x)
    {
        if (x.GetComponent<Witch>())
        {
            x.GetComponent<Princess>().endGame = true;
        }
        if (x.GetComponent<SCP173>())
        {
            x.GetComponent<Peanut>().endGame = true;
        }
        if (x.GetComponent<Klown>())
        {
            x.GetComponent<KlownAi>().endGame = true;
        }
        if (x.GetComponent<Sirenhead>())
        {
            x.GetComponent<sirenHeadAi>().endGame = true;
        }
    }

    void SpawnLowerMonsters()
    {
        int i = 0;
        foreach(Vector3 x in spawn.getLower())
        {
            switch (i)
            {
                default: break;
                case 0: Instantiate(scp173, x, Quaternion.identity); break;
                case 1: Instantiate(witch, x, Quaternion.identity); break;
                case 2: GameObject klow = Instantiate(klown, x, Quaternion.identity); klow.transform.parent = zombies; break;
            }
            i++;
            if(i > 2)
            {
                i = 0;
            }
        }
    }

    void SpawnUpperMonsters()
    {
        int i = 0;
        foreach(Vector3 x in spawn.getUpper())
        {
            switch (i)
            {
                default: break;
                case 0: Instantiate(scp173, x, Quaternion.identity); break;
                case 1: Instantiate(witch, x, Quaternion.identity); break;
                case 2: Instantiate(klown, x, Quaternion.identity); break;
            }
            i++;
            if (i > 2)
            {
                i = 0;
            }
        }
    }
}
