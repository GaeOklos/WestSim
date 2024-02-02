using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPrisoner : MonoBehaviour
{
    [SerializeField] Prisoner prisoner;

    public void Hitted()
    {
        prisoner.echec = true;
    }
}
