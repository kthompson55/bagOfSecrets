using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour 
{
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<CharacterController2D>() != null)
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
