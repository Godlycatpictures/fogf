using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopDestroy : MonoBehaviour
{
    public Rigidbody2D rb;
    public SceneInfo sceneInfo;
    public Animator animator;
    public bool OnHit;
    private float deathTimer = 10f;


    public void Start()
    {

    StartCoroutine(DestrucionTimer());

    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();

    animator.SetBool("OnHit", false);
    animator.SetBool("Flying", true);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    
        animator.SetBool("OnHit", true);
        animator.SetBool("Flying", false);

        StartCoroutine(Destrucion());

    }

    private IEnumerator Destrucion()
    {



        rb.velocity = Vector2.zero; 
        yield return new WaitForSeconds(sceneInfo.TimeScale * 1f);
        

        Destroy(gameObject);

    }

    private IEnumerator DestrucionTimer()
    {

        yield return new WaitForSeconds(sceneInfo.TimeScale * deathTimer);

        Destroy(gameObject);

    }
    void Update(){
               
    animator.speed = sceneInfo.TimeScale;

    }
}