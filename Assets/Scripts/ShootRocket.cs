using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRocket : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject Ammo;

    public void Fire()
    {

        Instantiate(Ammo, FirePoint.position, FirePoint.rotation);
    }
}
