using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Mandatory library for using AI scripts

public class PlayerAgent : MonoBehaviour
{
    private Camera myCamera;
    private NavMeshAgent myNavMeshAgent;

    private void Start()
    {
        myCamera = Camera.main;

        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        //Getting the left mouse button down event
        if(Input.GetMouseButton(0))
        {
            //Setting a ray between the camera and the mouse position
            Ray myRay = myCamera.ScreenPointToRay(Input.mousePosition);

            //Casting the raycast using the calculated ray with the camera & get the hit informations using an IF statement to check if the ray hit something
            RaycastHit hit;
            if (Physics.Raycast(myRay, out hit ))
            {
                //Move the player agent
                myNavMeshAgent.SetDestination(hit.point);
            }
        }
    }
}
