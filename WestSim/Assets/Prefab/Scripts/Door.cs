using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject uiIndicator;
    [SerializeField] BreakableWall breakableWall;

    private bool onCollision = false;
    public bool open = false;
    

    // Start is called before the first frame update
    void Start()
    {
        uiIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (onCollision && open is false && breakableWall.isBroken is false)
        {
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                animator.Play("Open");
                open = true;
            }
        }
        if (onCollision && open && breakableWall.isBroken is false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.Play("Close");
                open = false;
            }
        }

        if (breakableWall.isBroken) 
        {
            animator.Play("Closed");
            open = false;
            uiIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider Player)
    {
        onCollision = true;
        uiIndicator.SetActive(true);
    }

    private void OnTriggerExit(Collider Player)
    {
        onCollision = false;
        uiIndicator.SetActive(false);
    }
}
