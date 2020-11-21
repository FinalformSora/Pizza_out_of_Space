using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCollectibleCreation : MonoBehaviour
{
    public GameObject artifact;
    public Transform allArtifacts;

    public int numCollectibles = 10;
    public Tuple xRange;
    public Tuple yRange;
    public Tuple zRange;

    public Material m_MainMaterial;

    public float cubeSize = 0.5f;
    private Vector3 cubeVector;

    private int[] collectibles;

    private Collider[] spawnAreas;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all the spawn areas
        spawnAreas = GetComponentsInChildren<Collider>();
        generateArtifactsOnRacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The old way we generated cubes, only used for testing.
    void legacyGenArtifacts()
    {
        cubeVector = new Vector3(cubeSize, cubeSize, cubeSize);
        collectibles = new int[numCollectibles];
        for (int i = 0; i < numCollectibles; i++)
        {
            //GameObject collectible = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //collectible.transform.localScale = cubeVector;
            float xPos = UnityEngine.Random.Range(xRange.min, xRange.max);
            float yPos = UnityEngine.Random.Range(cubeSize / 2, yRange.max);
            float zPos = UnityEngine.Random.Range(zRange.min, zRange.max);
            //collectible.transform.position = new Vector3(xPos, yPos, zPos);
            //collectible.gameObject.name = "Collectible";
            //collectible.gameObject.transform.parent = parent;
            GameObject collectible = Instantiate(artifact, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            //collectible.GetComponent<Renderer>().material = m_MainMaterial;
            collectible.layer = artifact.layer;
        }
    }

    public class Tuple
    {
        public float min;
        public float max;
    }

    public void generateArtifactsOnRacks()
    {
        for (int i = 0; i < numCollectibles; i++)
        {
            Vector3 randomColliderArea = spawnAreas[Random.Range(0, spawnAreas.Length)].bounds.size;
            float randomPosX = Random.Range(0, randomColliderArea.x);
            float randomPosZ = Random.Range(0, randomColliderArea.z);
            GameObject collectible = Instantiate(artifact, new Vector3(randomPosX, randomColliderArea.y, randomPosZ), Quaternion.identity);
            collectible.transform.parent = allArtifacts;
            Debug.Log("Generated Artifact");
        }

    }
}
