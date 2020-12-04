using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    private GameObject spray;
    private GameObject eyes;
    private float timer = 30;
    // Start is called before the first frame update
    void Start()
    {
        spray = this.gameObject;
        eyes = GameObject.FindGameObjectWithTag("MainCamera");
        spray.GetComponent<Rigidbody>().AddForce(eyes.transform.forward * 1500);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(spray);
        }
    }
}
