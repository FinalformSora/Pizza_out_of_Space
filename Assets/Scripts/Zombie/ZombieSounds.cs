using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    public float minSeconds = 5f;
    public float maxSeconds = 15f;

    private AudioSource klownSound;
    public AudioClip[] clips;
    public AudioClip attack;

    public bool playerIsInBasement = false;
    private bool playerdead = false;

    // Start is called before the first frame update
    void Start()
    {
        klownSound = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSounds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackSound()
    {
        if (playerdead)
        {
            return;
        }
        klownSound.Stop();
        klownSound.clip = attack;
        klownSound.Play();
        playerdead = true;
    }

    private IEnumerator PlayRandomSounds()
    {
        while (!playerdead)
        {
            if (!klownSound.isPlaying && playerIsInBasement)
            {
                klownSound.clip = clips[Random.Range(0, clips.Length)];
                klownSound.Play();
            }
            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));
        }
    }
}
