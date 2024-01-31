using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SC_Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;
    // Weapon
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private int _damageWeapon = 10;
    [SerializeField] private float _rangeWeapon = 100.0f;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _impactEffectEnemyWeapon;
    [SerializeField] private GameObject _impactEffectWallWeapon;

    // Fist
    [SerializeField] private GameObject _spawnCollider;
    [SerializeField] private int _damageFist = 10;
    [SerializeField] private float _rangeFist = 1.0f;
    [SerializeField] private ParticleSystem _muzzleFlashFist;
    [SerializeField] private GameObject _impactEffectEnemyFist;
    [SerializeField] private GameObject _impactEffectWallFist;

    [SerializeField] private bool _debugMode = false;
    private void Update()
    {
        Shoot();
        FistHit();
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // _muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _rangeWeapon))
            {
                if (hit.collider.gameObject.GetComponent<SC_Enemy>() != null) {
                    hit.collider.gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageWeapon);
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.blue, 2.0f, true);
                    // Debug.Log(hit.transform.name);
                    // Instantiate(_impactEffectEnemyWeapon, hit.point, Quaternion.LookRotation(hit.normal));
                }
                else
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.red, 2.0f, true);
            }
        }
    }

    private void FistHit()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Collider[] hitColliders = Physics.OverlapSphere(_spawnCollider.transform.position, _rangeFist);
            int i = 0;
            if (_debugMode == true) {
                GameObject _sphereTest = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), _spawnCollider.transform.position, Quaternion.identity);
                _sphereTest.transform.localScale = _sphereTest.transform.localScale * 2;
                Destroy(_sphereTest, 1);
            }
            while (i < hitColliders.Length) {
                if (hitColliders[i].gameObject.GetComponent<SC_Enemy>() != null) {
                    hitColliders[i].gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageFist);
                    // Debug.Log(hitColliders[i].gameObject.name);
                    // Instantiate(_impactEffectEnemyFist, hitColliders[i].gameObject.transform.position, Quaternion.LookRotation(hitColliders[i].gameObject.transform.position));
                }
                i++;
            }
        }
    }
}
