using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterController2D : MonoBehaviour 
{
    public float moveSpeed;
    public float jumpHeight;
    public float jumpSpeed;

    private CharacterController controller;
    private Rigidbody rigidbody;
    private bool jumping;
    private float jumpTracking;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        jumping = false;
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
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

        Vector3 moveVector = new Vector3(horizontalMove, verticalMove, 0);
        controller.Move(moveVector);
    }
}
