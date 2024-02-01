using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Fonction pour lancer une fonction d'un autre script


public class PickUpTriger : MonoBehaviour
{
    private Deplacement MoveScript;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            bool OffOn = this.gameObject.activeSelf;
            print(OffOn);

            // col.gameObject.SendMessage("PickupTaken", 25);
            MoveScript = col.gameObject.GetComponent<Deplacement>();
            MoveScript.PickupTaken(25);
            // col.gameObject.GetComponent<Deplacement>().PickupTaken(25);
            this.gameObject.SetActive(false);
            // Destroy(this.gameObject);
        }
    }

    /*void OnCollisionEnter(Collision Collider)
    {
        print(Collider);
    }*/
}
