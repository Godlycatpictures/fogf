using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angler_AI : MonoBehaviour
{
    public float speed = 1f;
    public Transform unit;
    public Transform lamp;
    public Transform extractor;
    public Transform generator;
    public Animator animator;
    private Rigidbody2D rb;

    public float range = 10f;

    public float attackRange = 2f;

    private float distanceToUnit;

    public float cooldown;

    public bool nearLamp;

    public bool lurking;

    public bool hasAttacked;

    public bool attackUnit;


    void Start()
    {
        unit = GameObject.FindGameObjectWithTag("unit").transform;
        lamp = GameObject.FindGameObjectWithTag("lampa").transform;

        rb = GetComponent<Rigidbody2D>();
    }

    void CheckEnvironment()
    {
        if(cooldown == 0)
        {
        if(distanceToUnit <= range && lurking == false)
        {
            //Om den inte lurkar och en unit är inom range
            RunAway();

        }if(nearLamp == true){
        //Starta lurk
        lurking = true;

        }else
        {
            //Här ska den gå i en random riktning

            CheckEnvironment();

        
        }
        }else{

            RunAway();

        }
    }

    void RunAway (){

        //Spring i en random riktning ifrån en unit efter att ha attackerat

    }

    void AttackUnit ()
    {

        attackUnit = true;

    }

    void Cooldown ()
    {

        

    }
 
    Vector2 movement;

    void FixedUpdate()
    {
        movement.x = rb.velocity.x;
        animator.SetFloat("xVelocity", movement.x);
        animator.SetBool("lurking", lurking);
        animator.SetBool("nearLamp", nearLamp);
        animator.SetBool("attackUnit", attackUnit);
        animator.SetBool("hasAttacked", hasAttacked);

        if(lurking == true)
        {

            if(distanceToUnit >= attackRange)
            {

                AttackUnit();

            }
        }
    }
}
