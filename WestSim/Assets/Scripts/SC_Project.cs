using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Project : MonoBehaviour
{
    // [SerializeField] private float speed = 0.2f;
    [SerializeField] private float lifeTime = 5f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Collider[] azerty = Physics.OverlapSphere(transform.position, 0.1f);
        // hitColliders.Clear();
        // for (int j = 0; j < azerty.Length; j++)
        //     hitColliders.Add(azerty[j]);

        // while (hitColliders.Count > 0)
        // {
        //     if (hitColliders[0].gameObject.CompareTag("Player")) {
        //         hitColliders[0].gameObject.GetComponent<SC_Movement>().TakeDamage(10);
        //     }
        // }
        // if (hitColliders.Count > 0)
        //     Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<SC_Movement>().TakeDamage(10);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
 