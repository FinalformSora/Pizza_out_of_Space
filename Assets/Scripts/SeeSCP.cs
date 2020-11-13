using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSCP : MonoBehaviour
{

    public Camera playerCam;
    public float sightRange = 50f;
    public Peanut scp;

    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = playerCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * sightRange, Color.white);

        if (Physics.Raycast(ray, out hit, sightRange, layerMask))
        {
            if (hit.collider.GetComponent<Peanut>())
            {
                scp.isBeingLookedAt = true;
            } else
            {
                scp.isBeingLookedAt = false;
            }
        }
    }
}
