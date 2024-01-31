using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : MonoBehaviour
{
    [SerializeField] private int objectiveNb = 0;
    [SerializeField] public GameObject interactIcon;
    [SerializeField] public GameObject attached;
    [SerializeField] public GameObject free;
    private bool liberable = false;
    private bool libere = false;
    private ObjectiveManager manager;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && liberable)
        {
            libere = true;
            GameObject gb = GameObject.FindGameObjectWithTag("ObjectiveManager");
            manager = gb.GetComponent<ObjectiveManager>();
        }

        if (libere && objectiveNb == 1)
        {
            manager.firstObjective = true;
        } else if (libere && objectiveNb == 2)
        {
            manager.secondObjective = true;
        } else if (libere && objectiveNb == 3)
        {
            manager.thirdObjective = true;
        } else if (libere && objectiveNb == 4)
        {
            manager.fourthObjective = true;
        }

        if (libere)
        {
            attached.SetActive(false);
            free.SetActive(true);
            interactIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && libere is false)
        {
            interactIcon.SetActive(true);
            liberable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            liberable = false;
        }
    }
}
