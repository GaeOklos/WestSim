using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SC_Enemy : MonoBehaviour
{
    [SerializeField] private int life = 100;

    public void TakeDamage(int _dmgTotake)
    {
        life -= _dmgTotake;
        if (life <= 0) {
            // Rotatetoward
            Destroy(gameObject, 1);
        }
    }

}
