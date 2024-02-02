using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStarter : MonoBehaviour
{
    private Timer timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject gb = GameObject.FindGameObjectWithTag("Timer");
            timer = gb.GetComponent<Timer>();
            timer.StartTimer();
            EnemyActivator();
        }
    }

    private void EnemyActivator()
    {
        SC_EnyShoot[] sC_EnyShoots = FindObjectsOfType<SC_EnyShoot>();
        foreach (var b in sC_EnyShoots)
        {
            b._canShoot = true;
        }
    }
}
