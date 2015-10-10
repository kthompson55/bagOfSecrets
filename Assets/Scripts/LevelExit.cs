using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour 
{
    public string nextLevelName;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<CharacterController2D>() != null)
        {
            Application.LoadLevel(nextLevelName);
        }
    }
}
