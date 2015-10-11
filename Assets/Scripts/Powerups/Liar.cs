using UnityEngine;
using System.Collections;

public class Liar : MonoBehaviour 
{
    public int numUses;
    private int remainingUses;

    void Start()
    {
        remainingUses = numUses;
    }

    public void Lie()
    {
        remainingUses--;
    }

    public bool CanLie()
    {
        return remainingUses > 0;
    }
}
