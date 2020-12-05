using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Fear : MonoBehaviour
{
    // Default level of fear
    public int fear = 0;
    public int maxFear = 100;
    public float lowestFlickerInterval = 5f;

    public int dangerWearoffTime = 5;

    // private float testTimer = 0f;
    
    public Light flashlight;

    public float flickerInterval = 10f;

    private float timer = 0f;
    public bool flickering = false;

    public PlayerController pc;

    public bool inDanger = false;
    private float fearTimer = 0f;

    private float dangerTimer = 0f;

    public float defaultFearRate = 1f;
    private float fearRate;

    public Text fearText;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 
         * Used for testing fear value
         * Gives 5 fear every second.
        testTimer += Time.deltaTime;
        if (testTimer % 60 >= 1 && fear < 100)
        {
            fear += 5;
            testTimer = 0f;
        }
        */

        fearTimer += Time.deltaTime;
        if (inDanger)
        {
            dangerTimer += Time.deltaTime;
            if (dangerTimer % 60 >= dangerWearoffTime)
            {
                inDanger = false;
                fearRate = defaultFearRate;
            }

            if (fearTimer % 60/fearRate >= 1 && fear < 100)
            {
                fearTimer = 0;
                fear += 3;
            }

        } else
        {
            if (fearTimer % 60 >= 3 && fear > 0)
            {
                fearTimer = 0;
                fear--;
            }
        }

        flickerInterval = 60f + Random.Range(0.3f,lowestFlickerInterval) - fear * (60f/maxFear);

        if (timer % 60 >= flickerInterval && !flickering && pc.lighton)
        {
            StartFlicker();
        } else
        {
            timer += Time.deltaTime;
        }

        fearText.text = fear + "%";
    }

    public void StartFlicker()
    {
        flickering = true;
        float flickerDuration = Random.Range(0.1f, 1f);
        StartCoroutine(Flicker(flickerDuration));
        StartCoroutine(StopFlicker(flickerDuration));
    }

    public void invokeFear(float rate = 1f)
    {
        fearRate = rate;
        dangerTimer = 0f;
        inDanger = true;
    }

    IEnumerator Flicker(float duration)
    {
        flashlight.enabled = false;
        yield return new WaitForSeconds(duration);
        flashlight.enabled = true;
    }

    IEnumerator StopFlicker(float duration)
    {
        yield return new WaitForSeconds(duration);
        flickering = false;
        timer = 0f;
    }
}
