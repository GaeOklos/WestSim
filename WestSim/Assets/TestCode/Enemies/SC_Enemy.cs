using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SC_Enemy : MonoBehaviour
{
    [SerializeField] private int life = 100;
    [SerializeField] private int objectiveNb = 0;
    

    public void TakeDamage(int _dmgTotake)
    {
        life -= _dmgTotake;
        if (life <= 0) {
            GameObject gb = GameObject.FindGameObjectWithTag("ObjectiveManager");
            ObjectiveManager manager = gb.GetComponent<ObjectiveManager>();
            if (objectiveNb == 1)
            {
                manager.firstObjective = true;
            }
            else if (objectiveNb == 2)
            {
                manager.secondObjective = true;
            }
            else if (objectiveNb == 3)
            {
                manager.thirdObjective = true;
            }
            else if (objectiveNb == 4)
            {
                manager.fourthObjective = true;
            }
            // Rotatetoward
            Destroy(gameObject, 1);
            Destroy(gameObject, 1);
        }
    }

}
