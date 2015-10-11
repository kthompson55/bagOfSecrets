using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public GameObject player;
    public GameObject policePrefab;
    public GameObject[] policeSpawnpoints;
    public GameObject[] pickupLocations;
    public ConversationPartner[] partners;
    public float policeSpawnCooldown;
    public Canvas conversationUI;

    private CharacterController2D playerController;
    private List<ConversationPartner> remainingCharacters;
    private bool ableToSpawnPolice;
    private Image selfBackground;
    private Image otherBackground;
    private Text dialogueBox;
    private Vector3[] optionLocations = new Vector3[4];

    void Start()
    {
        foreach(Transform child in conversationUI.transform)
        {
            if(child.name.Contains("PlayerSpeaking"))
            {
                selfBackground = child.gameObject.GetComponent<Image>();
            }
            else if(child.name.Contains("CharacterSpeaking"))
            {
                otherBackground = child.gameObject.GetComponent<Image>();
            }
            else if(child.name.Contains("Locations"))
            {
                int i = 0;
                foreach(Transform location in child)
                {
                    optionLocations[i++] = new Vector3(location.position.x, location.position.y, 0);
                }
            }
            else if(child.name.Contains("TextBox"))
            {
                dialogueBox = child.gameObject.GetComponent<Text>();
            }
        }
        foreach(Transform child in player.transform)
        {
            if(child.GetComponent<CharacterController2D>() != null)
            {
                playerController = child.GetComponent<CharacterController2D>();
                break;
            }
        }
        ableToSpawnPolice = true;
        remainingCharacters = new List<ConversationPartner>(partners);
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

    public void StartConversation()
    {
        // disable player movement
        playerController.GetComponent<CharacterController2D>().enabled = false;
        // choose partner
        ConversationPartner partner = GetRandomConversationPartner();
        dialogueBox.text = partner.name;
        // display Conversation UI
        conversationUI.gameObject.SetActive(true);
    }

    ConversationPartner GetRandomConversationPartner()
    {
        System.Random rand = new System.Random();
        ConversationPartner ret = remainingCharacters[rand.Next(0, remainingCharacters.Count)];
        remainingCharacters.Remove(ret);
        return ret;
    }

    void EndConversation()
    {
        // renable player movement
        playerController.transform.rotation = Quaternion.identity;
        playerController.GetComponent<CharacterController2D>().enabled = true;
    }
}
