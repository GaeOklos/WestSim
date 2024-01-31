using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChateauDeau : MonoBehaviour
{
    public int hitNeeded = 0;
    public int hit = 0;
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject pied;

    void Update()
    {
        if (hit == hitNeeded)
        {
            animator.Play("ChateauFall");
            pied.SetActive(false);
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
