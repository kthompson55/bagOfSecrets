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
    public float forwardRotationSpeed;
    public float backwardRotationSpeed;
    public float fallingSpeedModifier;
    public float fallDownAngle;
    public float jumpRotationInfluence;

    private CharacterController controller;
    private bool jumping;
    private float jumpTracking;
    private bool fallen;

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
    }

    void Update()
    {
        if(!fallen)
        {
            HandleMovement();
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
            rotationAmount = (horizontalMove * Time.deltaTime) * forwardRotationSpeed;
        }
        else if (horizontalMove < 0)
        {
            rotationAmount = (horizontalMove * Time.deltaTime) * backwardRotationSpeed;
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
        if(thief.enabled)
        {
            rotationAmount /= thief.fallingReduction;
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

    void Fall()
    {
        sack.GetComponent<Sack>().RollAway();
        fallen = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        controller.enabled = false;
    }

    public void EnablePowerUp(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.THIEF:
                Debug.Log("Thief enabled");
                thief.enabled = true;
                break;
            case Pickup.PickupType.MURDERER:
                Debug.Log("Murderer enabled");
                murderer.enabled = true;
                break;
            case Pickup.PickupType.CHEATER:
                Debug.Log("Cheater enabled");
                cheater.enabled = true;
                break;
            case Pickup.PickupType.ADDICT:
                Debug.Log("Addict enabled");
                addict.enabled = true;
                break;
            case Pickup.PickupType.LIAR:
                Debug.Log("Liar enabled");
                liar.enabled = true;
                break;
        }
    }
}
