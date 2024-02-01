using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public bool isBroken = false;

    public bool isGlass = false;

    public bool isWood = false;

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
    public void BreakShoot()
    {
        if (isGlass) 
        {
            isBroken = true;
        }
    }
    public void BreakPunch()
    {
        if (isGlass || isWood)
        {
            isBroken = true;
        }
    }
    public void BreakBigPunch()
    {
        isBroken = true;
    }
}
