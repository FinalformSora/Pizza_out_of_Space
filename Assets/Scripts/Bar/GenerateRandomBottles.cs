using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomBottles : MonoBehaviour
{

    private GameObject[] bottlePrefabs;
    private Transform thisTransform;

    public bool isSide = false;

    void Start()
    {
        thisTransform = this.gameObject.transform;
        bottlePrefabs = Resources.LoadAll<GameObject>("LiquorBottlePrefabs/LiquorBottles");
        Vector3 colliderSize = this.gameObject.GetComponent<Collider>().bounds.size / 2;
        if (!isSide)
        {
            float minZ = colliderSize.z - 1;
            while (minZ > -colliderSize.z)
            {
                InstantiateRandomPrefab(thisTransform.position + new Vector3(0, -colliderSize.y, minZ));
                minZ--;
            }
        } else
        {
            float minX = colliderSize.x - 1;
            while (minX > -colliderSize.x)
            {
                InstantiateRandomPrefab(thisTransform.position + new Vector3(minX, -colliderSize.y, 0));
                minX--;
            }
        }
        
    }

    void Update()
    {
        
    }

    void InstantiateRandomPrefab(Vector3 position)
    {
        GameObject randomBottle = bottlePrefabs[Random.Range(0, bottlePrefabs.Length)];
        Vector3 startingRotation = randomBottle.transform.localRotation.eulerAngles;
        if (isSide)
        {
            startingRotation += new Vector3(0, 0, 90f);

        }

        GameObject go = Instantiate(randomBottle, position, Quaternion.Euler(startingRotation));
        go.transform.parent = thisTransform;
    }
}
