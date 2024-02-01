using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PCLifeCanva : MonoBehaviour
{
    [SerializeField] private int _life = 4;
    [SerializeField] private GameObject _life1;
    [SerializeField] private GameObject _life2;
    [SerializeField] private GameObject _life3;
    [SerializeField] private GameObject _life4;

    public void TakeDamage(int damage)
    {
        _life -= damage;
        if (_life == 3)
            _life4.SetActive(false);
        if (_life == 2)
            _life3.SetActive(false);
        if (_life == 1)
            _life2.SetActive(false);
        if (_life == 0)
            _life1.SetActive(false);
    }
}
