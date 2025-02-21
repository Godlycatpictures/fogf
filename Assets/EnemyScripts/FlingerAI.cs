using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingerAi : MonoBehaviour
{
    public SceneInfo sceneInfo;
    public float roamingSpeed = 2f;
    public float roamTime = 2f;
    public float roamPauseTime = 1f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float cooldownTime = 3f;

    private Animator animator;
    public Rigidbody2D rb;
    public Transform FirePoint;
    public Transform targetUnit;
    public GameObject gloop;

    public bool isRoaming = true;
    public bool attackUnit = false;
    public bool isMoving = false;
    public bool hasAttacked = false;

    private Vector2 roamingDirection;
    private float roamTimer;
    private float pauseTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        roamTimer = roamTime;
        roamingDirection = GetRandomDirection();

        targetUnit = null; // Prevent "Variable not assigned" error
    }

    void Update()
    {
        animator.speed = sceneInfo.TimeScale;
        roamingSpeed = sceneInfo.TimeScale * 2f;

        // Find the closest target
        targetUnit = FindClosestTargetWithTag("unit", detectionRange);
        attackUnit = targetUnit != null;

        // Attack if conditions are met
        if (attackUnit && isRoaming && !hasAttacked && Vector2.Distance(transform.position, targetUnit.position) <= attackRange)
        {
            hasAttacked = true; // Prevent multiple attacks
            StartCoroutine(Attack());
        }

        if (isRoaming)
        {
            Roam();
        }

        UpdateAnimator();
    }

    private void Roam()
    {
        if (roamTimer > 0)
        {
            roamTimer -= Time.deltaTime;
            rb.velocity = roamingDirection * roamingSpeed;
            isMoving = true;
        }
        else
        {
            rb.velocity = Vector2.zero;
            isMoving = false;

            if (pauseTimer > 0)
            {
                pauseTimer -= Time.deltaTime;
            }
            else
            {
                pauseTimer = roamPauseTime;
                roamTimer = roamTime;
                roamingDirection = GetRandomDirection();
            }
        }
    }

    private IEnumerator Attack()
    {
        if (targetUnit == null) yield break; // Stop if no valid target

        FirePoint.localPosition = Vector3.zero; // Ensures FirePoint stays where it should duh

        Vector2 direction = (targetUnit.position - FirePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate projectile at FirePoint's WORLD position (bugfix)
        var bullet = Instantiate(gloop, FirePoint.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 5f;

        isRoaming = false; // Stop moving while attacking

        yield return new WaitForSeconds(1f); // Small attack delay

        StartCoroutine(CooldownAndReturnToRoaming()); // Trigger cooldown
    }

    private IEnumerator CooldownAndReturnToRoaming()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown

        hasAttacked = false; // Reset attack state AFTER cooldown
        isRoaming = true; // Resume roaming
    }

    private Transform FindClosestTargetWithTag(string tag, float range)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < minDistance && distance <= range)
            {
                closest = target.transform;
                minDistance = distance;
            }
        }
        return closest;
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Attacking", attackUnit);
        animator.SetFloat("XVelocity", rb.velocity.x);
        animator.SetBool("IsMoving", isMoving);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}