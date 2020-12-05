using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] int hour;
    [SerializeField] int minute;
    [SerializeField] GameObject sun;
    [SerializeField] Text timer;

    public LockDoors[] doors = new LockDoors[2];
    // Start is called before the first frame update
    void Start()
    {
        time = 1080f;
        sun = GameObject.Find("Sun");
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
        }
        else
        {
            sun.SetActive(false);
            lockDoors(doors, false);
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


}
