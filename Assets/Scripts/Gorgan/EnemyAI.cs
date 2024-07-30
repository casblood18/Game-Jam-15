using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float resetRadius = 5f;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float moveSpeed = 3f;
    public GameObject projectilePrefab;

    private Transform player;
    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private Animator animator;
    private bool canShoot = true;
    public float shootDelay = 0.5f;

    public float health = 100;

    [Space(10)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color damageColor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (waypoints.Length > 0)
        {
            MoveToWaypoint();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRadius)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer < detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            ResetToWaypoints();
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        UpdateAnimation(direction);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void ResetToWaypoints()
    {
        if (!isChasing)
        {
            MoveToWaypoint();
            animator.SetBool("IsWalking", true);
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("IsAttacking", false);
        isChasing = true;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        UpdateAnimation(direction);
    }

    void AttackPlayer()
    {
        if (canShoot)
        {
            animator.SetBool("IsAttacking", true);
            StartCoroutine(ShootProjectileWithDelay());
        }
    }
    IEnumerator ShootProjectileWithDelay()
    {
        canShoot = false;
        ShootProjectile();
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            MobProjectile projectileScript = projectile.GetComponent<MobProjectile>();
            projectileScript.ProjectileDirection(player);
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }
        //else
        //{
        //    animator.SetFloat("MoveX", 0);
        //    animator.SetFloat("MoveY", 0);
        //}
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, resetRadius);
    }

    public void DamageTaken(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageSprite()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = normalColor;
    }
}