using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirePoint : MonoBehaviour
{
    public Vector2 aimTarget;
    public Transform target;
    public GameObject obj;
    public Rigidbody2D rb;
    public Rigidbody2D rb2;

    public void Start()
    {

        target = obj.GetComponent<FlingerAi>().targetUnit;
    }

    // Update is called once per frame
    private void Update() // modifierad Weapon.cs
    {
        rb.position = rb2.position; // eggsactly

        aimTarget = target.position;
        Vector2 aimDirection = aimTarget - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;
    }
}