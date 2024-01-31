using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class SC_Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;

    [Header("Weapon")]
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private int _damageWeapon = 10;
    [SerializeField] private float _rangeWeapon = 100.0f;
    [SerializeField] private VisualEffect _muzzleFlash;
    [SerializeField] private GameObject _muzzlePosition;
    [SerializeField] private VisualEffect _impactEffectEnemyWeapon;
    [SerializeField] private VisualEffect _impactEffectWallWeapon;

    [Header("Fist")]
    [SerializeField] private GameObject _spawnCollider;
    [SerializeField] private int _damageFist = 10;
    [SerializeField] private float _rangeFist = 1.0f;
    [SerializeField] private VisualEffect _muzzleFlashFist;
    [SerializeField] private VisualEffect _impactEffectEnemyFist;
    [SerializeField] private VisualEffect _impactEffectWallFist;

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
            VisualEffect newMuzzle = Instantiate(_muzzleFlash, _muzzlePosition.transform.position, this.transform.rotation);
            newMuzzle.transform.parent = _muzzlePosition.transform;
            newMuzzle.Play();
            Destroy(newMuzzle.gameObject, 1.0f);

            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _rangeWeapon))
            {
                if (hit.collider.gameObject.GetComponent<SC_Enemy>() != null) {
                    hit.collider.gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageWeapon);
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.blue, 2.0f, true);
                    // Debug.Log(hit.transform.name);
                    VisualEffect newImpact = Instantiate(_impactEffectEnemyWeapon, hit.point, Quaternion.LookRotation(hit.normal));
                    newImpact.transform.parent = hit.collider.gameObject.transform;
                    newImpact.Play();
                    Destroy(newImpact.gameObject, 1.0f);
                }
                else {
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.red, 2.0f, true);
                    VisualEffect newImpact = Instantiate(_impactEffectWallWeapon, hit.point, Quaternion.LookRotation(hit.normal));
                    newImpact.Play();
                    Destroy(newImpact.gameObject, 1.0f);
                }
                if (hit.collider.gameObject.GetComponent<BreakableWall>() != null)
                {
                    hit.collider.gameObject.GetComponent<BreakableWall>().isBroken = true;
                }
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
                if (hitColliders[i].gameObject.GetComponent<BreakableWall>() != null)
                {
                    hitColliders[i].gameObject.GetComponent<BreakableWall>().isBroken = true;
                }
                if (hitColliders[i].gameObject.GetComponent <BreakableWallPunch>() != null)
                {
                    hitColliders[i].gameObject.GetComponent<BreakableWallPunch>().isBroken = true;
                }
                if (hitColliders[i].gameObject.GetComponent<ChateauDeau>() != null)
                {
                    hitColliders[i].gameObject.GetComponent<ChateauDeau>().hit++;
                    hitColliders[i].gameObject.GetComponent<ChateauDeau>().Hit();

                }
                i++;
            }
            
        }
    }
}
