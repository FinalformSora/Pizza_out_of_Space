using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{

    public int bounty = 5;
    public int taskTime = 1;
    public float progress = 0f;
    private CreateRandomGlasses glassManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setGlassManager(CreateRandomGlasses manager)
    {
        glassManager = manager;
    }

    public void pickupGlass()
    {
        progress += Time.deltaTime;
    }

    public void resetGlass()
    {
        progress = 0;
    }

    public void finishGlass()
    {
        // Do stuff here when pizza is finished
        Destroy(this.gameObject);
        if (glassManager)
        {
            glassManager.subtractGlass();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
