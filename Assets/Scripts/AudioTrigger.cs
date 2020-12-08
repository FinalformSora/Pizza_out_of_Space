using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource topFloorAudio;
    public AudioSource bottomFloorAudio;
    public AudioSource arcadeAudio;
    public PlayerController pc;
    public GameObject zombies;

    private ZombieSounds[] zombiesAudio;
    private bool playerOnBottomFloor = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == pc.GetComponent<Collider>())
        {
            Debug.Log("Entered bottom floor");
            playerOnBottomFloor = true;
        }
        TriggerAudio();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == pc.GetComponent<Collider>())
        {
            playerOnBottomFloor = false;
        }
        TriggerAudio();
    }

    private void TriggerAudio()
    {
        bottomFloorAudio.enabled = playerOnBottomFloor;
        topFloorAudio.enabled = !playerOnBottomFloor;
        arcadeAudio.enabled = !playerOnBottomFloor;
        TriggerZombieAudio();
    }

    private void TriggerZombieAudio()
    {
        zombiesAudio = zombies.GetComponentsInChildren<ZombieSounds>();
        for (int i = 0; i < zombiesAudio.Length; i++)
        {
            zombiesAudio[i].playerIsInBasement = playerOnBottomFloor;
        }
    }
}
