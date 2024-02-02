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
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<SC_Movement>().TakeDamage(1);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log(other.name);
            Destroy(gameObject);
        }
    }
}
 