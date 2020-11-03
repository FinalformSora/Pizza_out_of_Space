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
    // Start is called before the first frame update
    void Start()
    {
        time = 360f;
        sun = GameObject.Find("Sun");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        time %= 1440;
        hour = (int)time / 60;
        minute = (int)time % 60;
        if (time > 360 && time < 1080)
        {
            sun.SetActive(true);
        }
        else
        {
            sun.SetActive(false);
        }
        if(minute < 10)
        {
            timer.text = hour + ":0" + minute; 
        }
        else
        {
            timer.text = hour + ":" + minute;
        }

        Debug.Log("Hour: " + hour + "Minutes:" + minute + "time:" + time);
    }
}
