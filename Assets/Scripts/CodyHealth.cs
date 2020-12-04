using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 1f;

    public AudioSource mouth;
    public AudioClip deathScream;

    private bool isDead = false;

    //Take damage
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            isDead = true;
            //Debug.Log("You dead, my Glip glop");
            GetComponent<PlayerController>().isGameOver = true;
            mouth.clip = deathScream;
            mouth.Play();
            
        }
    }

}
