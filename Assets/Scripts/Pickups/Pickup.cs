using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        THIEF,
        MURDERER,
        CHEATER,
        ADDICT,
        LIAR,
		NONE
    };

    public PickupType type;

    void OnTriggerEnter(Collider col)
    {
        CharacterController2D player = col.gameObject.GetComponent<CharacterController2D>();
        if (player != null)
        {
            player.EnablePowerUp(type);
            Destroy(gameObject);
        }
    }
}
