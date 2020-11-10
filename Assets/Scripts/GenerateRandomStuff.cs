using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomStuff : MonoBehaviour
{
    private GameObject[] prefabs;
    private Transform thisTransform;
    public string prefabResource;

    public string axis;

    public bool randomRotation = false;

    void Start()
    {
        thisTransform = this.gameObject.transform;
        prefabs = Resources.LoadAll<GameObject>(prefabResource);
        Vector3 colliderSize = this.gameObject.GetComponent<Collider>().bounds.size / 2;

        float min;

        if (axis == "x" || axis == "X")
        {
            min = colliderSize.x - 1;

            while (min > -colliderSize.x)
            {
                InstantiateRandomPrefab(thisTransform.position + new Vector3(min, colliderSize.y, 0));
                min -= 6;
            }

        } else if (axis == "z" || axis == "Z")
        {
            min = colliderSize.z - 1;

            while (min > -colliderSize.z)
            {
                InstantiateRandomPrefab(thisTransform.position + new Vector3(0, colliderSize.y, min));
                min -= 6;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateRandomPrefab(Vector3 position)
    {
        GameObject randomBottle = prefabs[Random.Range(0, prefabs.Length)];
        Vector3 startingRotation = randomBottle.transform.localRotation.eulerAngles;
        if (randomRotation)
        {
            startingRotation = startingRotation + new Vector3(0, (float)Random.Range(0, 360), 0);

        }

        GameObject go = Instantiate(randomBottle, position, Quaternion.Euler(startingRotation));
        go.transform.parent = thisTransform;
    }
}
