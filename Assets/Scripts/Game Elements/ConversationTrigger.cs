using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class ConversationTrigger : MonoBehaviour 
{
    public GameManager manager;

    void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<CharacterController2D>() != null)
        {
            manager.StartConversation();
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
    }
}
