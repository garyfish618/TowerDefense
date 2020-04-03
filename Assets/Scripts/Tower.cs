using System.Collections.Specialized;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float speed;

    void OnCollisionStay2D(Collision2D col)
    {
        GameObject enemy = col.gameObject;

        Vector3 targetPosition = enemy.transform.position;
        targetPosition.z = 0;

        targetPosition.x = targetPosition.x - transform.position.x;
        targetPosition.y = targetPosition.y - transform.position.y;

        float angle = (Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
     
    }

    void OnCollisionExit2D()
    {
        Debug.Log("Exit");
    }
}
