using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Project : MonoBehaviour
{
    // [SerializeField] private float speed = 0.2f;
    [SerializeField] private float lifeTime = 8f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        // this.transform.position = new Vector3 (speed,0,0) + this.transform.position;
    }
}
 