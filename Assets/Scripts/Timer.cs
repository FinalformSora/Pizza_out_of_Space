using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] GameObject sun;
    [SerializeField] Text timer;
    [SerializeField] bool night = false;
    [SerializeField] GameObject solaire;
    GameObject[] monsters;

    public LockDoors[] doors = new LockDoors[2];
    // Start is called before the first frame update
    void Start()
    {
        time = 350f;
        sun = GameObject.Find("Sun");
        monsters = GameObject.FindGameObjectsWithTag("Monster");
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
            sun.SetActive(false);
            lockDoors(doors, false);
            night = true;
            solaire.SetActive(true);
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
}
