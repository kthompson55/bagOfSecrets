using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PoliceOfficer : MonoBehaviour 
{
    public GameObject target;

    private NavMeshAgent agent;
    private Vector3 lastPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastPosition = target.transform.position;
    }
	
	// Update is called once per frame
	void Update () 
    {
        agent.destination = target.transform.position;
	}

    void OnTriggerEnter(Collider col)
    {
        CharacterController2D player = col.gameObject.GetComponent<CharacterController2D>();
        if(player != null)
        {
            if(player.IsMurderer() && player.CanMurder())
            {
                player.Murder();
                Destroy(gameObject);
            }
            else
            {
                player.Fall();
            }
        }
    }
}
