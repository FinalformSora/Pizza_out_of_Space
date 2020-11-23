using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoors : MonoBehaviour
{
    public GameObject sun;
    public bool locked;

    private RigidbodyConstraints initialConstraints;
    private Quaternion initialQuaternion;

    // Start is called before the first frame update
    void Start()
    {
        initialConstraints = GetComponent<Rigidbody>().constraints;
        initialQuaternion = transform.rotation;
        Lock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Lock()
    {
        locked = true;
        transform.rotation = initialQuaternion;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Unlock()
    {
        locked = false;
        GetComponent<Rigidbody>().constraints = initialConstraints;
    }
}
