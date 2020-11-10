using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Fear : MonoBehaviour
{
    // Default level of fear
    public int fear = 0;
    public int maxFear = 100;
    public float lowestFlickerInterval = 5f;

    private float testTimer = 0f;
    
    public Light flashlight;

    public float flickerInterval = 10f;

    private float timer = 0f;
    public bool flickering = false;

    public PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        testTimer += Time.deltaTime;
        if (testTimer % 60 >= 1 && fear < 100)
        {
            fear += 5;
            testTimer = 0f;
        }

        flickerInterval = 60f + Random.Range(0.3f,lowestFlickerInterval) - fear * (60f/maxFear);

        if (timer % 60 >= flickerInterval && !flickering && pc.lighton)
        {
            StartFlicker();
        } else
        {
            timer += Time.deltaTime;
        }
    }

    void StartFlicker()
    {
        flickering = true;
        float flickerDuration = Random.Range(0.1f, 1f);
        StartCoroutine(Flicker(flickerDuration));
        StartCoroutine(StopFlicker(flickerDuration));
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
