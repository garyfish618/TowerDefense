using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float shootSpeed;
    private bool isShooting;

    public Tower()
    {
        isShooting = false;
    
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

        }

        //Ignore towers
        if (col.gameObject.tag == "Enemy") 
        {
            GameObject enemy = col.gameObject;

            Vector3 targetPosition = enemy.transform.position;
            targetPosition.z = 0;

            targetPosition.x = targetPosition.x - transform.position.x;
            targetPosition.y = targetPosition.y - transform.position.y;

            float angle = (Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            StartCoroutine(ShootEnemy(transform.gameObject.tag));
        }
        

        
     
    }

    IEnumerator ShootEnemy(string tag)
    {
        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        if (tag == "Turret") {
                    
            transform.GetChild(0).gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            transform.GetChild(0).gameObject.SetActive(false);

            yield return new WaitForSeconds(shootSpeed);

        }

        
        if (tag == "Missile")
        {

            transform.gameObject.GetComponent<ShootRocket>().Fire();
            yield return new WaitForSeconds(shootSpeed);

        }

        isShooting = false;


    }


}
