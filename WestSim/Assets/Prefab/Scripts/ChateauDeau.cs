using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChateauDeau : MonoBehaviour
{
    public int hitNeeded = 0;
    public int hit = 0;
    [SerializeField] private Animator animator;

    void Update()
    {
        if (hit == hitNeeded)
        {
            animator.Play("ChateauFall");
        }
    }

    public void Hit()
    {
        if (hitNeeded != hit)
        {
            animator.Play("Pied");
        }
    }
}
