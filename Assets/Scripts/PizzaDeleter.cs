using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaDeleter : MonoBehaviour
{

    public int bounty = 20;
    public int taskTime = 1;
    public float progress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void makePizza()
    {
        progress += Time.deltaTime;
    }

    public void finishPizza()
    {
        // Do stuff here when pizza is finished
        Destroy(this.gameObject);
    }
}
