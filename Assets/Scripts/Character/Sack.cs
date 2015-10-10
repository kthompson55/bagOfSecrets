using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Sack : MonoBehaviour 
{
    public void RollAway()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        transform.parent = null;
        System.Random rand = new System.Random();
        float xLaunch = rand.Next(-200, 200);
        float yLaunch = rand.Next(-200, 200);
        float zLaunch = rand.Next(-200, 200);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(xLaunch, yLaunch, zLaunch);
    }
}
