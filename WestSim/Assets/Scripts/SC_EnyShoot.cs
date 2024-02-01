using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SC_EnyShoot : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject _positionStartShot;

    public float bulletSpeed = 5;

    public LayerMask whatIsPlayer;


    [Header("Attack")]
    public float timeBetweenAttacks = 1f;
    bool alreadyAttacked;
    public GameObject projectile;

    [Header("Range")]
    public float attackRange;
    public bool playerInAttackRange;

    [SerializeField] private int life = 1;
    [SerializeField] private int objectiveNb = 0;
    [SerializeField] private float _minTimeBetweenAttacks = 0.5f;
    [SerializeField] private float _maxTimeBetweenAttacks = 1.5f;

    public bool isArmored = false;
    public GameObject armor;
    public bool isShielded = false;
    public GameObject shield;
    private float punchCD;

    private bool canPunch = false;
    private float _punchCoolDown = 0f;
    private bool _punchTimerOn = false;
    private float _punchRate = 0f;

    private void Start()
    {
        canPunch = true;
        if (isArmored)
        {
            armor.SetActive(true);
        }
        if (isShielded)
        {
            shield.SetActive(true);
        }
    }

    public void TakeDamageShoot(int _dmgTotake)
    {
        if (isArmored is false)
        {
            life -= _dmgTotake;
            if (life <= 0)
            {
                GameObject gb = GameObject.FindGameObjectWithTag("ObjectiveManager");
                ObjectiveManager manager = gb.GetComponent<ObjectiveManager>();
                if (objectiveNb == 1)
                {
                    manager.firstObjective = true;
                }
                else if (objectiveNb == 2)
                {
                    manager.secondObjective = true;
                }
                else if (objectiveNb == 3)
                {
                    manager.thirdObjective = true;
                }
                else if (objectiveNb == 4)
                {
                    manager.fourthObjective = true;
                }
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void TakeDamagePunch(int _dmgTotake, float _cooldown)
    {
        _punchRate = _cooldown;
        _punchTimerOn = true;
        if (canPunch is true)
        {
            if (isShielded is true)
            {
                isShielded = false;
            }
            else if (isShielded is false)
            {
                life -= _dmgTotake;
                if (life <= 0)
                {
                    GameObject gb = GameObject.FindGameObjectWithTag("ObjectiveManager");
                    ObjectiveManager manager = gb.GetComponent<ObjectiveManager>();
                    if (objectiveNb == 1)
                    {
                        manager.firstObjective = true;
                    }
                    else if (objectiveNb == 2)
                    {
                        manager.secondObjective = true;
                    }
                    else if (objectiveNb == 3)
                    {
                        manager.thirdObjective = true;
                    }
                    else if (objectiveNb == 4)
                    {
                        manager.fourthObjective = true;
                    }
                    Destroy(transform.parent.gameObject);
                }
            }
            canPunch = false;
        }
    }

    private void PunchCD()
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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        PunchCD();

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
            AttackPlayer();

    }

    private void AttackPlayer()
    {
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

        if (!alreadyAttacked)
        {
            // Lance le projectile sur le joueur
            // Rigidbody rb = Instantiate(projectile, _positionStartShot.transform.position, _positionStartShot.transform.rotation).GetComponent<Rigidbody>();
            // rb.AddForce(_positionStartShot.transform.forward * 10f, ForceMode.Impulse);
            // rb.AddForce(_positionStartShot.transform.up * 8f, ForceMode.Impulse);
            var bullet = Instantiate(projectile, _positionStartShot.transform.position, _positionStartShot.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = _positionStartShot.transform.forward * bulletSpeed;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), Random.Range(_minTimeBetweenAttacks, _maxTimeBetweenAttacks));
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
