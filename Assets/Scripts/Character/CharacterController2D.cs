using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Thief))]
[RequireComponent(typeof(Murderer))]
[RequireComponent(typeof(Cheater))]
[RequireComponent(typeof(Addict))]
[RequireComponent(typeof(Liar))]
public class CharacterController2D : MonoBehaviour 
{
    public GameObject sack;
    public float moveSpeed;
    public float backwardMoveSpeed;
    public float jumpHeight;
    public float jumpSpeed;
    public float movementRotationSpeed;
    public float rotationSpeed;
    public float fallingSpeedModifier;
    public float fallDownAngle;
    public float jumpRotationInfluence;

    private CharacterController controller;
    private bool jumping;
    private float jumpTracking;
    private bool fallen;
    private float previousGroundedY;
    private int numPowerUps;

    // power-ups
    private Thief thief;
    private Murderer murderer;
    private Cheater cheater;
    private Addict addict;
    private Liar liar;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumping = false;
        fallen = false;
        previousGroundedY = transform.position.y;

        thief = GetComponent<Thief>();
        murderer = GetComponent<Murderer>();
        cheater = GetComponent<Cheater>();
        addict = GetComponent<Addict>();
        liar = GetComponent<Liar>();

        thief.enabled = false;
        murderer.enabled = false;
        cheater.enabled = false;
        addict.enabled = false;
        liar.enabled = false;

        numPowerUps = 0;
    }

    void Update()
    {
        if(!fallen)
        {
            HandleMovement();
            if(cheater.enabled && Input.GetButton("Cheat") && cheater.CanCheat())
            {
                transform.rotation = Quaternion.identity;
                cheater.Cheat();
            }
        }
    }

    void LateUpdate()
    {
        if(transform.position.z != 44.2f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 44.2f);
        }
    }

    void HandleMovement()
    {
        // move faster forward than backwards
        float horizontalMove = Input.GetAxis("Horizontal");
        if (horizontalMove > 0)
        {
            horizontalMove *= moveSpeed * Time.deltaTime;
        }
        else
        {
            horizontalMove *= backwardMoveSpeed * Time.deltaTime;
        }

        // only jump while on the ground, fall otherwise
        float verticalMove = 0;
        if (controller.isGrounded && !jumping)
        {
            if (Input.GetButton("Jump"))
            {
                Vector3 jumpForce = new Vector3(0, jumpHeight, 0);
                jumpTracking = 0;
                jumping = true;
            }
        }
        else if (jumping)
        {
            float jumpShift = jumpSpeed * Time.deltaTime;
            jumpTracking += jumpShift;
            verticalMove = jumpShift;
            if (jumpTracking >= jumpHeight)
            {
                jumping = false;
            }
        }
        else
        {
            verticalMove = Physics.gravity.y * Time.deltaTime;
        }

        // rotation and falling over
        float rotationAmount = 0;
        // rotate based off the type of movement made this frame
        if (horizontalMove > 0)
        {
            rotationAmount = (horizontalMove * Time.deltaTime) * movementRotationSpeed;
        }
        else if (horizontalMove < 0)
        {
            rotationAmount = (horizontalMove * Time.deltaTime) * movementRotationSpeed;
        }
        else if(transform.rotation.eulerAngles.z != 0)
        {
            // rotate based off current rotation
            if (transform.rotation.eulerAngles.z < 180)
            {
                rotationAmount = fallingSpeedModifier * transform.rotation.eulerAngles.z * Time.deltaTime;
            }
            else if (transform.rotation.eulerAngles.z > 180)
            {
                rotationAmount = fallingSpeedModifier * (transform.rotation.eulerAngles.z - 360) * Time.deltaTime;
            }
        }
        if(verticalMove > 0)
        {
            rotationAmount *= verticalMove * jumpRotationInfluence;
        }

        if (!jumping && previousGroundedY != transform.position.y)
        {
            if(previousGroundedY < transform.position.y)
            {
                rotationAmount -= Mathf.Sign(horizontalMove) * Mathf.Abs(transform.position.y - previousGroundedY) * Time.deltaTime;
            }
            else
            {
                rotationAmount += Mathf.Sign(horizontalMove) * Mathf.Abs(transform.position.y - previousGroundedY) * Time.deltaTime;
            }
        }

        float rotationInput = Input.GetAxis("Balance");
        if(rotationInput != 0)
        {
            rotationAmount -= rotationInput * Time.deltaTime * rotationSpeed;
        }

        if(thief.enabled)
        {
            rotationAmount /= thief.fallingReduction;
        }
        if(numPowerUps > 0)
        {
            rotationAmount += (.5f * rotationAmount) + (numPowerUps * Time.deltaTime);
        }
        Quaternion currentFrameRotation = Quaternion.Euler(0, 0, rotationAmount);

        // apply movement and rotation
        if(addict.enabled)
        {
            horizontalMove *= addict.speedIncrease;
        }
        Vector3 moveVector = new Vector3(horizontalMove, verticalMove, 0);
        controller.Move(moveVector);
        transform.Rotate(currentFrameRotation.eulerAngles);

        // check for failure
        if (transform.rotation.eulerAngles.z > fallDownAngle && transform.rotation.eulerAngles.z < (360-fallDownAngle))
        {
            Fall();
        }
    }

    public void Fall()
    {
        sack.GetComponent<Sack>().RollAway();
        fallen = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        controller.enabled = false;
        StartCoroutine("ReloadLevel");
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(5);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void EnablePowerUp(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.THIEF:
                thief.enabled = true;
                break;
            case Pickup.PickupType.MURDERER:
                murderer.enabled = true;
                break;
            case Pickup.PickupType.CHEATER:
                cheater.enabled = true;
                break;
            case Pickup.PickupType.ADDICT:
                addict.enabled = true;
                break;
            case Pickup.PickupType.LIAR:
                liar.enabled = true;
                break;
        }
        numPowerUps++;
    }

    public void DisablePowerUp(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.THIEF:
                thief.enabled = true;
                break;
            case Pickup.PickupType.MURDERER:
                murderer.enabled = true;
                break;
            case Pickup.PickupType.CHEATER:
                cheater.enabled = true;
                break;
            case Pickup.PickupType.ADDICT:
                addict.enabled = true;
                break;
            case Pickup.PickupType.LIAR:
                liar.enabled = true;
                break;
        }
        numPowerUps--;
    }

    public bool HasIllegalSecrets()
    {
        return thief.enabled || murderer.enabled || addict.enabled;
    }

    public bool IsMurderer()
    {
        return murderer.enabled;
    }

    public bool CanMurder()
    {
        return murderer.CanKill() && murderer.enabled;
    }

    public void Murder()
    {
        murderer.Kill();
    }

    public bool CanLie()
    {
        return liar.CanLie() && liar.enabled;
    }

    public void Lie()
    {
        liar.Lie();
    }
}
