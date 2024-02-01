using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DontDestroy : MonoBehaviour
{
    public GameObject[] _tabObject;

    private void Awake() {
        foreach(var element in _tabObject)
            DontDestroyOnLoad(element);
    }
}
