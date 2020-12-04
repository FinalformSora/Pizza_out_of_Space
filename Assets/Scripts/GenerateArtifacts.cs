using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateArtifacts : MonoBehaviour
{
    public GameObject artifactPrefab;

    public int numArtifacts = 10;

    private Collider[] children;
    private Transform parent;
    private Collider randomCollider;

    void Start()
    {
        SpawnArtifacts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateRandomPrefab(Vector3 position)
    {

    }

    public void SpawnArtifacts()
    {

        parent = this.gameObject.transform;
        children = GetComponentsInChildren<Collider>();

        for (int i = 0; i < numArtifacts; i++)
        {

            randomCollider = children[Random.Range(0, children.Length)];
            Vector3 colliderSize = randomCollider.bounds.size / 2;
            Vector3 randomX = new Vector3(UnityEngine.Random.Range(-colliderSize.x, colliderSize.x), 0, 0);

            GameObject go = Instantiate(artifactPrefab, randomCollider.transform.position + randomX, Quaternion.identity);
            go.transform.parent = parent;
        }
    }
}
