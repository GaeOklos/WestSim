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
    [SerializeField] private bool canShoot = true;
    [SerializeField] private float _fireRateWeapon = 0.4f;
    [SerializeField] private float _TimerCooldown = 0.0f;
    [SerializeField] private Animator _animator;
    private bool _TimerOn = false;

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
    [SerializeField] private Animator punchAnimator;
    [SerializeField] private bool canPunch = false;
    [SerializeField] private float _punchRate = 0f;
    [SerializeField] private float _punchCoolDown = 0f;
    private bool _punchTimerOn = false;

    [SerializeField] private bool _debugMode = false;

    private SC_Movement moveSc;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        moveSc = player.GetComponent<SC_Movement>();
    }

    private void Update()
    {
        CooldownShoot();
        Shoot();
        FistHit();
        FistCoolDown();
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot == true)
        {
            canShoot = false;
            _TimerOn = true;

            VisualEffect newMuzzle = Instantiate(_muzzleFlash, _muzzlePosition.transform.position, this.transform.rotation);
            newMuzzle.transform.parent = _muzzlePosition.transform;
            newMuzzle.Play();
            Destroy(newMuzzle.gameObject, 1.0f);
            _animator.SetTrigger("Reload");

            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _rangeWeapon))
            {
                if (hit.collider.gameObject.GetComponent<SC_EnyShoot>() != null) {
                    hit.collider.gameObject.GetComponent<SC_EnyShoot>().TakeDamage(_damageWeapon);
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.blue, 2.0f, true);
                    // Debug.Log(hit.transform.name);
                    VisualEffect newImpact = Instantiate(_impactEffectEnemyWeapon, hit.point, Quaternion.LookRotation(hit.normal));
                    newImpact.Play();
                    Destroy(newImpact.gameObject, 1.0f);
                }
                else {
                    Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.red, 2.0f, true);
                    VisualEffect newImpact = Instantiate(_impactEffectWallWeapon, hit.point, Quaternion.LookRotation(hit.normal));
                    newImpact.Play();
                    Destroy(newImpact.gameObject, 1.0f);
                }
                if (hit.collider.gameObject.GetComponent<NewBreakable>() != null)
                {
                    hit.collider.gameObject.GetComponent<NewBreakable>().BreakShoot();
                }
            }
        }
    }

    private void CooldownShoot()
    {
        if (_TimerOn == true) {
            _TimerCooldown += Time.deltaTime;
            if (_TimerCooldown >= _fireRateWeapon)
            {
                _TimerOn = false;
                _TimerCooldown = 0.0f;
                canShoot = true;
            }
        }
    }

    private void FistHit()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && canPunch)
        {
            _punchTimerOn = true;
            canPunch = false;
            if (moveSc._Octane_isUsed)
            {
                punchAnimator.Play("BigPunch");
            } else if (moveSc._Octane_isUsed is false)
            {
                punchAnimator.Play("Punch");
            }
            Collider[] hitColliders = Physics.OverlapSphere(_spawnCollider.transform.position, _rangeFist);
            int i = 0;
            if (_debugMode == true) {
                GameObject _sphereTest = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), _spawnCollider.transform.position, Quaternion.identity);
                _sphereTest.transform.localScale = _sphereTest.transform.localScale * 2;
                Destroy(_sphereTest, 1);
            }
            while (i < hitColliders.Length) {
                if (moveSc._Octane_isUsed)
                {
                    Debug.Log("Big Punch");
                    if (hitColliders[i].gameObject.GetComponent<SC_Enemy>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageFist);
                        // Debug.Log(hitColliders[i].gameObject.name);
                        // Instantiate(_impactEffectEnemyFist, hitColliders[i].gameObject.transform.position, Quaternion.LookRotation(hitColliders[i].gameObject.transform.position));
                    }
                    if (hitColliders[i].gameObject.GetComponent<NewBreakable>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<NewBreakable>().BreakBigPunch();
                    }
                    if (hitColliders[i].gameObject.GetComponent<ChateauDeau>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<ChateauDeau>().hit = hitColliders[i].gameObject.GetComponent<ChateauDeau>().hitNeeded;
                        hitColliders[i].gameObject.GetComponent<ChateauDeau>().Hit();

                    }
                } else if (moveSc._Octane_isUsed is false)
                {
                    Debug.Log("Normal Punch");
                    if (hitColliders[i].gameObject.GetComponent<SC_Enemy>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageFist);
                        // Debug.Log(hitColliders[i].gameObject.name);
                        // Instantiate(_impactEffectEnemyFist, hitColliders[i].gameObject.transform.position, Quaternion.LookRotation(hitColliders[i].gameObject.transform.position));
                    }
                    if (hitColliders[i].gameObject.GetComponent<NewBreakable>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<NewBreakable>().BreakPunch();
                    }
                    if (hitColliders[i].gameObject.GetComponent<ChateauDeau>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<ChateauDeau>().hit++;
                        hitColliders[i].gameObject.GetComponent<ChateauDeau>().Hit();

                    }
                }
                i++;
            }
            
        }
    }

    private void FistCoolDown()
    {
        if (_punchTimerOn == true)
        {
            _punchCoolDown += Time.deltaTime;
            if (_punchCoolDown >= _punchRate)
            {
                _punchTimerOn = false;
                _punchCoolDown = 0.0f;
                canPunch = true;
            }
        }
    }
}
