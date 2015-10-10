using UnityEngine;
using System.Collections;

public class Cheater : MonoBehaviour 
{
    public int numUses;
    private int remainingUses;

    void Start()
    {
        remainingUses = numUses;
    }

    public void Cheat()
    {
        remainingUses--;
    }

    public bool CanCheat()
    {
        return remainingUses > 0;
    }
}
