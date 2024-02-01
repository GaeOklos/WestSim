using System.Collections;
using System.Collections.Generic;
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

    public void TakeDamage(int _dmgTotake)
    {
        life -= _dmgTotake;
        if (life <= 0) {
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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
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
