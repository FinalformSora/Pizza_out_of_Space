using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    [SerializeField] float attractRange = 15f;
    GameObject[] monstersInRange;

    private GameObject spray;
    private GameObject eyes;
    private float timer = 30f;

    private float distanceToMonster = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        spray = this.gameObject;
        eyes = GameObject.FindGameObjectWithTag("MainCamera");
        spray.GetComponent<Rigidbody>().AddForce(eyes.transform.forward * 1500);
        monstersInRange = GameObject.FindGameObjectsWithTag("Monster");
    }

    // Update is called once per frame
    void Update()
    {
        monstersInRange = GameObject.FindGameObjectsWithTag("Monster");
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(spray);
        }

        foreach (GameObject x in monstersInRange)
        {
            distanceToMonster = Vector3.Distance(x.transform.position, transform.position);
            if(distanceToMonster <= attractRange)
            {
                AttractMonster(x);
            }
        }
    }

    private void AttractMonster(GameObject x)
    {
        if (x.GetComponent<Witch>())
        {
            x.GetComponent<Princess>().pTransform = gameObject.transform;
            x.GetComponent<Princess>().attracted = true;
        }
        if (x.GetComponent<SCP173>())
        {
            x.GetComponent<Peanut>().target = gameObject.transform;
            x.GetComponent<Peanut>().attracted = true;
        }
        if (x.GetComponent<Klown>())
        {
            x.GetComponent<KlownAi>().target = gameObject.transform;
            x.GetComponent<KlownAi>().attracted = true;
        }
        if (x.GetComponent<Sirenhead>())
        {
            x.GetComponent<sirenHeadAi>().target = gameObject.transform;
            x.GetComponent<sirenHeadAi>().attracted = true;
        }
    }

}
