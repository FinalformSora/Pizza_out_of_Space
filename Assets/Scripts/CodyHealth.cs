using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 1f;

    //Take damage
    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            //Debug.Log("You dead, my Glip glop");
            GetComponent<LosingScreen>().HandleDeath();
        }
    }

}
