using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingerAI : MonoBehaviour

{
    public float detectionRange = 10f;
    public float attackRange = 3f;

    public float wanderSpeed = 2f;
    public float wanderTime = 2f;
    public float idleTime = 2f;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;

    private Animator animator;
    private Transform target;
    private float cooldownTimer;
    private Vector2 wanderDirection;
    private float wanderTimer;
    private bool isIdle;

    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cooldownTimer = 0;
        wanderTimer = 0;
        PickNewWanderDirection();
    }

    void Update()
    {
        // Decrease cooldown timer
        cooldownTimer -= Time.deltaTime;

        // Detect targets
        DetectTarget();

        if (target != null)
        {
            // Move towards the target or attack
            HandleTargetInteraction();
        }
        else
        {
            // Wander around
            Wander();
        }

        // Update animator parameters
        UpdateAnimator();
    }

    void DetectTarget()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D obj in detectedObjects)
        {
            if (obj.CompareTag("lampa") || obj.CompareTag("unit"))
            {
                target = obj.transform;
                return;
            }
        }
        target = null;
    }

    void HandleTargetInteraction()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange)
        {
            // Move towards the target
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * wanderSpeed, rb.velocity.y);

            // Store last horizontal velocity for animator
            animator.SetFloat("LastXVelocity", direction.x);
        }
        else
        {
            // Attack the target if cooldown allows
            rb.velocity = Vector2.zero;

            if (cooldownTimer <= 0)
            {
                Attack();
            }
        }

        animator.SetBool("WithinRange", true);
    }

    void Attack()
    {
        animator.SetBool("Attacking", true);

        // Instantiate projectile
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        cooldownTimer = attackCooldown;
        animator.SetBool("Attacking", false);
    }

    void Wander()
    {
        if (isIdle)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0)
            {
                isIdle = false;
                PickNewWanderDirection();
            }
        }
        else
        {
            wanderTimer -= Time.deltaTime;
            rb.velocity = new Vector2(wanderDirection.x * wanderSpeed, rb.velocity.y);

            if (wanderTimer <= 0)
            {
                isIdle = true;
                wanderTimer = idleTime;
                rb.velocity = Vector2.zero;
            }
        }

        animator.SetBool("WithinRange", false);
    }

    void PickNewWanderDirection()
    {
        wanderDirection = new Vector2(Random.Range(-1f, 1f), 0).normalized;
        wanderTimer = wanderTime;

        // Update animator's LastXVelocity
        animator.SetFloat("LastXVelocity", wanderDirection.x);
    }

    void UpdateAnimator()
    {
        animator.SetFloat("XVelocity", Mathf.Abs(rb.velocity.x));
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection and attack range for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}