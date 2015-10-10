using UnityEngine;
using System.Collections;

public class Murderer : MonoBehaviour 
{
    public int numUses;
    private int remainingKills;

    void Start()
    {
        remainingKills = numUses;
    }

    public bool CanKill()
    {
        return remainingKills > 0;
    }

    public void Kill()
    {
        remainingKills--;
    }
}
