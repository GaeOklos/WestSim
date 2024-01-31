using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Capacity : MonoBehaviour
{
    [SerializeField] private bool _capacity_isUsed = false;
    private float _capacityCooldownTimer = 0.0f;
    [SerializeField] private float _capacityDurationTimer = 5.0f;
    public float _capacityLoadValue = 0.0f;
    public Slider bumperUICD;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _prefabBumperCpt;
    public LayerMask shootLayer;
    private GameObject _PreviousBumper;


    // Update is called once per frame
    void Update()
    {
        CheckUseCapacity();
        CapacityCooldown();
    }

    private void CheckUseCapacity()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            if (_capacity_isUsed == false) {
                RaycastHit hit;
                if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 100.0f, shootLayer))
                {
                    if (hit.collider.gameObject != this) {
                        if (_PreviousBumper)
                            Destroy(_PreviousBumper);
                        // Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.red, 2.0f, true);
                        // Debug.Log(hit.transform.name);
                        _capacity_isUsed = true;
                        _capacityCooldownTimer = 0.0f;
                        _capacityLoadValue = 0.0f;
                        _PreviousBumper = Instantiate(_prefabBumperCpt, hit.point, Quaternion.LookRotation(hit.normal, Vector3.left));
                        _PreviousBumper.transform.parent = hit.collider.gameObject.transform;
                    }
                }
            }
        }
    }
    private void CapacityCooldown()
    {
        bumperUICD.value = _capacityLoadValue;
        if (_capacity_isUsed == true)
        {
            _capacityCooldownTimer += Time.deltaTime;
            if (_capacityCooldownTimer >= _capacityDurationTimer)
            {
                _capacity_isUsed = false;
                _capacityCooldownTimer = 0.0f;
            }
            _capacityLoadValue = _capacityCooldownTimer / _capacityDurationTimer;
        }
        else
        {
            _capacityLoadValue = 1f;
        }
    }
}
