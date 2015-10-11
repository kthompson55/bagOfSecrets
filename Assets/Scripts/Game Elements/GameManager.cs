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
    public Pickup[] pickupTypes;
    public ConversationPartner[] partners;
    public float policeSpawnCooldown;
    public int policePerSpawn;
    public Canvas conversationUI;

    private CharacterController2D playerController;
    private List<ConversationPartner> remainingCharacters;
    private List<PoliceOfficer> officers;
    private bool ableToSpawnPolice;
    private bool conversing;
    private Image selfBackground;
    private Image otherBackground;
    private Text dialogueBox;
    private Vector3[] optionLocations = new Vector3[4];
    private ConversationPartner currentPartner;
    private int currentConversationLine;

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
        conversing = false;
        remainingCharacters = new List<ConversationPartner>(partners);
        officers = new List<PoliceOfficer>();
    }
	
	void Update () 
    {
        // spawn police
	    if(playerController.HasIllegalSecrets() && ableToSpawnPolice)
        {
            for(int i = 0; i < policePerSpawn; i++)
            {
                Vector3 randomLocation = GetRandomPoliceSpawnLocation();
                GameObject newOfficer = Instantiate(policePrefab, randomLocation, Quaternion.identity) as GameObject;
                newOfficer.GetComponent<PoliceOfficer>().target = playerController.gameObject;
                newOfficer.GetComponent<PoliceOfficer>().gameManager = this;
                officers.Add(newOfficer.GetComponent<PoliceOfficer>());
                StartCoroutine("PoliceSpawnWait");
            }
        }
        if(Input.GetButtonDown("Lie") && playerController.CanLie())
        {
            playerController.Lie();
            while(officers.Count > 0)
            {
                PoliceOfficer toDestroy = officers[0];
                Destroy(officers[0].gameObject);
                officers.Remove(toDestroy);
            }
        }
        // halt gameplay for conversation
        if(conversing)
        {
            ableToSpawnPolice = false;
            if(Input.GetButtonDown("Progress"))
            {
                if (currentConversationLine >= currentPartner.conversation.Length)
                {
                    EndConversation();
                }
                else
                {
                    dialogueBox.text = currentPartner.conversation[currentConversationLine];
                    if (currentPartner.playerSpeaking[currentConversationLine])
                    {
                        selfBackground.gameObject.SetActive(true);
                        otherBackground.gameObject.SetActive(false);
                    }
                    else
                    {
                        selfBackground.gameObject.SetActive(false);
                        otherBackground.gameObject.SetActive(true);
                    }
                    currentConversationLine++;
                }
            }
        }
	}

    Vector3 GetRandomPoliceSpawnLocation()
    {
        int now = (int)(DateTime.Now.Ticks / 10000);
        System.Random rand = new System.Random(now);
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
        currentPartner = GetRandomConversationPartner();
        // display Conversation UI
        conversationUI.gameObject.SetActive(true);
        // halt police officers
        foreach(PoliceOfficer officer in officers)
        {
            officer.GetComponent<NavMeshAgent>().Stop();
        }
        // set the next pick up item
        GameObject toBeSpawned = null;
        switch (currentPartner.secretType)
        {
            case Pickup.PickupType.ADDICT:
                toBeSpawned = pickupTypes[0].gameObject;
                break;
            case Pickup.PickupType.CHEATER:
                toBeSpawned = pickupTypes[1].gameObject;
                break;
            case Pickup.PickupType.LIAR:
                toBeSpawned = pickupTypes[2].gameObject;
                break;
            case Pickup.PickupType.MURDERER:
                toBeSpawned = pickupTypes[3].gameObject;
                break;
            case Pickup.PickupType.THIEF:
                toBeSpawned = pickupTypes[4].gameObject;
                break;
        }
        Instantiate(toBeSpawned, pickupLocations[(5 - remainingCharacters.Count) - 1].transform.position, Quaternion.identity);
        // start conversation
        conversing = true;
        currentConversationLine = 0;
        dialogueBox.text = currentPartner.conversation[currentConversationLine];
        if (currentPartner.playerSpeaking[currentConversationLine])
        {
            selfBackground.gameObject.SetActive(true);
            otherBackground.gameObject.SetActive(false);
        }
        else
        {
            selfBackground.gameObject.SetActive(false);
            otherBackground.gameObject.SetActive(true);
        }
        currentConversationLine++;
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
        // hide UI
        conversationUI.gameObject.SetActive(false);
        // reenable player movement
        playerController.transform.rotation = Quaternion.identity;
        playerController.GetComponent<CharacterController2D>().enabled = true;
        // resume police movement
        foreach(PoliceOfficer officer in officers)
        {
            officer.GetComponent<NavMeshAgent>().Resume();
        }
        // end conversation
        conversing = false;
        ableToSpawnPolice = true;
    }

    IEnumerator DemoEndConversation()
    {
        yield return new WaitForSeconds(2.5f);
        EndConversation();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
    }

    public void RemoveOfficer(PoliceOfficer officer)
    {
        officers.Remove(officer);
    }
}
