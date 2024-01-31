using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SC_Movement _Player = other.gameObject.GetComponent<SC_Movement>();
        if (_Player != null)
        {
            _Player.Bumper(transform.up);
            // _Player.Bumper(transform.up);
        }
    }
}