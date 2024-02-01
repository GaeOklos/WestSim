using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBreakable : MonoBehaviour
{
    public bool isBroken = false;

    public bool isGlass = false;

    public bool isWood = false;

    void Update()
    {
        if (isBroken)
        {
            Destroy(gameObject);
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
