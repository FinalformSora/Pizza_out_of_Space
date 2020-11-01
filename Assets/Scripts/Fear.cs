using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : MonoBehaviour
{
    // Default level of fear
    public int fear = 100;

    public int[] fearLevels = { 0, 20, 40, 60, 80 };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fear < 100)
        {

        }
    }
}
