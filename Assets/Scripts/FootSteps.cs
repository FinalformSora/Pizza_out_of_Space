using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FootSteps : MonoBehaviour
{

    public CharacterController player;

    public AudioSource feet;

    public AudioClip clip_walking;
    public AudioClip clip_running;
    public AudioClip clip_jumping;

    // Start is called before the first frame update
    void Start()
    {
        feet.clip = clip_running;
    }

    // Update is called once per frame
    void Update()
    {
        if (!feet.isPlaying)
            feet.Play();

        if ((player.velocity.x != 0 || player.velocity.z != 0) && player.velocity.y < 0.1)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (feet.isPlaying && (feet.clip != clip_running))
                {
                    feet.Stop();
                    feet.clip = clip_running;
                }
            }
            else
            {
                if (feet.isPlaying && (feet.clip == clip_running || feet.clip == clip_jumping))
                {
                    feet.Stop();
                    feet.clip = clip_walking;
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            feet.Stop();
            feet.clip = clip_jumping;
        }

        if (player.velocity.x == 0 && player.velocity.z == 0 && player.velocity.y < 0.1)
            feet.Stop();

    }
}