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
        }
    }
}
