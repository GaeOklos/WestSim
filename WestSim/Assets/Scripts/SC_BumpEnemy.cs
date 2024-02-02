using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SC_BumpEnemy : MonoBehaviour
{
    [Header("Bumper")]
    private float _bumpForce = 10.0f;
    [SerializeField] private bool _isBumped = false;
    private Vector3 _bumpVectorDirector;
    private Vector3 _bumpVectorSpeedCurrent;
    [SerializeField] private float _ForceYBumperInGround = 20f;
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private Vector3 moveDirection = Vector3.zero ;

    private void Update()
    {
        // DecreaseSpeedBump();
    }

    public void Bumper(Vector3 _bumpVectorDirector)
    {
        _bumpVectorSpeedCurrent = _bumpVectorDirector * _bumpForce;
        if (_bumpVectorSpeedCurrent.y <= 0)
        {
            _bumpVectorSpeedCurrent.y = 7;
        }
        else if (_bumpVectorSpeedCurrent.y < _ForceYBumperInGround)
        {
            _bumpVectorSpeedCurrent.y = _ForceYBumperInGround;
        }
        moveDirection += _bumpVectorSpeedCurrent;
        Debug.Log(_bumpVectorSpeedCurrent);

        _isBumped = true;
        _rb.AddForce(_bumpVectorSpeedCurrent, ForceMode.Impulse);
    }
    private void DecreaseSpeedBump()
    {
        if (_isBumped == true)
        {
            _bumpVectorSpeedCurrent -= _bumpVectorSpeedCurrent * Time.deltaTime;
        }
    }
}
