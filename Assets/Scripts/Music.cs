using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    private AudioSource musicPlayer;
    public AudioClip[] music;

    private int songNum = 0;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicPlayer.isPlaying && musicPlayer.enabled == true)
            playSong();
    }

    void playSong()
    {
        musicPlayer.clip = music[index];
        musicPlayer.Play();

        index++;

        if (index >= music.Length)
        {
            index = 0;
        }
    }
}
