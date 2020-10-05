using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPhysicsBehavior : MonoBehaviour
{

    // The door's pivot point is adjusted on the model, so we can just say that's the center of gravity. (0,0,0)
    public Vector3 centerOfGravity = new Vector3(0, 0, 0);
    public Rigidbody rb;
    public Transform tf;

    // Default degree of rotation
    private float defaultEulerY;
    private Quaternion defaultRotation;

    // Maxmimum degree of rotation +/- 90;
    public float degreeOfRotation = 90f;

    // Value for smooth rotation back to normal
    public float smooth = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize rigidbody
        rb = GetComponent<Rigidbody>();

        // Set the center of mass of the rigidbody (so the door rotates around a specific axis)
        rb.centerOfMass = centerOfGravity;

        defaultEulerY = tf.localEulerAngles.y;
        defaultRotation = tf.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (tf.localEulerAngles.y < 45.0)
        {
            //tf.rotation = Quaternion.Euler(new Vector3(tf.localEulerAngles.x, tf.localEulerAngles.y + degreeOfRotation, tf.localEulerAngles.z));
        } else if (tf.localEulerAngles.y < 135 && tf.localEulerAngles.y > 45)
        {
           // tf.rotation = Quaternion.Euler(new Vector3(tf.localEulerAngles.x, tf.localEulerAngles.y - degreeOfRotation, tf.localEulerAngles.z));
        }
    }
}
