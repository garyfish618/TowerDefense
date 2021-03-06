﻿using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float shootSpeed;
    public int turretDamage;
    private bool isShooting;
    private GameObject target;
    private AudioManager aud;
    private PersistenceController contr;
    void Start()
    {
        isShooting = false;
        aud = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        contr = PersistenceController.Instance;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Enemy")
        {

            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), transform.gameObject.GetComponent<Collider2D>());

        }

        
    }

    void OnCollisionStay2D(Collision2D col)
    {
        

        if (col.gameObject.tag != "Enemy")
        {

            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), transform.gameObject.GetComponent<Collider2D>());
            return;
        }
        
        //Ignore towers and ensure we are focusing on shooting target
        if (col.gameObject.tag == "Enemy" && (col.gameObject == target || target == null) ) 
        {
            //Check if tower lost target and needs one that is already inside
           if (target == null) {

                //If the potential target is a cannon only enemy and is in range of a missile launcher, ignore it
                if (col.gameObject.transform.GetChild(0).tag == "CannonOnlyEnemy" && transform.gameObject.tag == "Missile")
                {
                    return;
                }
                Debug.Log("Gaining target:" + transform.name + "Enemy is: " + col.transform.name);
                target = col.gameObject;
                
                col.gameObject.GetComponent<Enemy>().targeted = true;
            }



            GameObject enemy = col.gameObject;

            Vector3 targetPosition = enemy.transform.position;
            targetPosition.z = 0;

            targetPosition.x = targetPosition.x - transform.position.x;
            targetPosition.y = targetPosition.y - transform.position.y;

            float angle = (Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            StartCoroutine(ShootEnemy(transform.gameObject.tag, col.gameObject.GetComponent<Enemy>()));
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        
        //Target is leaving turret range, will need a new target
        if (col.gameObject == target) {
            target = null;
            Debug.Log("Losing target:" + transform.name + "Enemy is: " + col.transform.name);
            col.gameObject.GetComponent<Enemy>().targeted = false;

        }
    
    }

    IEnumerator ShootEnemy(string tag, Enemy en)
    {
        if(contr.GameOver) {
            yield break;
        }

        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        if (tag == "Turret") {
            
            transform.GetChild(0).gameObject.SetActive(true);
            aud.Play("TurretShot");
            en.ReduceHealth(turretDamage);

            yield return new WaitForSeconds(0.5f);

            transform.GetChild(0).gameObject.SetActive(false);

            yield return new WaitForSeconds(shootSpeed);

        }

        
        if (tag == "Missile")
        {
            aud.Play("RocketShot");
            transform.gameObject.GetComponent<ShootRocket>().Fire();
            yield return new WaitForSeconds(shootSpeed);

        }

        isShooting = false;


    }


}
