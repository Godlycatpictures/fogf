using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopDestroyer : MonoBehaviour
{
    public float life = 3; // does something dont remove pls
    public bool onHit = false;
    private Animator animator;

    void start(){

    animator = GetComponent<Animator>();
    
    }

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(gloopdestroy());
    }

    IEnumerator gloopdestroy(){
                
        onHit = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }

    void FixedUpdate(){
        animator.SetBool("OnHit", onHit);
    }
}