using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public GameObject player;
    public GameObject policePrefab;
    public GameObject[] policeSpawnpoints;
    public float policeSpawnCooldown;

    private CharacterController2D playerController;
    private bool ableToSpawnPolice;

    void Start()
    {
        foreach(Transform child in player.transform)
        {
            if(child.GetComponent<CharacterController2D>() != null)
            {
                playerController = child.GetComponent<CharacterController2D>();
                break;
            }
        }
        ableToSpawnPolice = true;
    }
	
	void Update () 
    {
	    if(playerController.HasSecrets() && ableToSpawnPolice)
        {
            GameObject newOfficer = Instantiate(policePrefab, GetRandomPoliceSpawnLocation(), Quaternion.identity) as GameObject;
            newOfficer.GetComponent<PoliceOfficer>().target = playerController.gameObject;
            StartCoroutine("PoliceSpawnWait");
        }
	}

    Vector3 GetRandomPoliceSpawnLocation()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(0, policeSpawnpoints.Length);
        return policeSpawnpoints[index].transform.position;
    }

    IEnumerator PoliceSpawnWait()
    {
        ableToSpawnPolice = false;
        yield return new WaitForSeconds(policeSpawnCooldown);
        ableToSpawnPolice = true;
    }
}
