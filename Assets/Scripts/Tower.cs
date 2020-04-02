using UnityEngine;

public class Tower : MonoBehaviour
{
    void OnCollisionEnter2D()
    {
        Debug.Log("Hit");
    }

    void OnCollisionExit2D()
    {
        Debug.Log("Exit");
    }
}
