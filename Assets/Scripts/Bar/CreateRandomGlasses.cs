using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomGlasses : MonoBehaviour
{
    // Start is called before the first frame update
    // Task that creates random glasses in a collider

    public int maxGlasses = 3;
    public float spawnRate = 5f;
    public GameObject glassPrefab;

    private Collider area;
    private int numGlasses = 0;
    private Quaternion ogRotation;

    void Start()
    {
        area = GetComponent<Collider>();
        ogRotation = glassPrefab.transform.rotation;
        StartCoroutine(SpawnGlasses());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void subtractGlass()
    {
        numGlasses--;
    }

    private IEnumerator SpawnGlasses()
    {
        while (true)
        {
            if (numGlasses < maxGlasses)
            {
                // Spawn a glass somewhere in the collider
                GameObject glass = Instantiate(glassPrefab, RandomPointInBounds(area.bounds), ogRotation);
                Glass glassScript = glass.GetComponent<Glass>();
                glassScript.setGlassManager(this);
                numGlasses++;
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
