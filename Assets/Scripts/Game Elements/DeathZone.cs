using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour 
{
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<CharacterController2D>() != null)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
