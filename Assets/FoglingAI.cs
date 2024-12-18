using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoglingAI : MonoBehaviour
{
    public float detectionRange = 10f;      // Range to detect objects with the tag "lampa" or "unit"
    public float attackCooldown = 3f;      // Cooldown between attacks
    public float windupTime = 1f;          // Delay before launching the attack

    public float wanderSpeed = 2f;
    public float chaseSpeed = 4f;
    public float wanderTime = 2f;          // Time between changing wander directions

    private Animator animator;
    public bool Attacking = false;
    public bool WithinRange = false;
    public bool IsMoving = false;
    public float XVelocity = 0f;

    private Vector2 wanderDirection;
    private Rigidbody2D rb;
    private Transform target;
    private float attackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        // Reduce the attack timer over time
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        // Prevent ALL movement when attacking
        if (Attacking)
        {
            rb.velocity = Vector2.zero; // Ensure enemy is stationary
            animator.SetBool("IsMoving", false); // Ensure "IsMoving" animation stops
            animator.SetFloat("XVelocity", 0f); // Set XVelocity to zero to prevent any movement animations
            return; // Skip all movement logic
        }

        // Scan for the closest target
        target = FindPriorityTarget();

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            WithinRange = distanceToTarget <= detectionRange;
            animator.SetBool("WithinRange", WithinRange);

            if (WithinRange)
            {
                MoveTowardsTarget();
                if (distanceToTarget <= 1f && attackTimer <= 0)
                {
                    StartCoroutine(PrepareAttack());
                }
            }
        }
        else
        {
            // Wander if no targets are found
            animator.SetBool("WithinRange", false);
            Wander();
        }

        // Update Animation Parameters
        UpdateAnimationParameters();
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            wanderDirection = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(wanderTime);
        }
    }

    void Wander()
    {
        if (Attacking) return; // Skip wandering if attacking
        rb.velocity = wanderDirection * wanderSpeed;
    }

    Transform FindPriorityTarget()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Transform closestLampa = null;
        Transform closestUnit = null;
        float closestLampaDistance = Mathf.Infinity;
        float closestUnitDistance = Mathf.Infinity;

        foreach (Collider2D obj in detectedObjects)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);

            if (obj.CompareTag("lampa") && distance < closestLampaDistance)
            {
                closestLampa = obj.transform;
                closestLampaDistance = distance;
            }
            else if (obj.CompareTag("unit") && distance < closestUnitDistance)
            {
                closestUnit = obj.transform;
                closestUnitDistance = distance;
            }
        }

        if (closestLampa != null)
        {
            return closestLampa;
        }
        else if (closestUnit != null)
        {
            return closestUnit;
        }

        return null;
    }

    void MoveTowardsTarget()
    {
        if (Attacking) return; // Skip movement if attacking
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }

    IEnumerator PrepareAttack()
    {
        
        // Lock all movement during attack
        rb.velocity = Vector2.zero;
        Attacking = true; // Set attacking flag
        animator.SetBool("Attacking", true);
        animator.SetBool("IsMoving", false); // Ensure "IsMoving" is false

        // Wait during the windup period
        yield return new WaitForSeconds(windupTime);

        PerformAttack();

        // Reset attack state
        Attacking = false;
        animator.SetBool("Attacking", false);
        animator.SetBool("IsMoving", false); // Ensure "IsMoving" is false after attack
    }

    void PerformAttack()
    {
        // Attack logic here (e.g., damage the target if applicable)
        attackTimer = attackCooldown; // Reset attack cooldown
    }

    void UpdateAnimationParameters()
    {
        if (Attacking) return; // Skip updates during attack, enemy is stationary

        // Update the XVelocity and IsMoving animation parameters
        XVelocity = rb.velocity.x;
        IsMoving = rb.velocity.magnitude > 0.1f; // Enemy is moving if the velocity magnitude is greater than 0.1
        animator.SetBool("IsMoving", IsMoving);
        animator.SetFloat("XVelocity", XVelocity);

        // Debug to confirm movement and velocity state
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}