using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{

    public AudioSource mouthAudio;
    public AudioClip fastBreathing;
    public AudioClip slowBreathing;

    private Fear fear;

    // Start is called before the first frame update
    void Start()
    {
        fear = GetComponent<Fear>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouthAudio.isPlaying)
        {
            if (fear.fear > 50 || fear.inDanger)
            {
                mouthAudio.clip = fastBreathing;
                mouthAudio.Play();
            }
        }
    }
}
