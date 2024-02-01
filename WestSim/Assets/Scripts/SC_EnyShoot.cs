using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_EnyShoot : MonoBehaviour
{
    public Transform player;

    public LayerMask whatIsPlayer;

    public float health;


    [Header("Attack")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    [Header("Range")]
    public float attackRange;
    public bool playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PC").transform;
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
            AttackPlayer();
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked) {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
//            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
//            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
