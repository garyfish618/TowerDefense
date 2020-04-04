using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float FlySpeed;
    public Rigidbody2D rig;
    public int RocketDamage;

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = transform.up * FlySpeed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Missile" || col.gameObject.tag == "Turret" || col.gameObject.tag == "Projectile")
        {

            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), transform.gameObject.GetComponent<Collider2D>());

        }

    }

    void OnCollisionStay2D(Collision2D col)
    {

        if (col.gameObject.tag == "Missile" || col.gameObject.tag == "Turret" || col.gameObject.tag == "Projectile")
        {

            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), transform.gameObject.GetComponent<Collider2D>());

        }


    }

}
