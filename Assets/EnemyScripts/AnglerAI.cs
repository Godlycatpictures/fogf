using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerAI : MonoBehaviour
{
    public float roamingSpeed = 2f;
    public float roamTime = 2f;
    public float roamPauseTime = 1f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float runAwaySpeed = 3f;
    public float cooldownTime = 3f;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform targetLamp;
    private Transform targetUnit;

    public bool isRoaming = true;
    public bool lurking = false;
    public bool nearLamp = false;
    public bool attackUnit = false;
    public bool hasAttacked = false;
    public bool isMoving = false;

    private float xVelocity;

    private Vector2 roamingDirection;
    private float roamTimer;
    private float pauseTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        roamTimer = roamTime;
        roamingDirection = GetRandomDirection();
    }

    void Update()
    {

        if (isRoaming)
        {
            Roam();
        }
        else if (lurking)
        {
            Lurk();
        }
        else if (hasAttacked)
        {
            StartCoroutine(CooldownAndReturnToRoaming());
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

        // Check for nearby lamps
        targetLamp = FindClosestTargetWithTag("lampa", detectionRange);
        nearLamp = targetLamp != null;

        if (nearLamp)
        {
            isRoaming = false;
            lurking = true;
            rb.velocity = Vector2.zero;
            isMoving = false;
        }
    }

    private void Lurk()
    {
        // Check if the lamp still exists
        if (targetLamp == null || Vector2.Distance(transform.position, targetLamp.position) > detectionRange)
        {
            lurking = false;
            isRoaming = true;
            return;
        }

        // Check for nearby units
        targetUnit = FindClosestTargetWithTag("unit", attackRange);
        attackUnit = targetUnit != null;

        if (attackUnit)
        {
            lurking = false;
            StartCoroutine(AttackAndRunAway());
        }
    }

    private IEnumerator AttackAndRunAway()
    {
        // Attack
        hasAttacked = true;

        // Simulate attack with a brief delay
        yield return new WaitForSeconds(0.5f);

        // Run away
        if (targetUnit != null)
        {
            Vector2 runDirection = (transform.position - targetUnit.position).normalized;
            rb.velocity = runDirection * runAwaySpeed;
            yield return new WaitForSeconds(1f); // Run for 1 second
        }

        rb.velocity = Vector2.zero;
        hasAttacked = false;

        // Check if the lamp is still nearby
        if (targetLamp == null || Vector2.Distance(transform.position, targetLamp.position) > detectionRange)
        {
            isRoaming = true;
        }
        else
        {
            lurking = true;
        }
    }

    private IEnumerator CooldownAndReturnToRoaming()
    {
        yield return new WaitForSeconds(cooldownTime);
        hasAttacked = false;
        isRoaming = true;
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
        animator.SetBool("lurking", lurking);
        animator.SetBool("nearLamp", nearLamp);
        animator.SetBool("attackUnit", attackUnit);
        animator.SetBool("hasAttacked", hasAttacked);
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("xVelocity", rb.velocity.x);
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