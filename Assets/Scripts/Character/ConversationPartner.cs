using UnityEngine;
using System.Collections;

public class ConversationPartner : MonoBehaviour 
{
    public Pickup.PickupType secretType;
    public string name;
    public string[] conversation;
    public bool[] playerSpeaking;
    public string[] responses = new string[4];
}
