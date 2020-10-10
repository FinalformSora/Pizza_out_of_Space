using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{

    public CharacterController player;

    public AudioSource feet;

    public AudioClip[] clips_walking;
    public AudioClip[] clips_running;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.velocity.x != 0 || player.velocity.z != 0) && player.velocity.y == 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playFootsteps(clips_running);
            }

            playFootsteps(clips_walking);
        }
    }

    void playFootsteps(AudioClip[] clips)
    {
        if (!feet.isPlaying)
        {
            AudioClip nextStep = clips[Random.Range(0, clips.Length)];
            feet.clip = nextStep;
            feet.Play();

        }
    }
}
