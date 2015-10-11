﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PoliceOfficer : MonoBehaviour 
{
    public GameManager gameManager;
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

    void LateUpdate()
    {
        if(transform.position.z != 44.2f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 44.2f);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        CharacterController2D player = col.gameObject.GetComponent<CharacterController2D>();
        if(player != null)
        {
            if(player.CanMurder())
            {
                gameManager.RemoveOfficer(this);
                Destroy(gameObject);
            }
            else
            {
                player.Fall();
            }
        }
    }
}
