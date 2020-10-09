using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_rb_interaction : MonoBehaviour
{

    public float pushForce = 30.0f;

    public CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body != null && !body.isKinematic)
        {
            body.AddForceAtPosition(hit.controller.velocity * pushForce, hit.point);
        }
    }
}
