using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMovement;
    float verticalMovement;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camOrientation;
    [SerializeField] Transform playeBody;
    [SerializeField] Text massDisplay;
    [SerializeField] Text jumpDisplay;

    [Header("Player Physics")]
    [SerializeField] float playerMass = 10f;
    [SerializeField] float minPlayerMass = 1f;

    [SerializeField] Vector3 playerScale;
    [SerializeField] float playerHeight = 1f;

    [SerializeField] bool loseMass = true;
    [SerializeField] bool loseScale = false;

    [Header("Physics")]
    [SerializeField] float gravityMultiplyer = 0.2f;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    //[SerializeField] float drawingSpeed = 4f;
    //float drawingslow = 0f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] public float jumpForce = 200f;
    [SerializeField] public float aerialJumpForce = 150f;

    [SerializeField] public float jumpChargeSpeed = 0.01f;
    [SerializeField] public float jumpChargeStrenght = 0.15f;
    [SerializeField] public float jumpChargeMaxMass = 0.45f;
    [SerializeField] public float jumpChargeMaxTime = 100f;

    [Header("Spells")]
    [SerializeField] public float leapForce = 200f;
    [SerializeField] public float leapForceAir = 175f;

    private float jumpCharge;
    private bool jumpedLastFrame;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    [SerializeField] float groundDrag = 3f;
    [SerializeField] float airDrag = 0.5f;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    public bool Sloped;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Rigidbody rb;
    RaycastHit slopeHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerMass = Mathf.Clamp(playerMass, 1f, 100f);
        playerScale = new Vector3(1, 1, 1);
        jumpChargeSpeed = (playerMass * jumpChargeMaxMass) / jumpChargeMaxTime;

    }

    private void FixedUpdate()
    {
        PlayerStateCheck();
        MyInput();
        ControlDrag();
        MovePlayer();
        PlayerPhysicsUpdate();
        ControlSpeed();

        if (!isGrounded)
            ControlGravity();

        if (Input.GetKey(jumpKey))
            JumpCharging();
        else
            JumpRelease();
    }

    void PlayerStateCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Sloped = false;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
                Sloped  = true;
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void ControlSpeed()
    {
        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    void ControlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    void MovePlayer()
    {
        if (isGrounded && !Sloped)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && Sloped)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    void ControlGravity()
    {
        rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);
    }

    void PlayerPhysicsUpdate()
    {
        rb.mass = playerMass;
        SetFloatText(playerMass, massDisplay, 1);
        playeBody.transform.localScale = playerScale;
    }

    void JumpCharging()
    {
        jumpedLastFrame = true;
        float scale = (playerMass * jumpChargeMaxMass) / jumpCharge;
        if (jumpCharge < (playerMass * jumpChargeMaxMass))
            jumpCharge += jumpChargeSpeed;
        SetFloatText(jumpCharge, jumpDisplay, scale);
    }

    void JumpRelease()  
    {
        if (loseMass)
            playerMass -= jumpCharge/2;
        if (loseScale)
            playerScale = new Vector3(playerMass , playerMass , playerMass);
        if (playerMass < minPlayerMass)
            playerMass = minPlayerMass;
        if (jumpedLastFrame)
        {
            Jump();
            jumpChargeSpeed = (playerMass * jumpChargeMaxMass) / jumpChargeMaxTime;
        }
        jumpedLastFrame = false;
        jumpCharge = 0;
        SetFloatText(jumpCharge, jumpDisplay, 1);
    }
    

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * (jumpForce * jumpCharge * jumpChargeStrenght * playerMass), ForceMode.Impulse);
        }
        else if(playerMass > 1)
        {
            rb.AddForce(transform.up * (aerialJumpForce * jumpCharge * jumpChargeStrenght * playerMass), ForceMode.Impulse);
        }
 
    }

    public void Leap()
    {
        if (isGrounded)
        {
            rb.AddForce(camOrientation.forward.normalized * (leapForce * 5), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(camOrientation.forward.normalized * (leapForceAir * 5), ForceMode.Impulse);
        }

    }

    public void SetFloatText(float text, Text sprite, float scale)
    {
        string Stext;
        Stext = text.ToString("0.00");
        sprite.text = Stext;
        //sprite.fontSize = (int)scale * 25;
    }

}