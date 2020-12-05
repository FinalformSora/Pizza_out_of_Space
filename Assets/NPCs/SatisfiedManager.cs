using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SatisfiedManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HelpCustomer();
    }

    private void HelpCustomer()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            //print(hit.collider.gameObject.name + " got hit");
            if (Input.GetKey(KeyCode.Mouse0) && hit.collider.gameObject.tag == "Unsatisfied" )
            {
                hit.collider.gameObject.tag = "Satisfied";
                //hit.collider.gameObject.SendMessage("ForceWalkAroundMood");
                print(hit.collider.gameObject.name + " has been clicked!");
            }
        }
    }


}
