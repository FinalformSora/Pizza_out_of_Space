using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using UnityEngine.AI;

public class DynamicSpiderCreation : MonoBehaviour
{
    // The objects that we want to create spiders on top of
    [SerializeField]
    public GameObject[] planes;
    public GameObject spiderPrefab;
    public int spawnTimeSeconds = 5;
    private float internalClock = 0f;

    private int numSpiders = 0;
    public int maxSpiders = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        internalClock += Time.deltaTime;

        if (internalClock % 60 >= spawnTimeSeconds)
        {
            if (numSpiders != maxSpiders)
            {
                GameObject randomPlane = planes[UnityEngine.Random.Range(0, planes.Length)];
                Vector3 randomPosition = randomPositionOnPlane(randomPlane);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 1000f, NavMesh.AllAreas))
                {
                    GameObject go = Instantiate(spiderPrefab, hit.position, Quaternion.identity);
                    go.transform.parent = this.gameObject.transform;
                    numSpiders++;
                }
            }
            internalClock = 0f;
        }
    }

    Vector3 randomPositionOnPlane(GameObject plane)
    {
        Vector3 planeSize = plane.GetComponent<Collider>().bounds.size;
        float randX = UnityEngine.Random.Range(plane.transform.position.x, planeSize.x/2);
        float randZ = UnityEngine.Random.Range(plane.transform.position.z, planeSize.z/2);
        return new Vector3(randX, plane.transform.position.y, randZ);
    }

    public void killASpider()
    {
        if (numSpiders > 0)
        {
            numSpiders--;
        }
    }
}
