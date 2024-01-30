using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public bool isBroken = false;

    [SerializeField] GameObject wall;
    [SerializeField] GameObject breakedWall;

    // Start is called before the first frame update
    void Start()
    {
        breakedWall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBroken)
        {
            wall.SetActive(false);
            breakedWall.SetActive(true);
        }
    }
}
