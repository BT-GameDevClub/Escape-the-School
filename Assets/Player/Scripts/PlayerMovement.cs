using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    [Header("Trigger Modifiers")]
    [Range(1, 25)]
    public int jumpChecks;
    public float distanceToJump;
    public float inputHoldTime;

    [Header("Raycast Options")]
    public float width;
    public bool showRays;

    [Header("Player Parts")]
    public Transform weaponHolderParent;

    // Component References
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    // Input Trackers
    private PlayerInputActions input;
    private float[] movementInput = new float[2];
    private float jumpInput;
    private bool jumpInputHold;
    private float jumpInputHoldTime;
    private float jumpDirection;

    // Movement Trackers
    private bool touchingGround;
    private bool jumpPerformed;
    private bool doubleJumpPerformed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // Setup for the new Input System
        input = new PlayerInputActions();
        input.Game.Move.performed += MovePerformed;
        input.Game.Move.canceled += MoveCancelled;
        input.Game.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();
        input.Game.Jump.canceled += JumpCancelled;
    }

    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        movementInput[0] = ctx.ReadValue<float>();
        anim.SetBool("Move", true);
    }

    private void MoveCancelled(InputAction.CallbackContext ctx)
    {
        movementInput[0] = ctx.ReadValue<float>();
        anim.SetBool("Move", false);
    }

    private void JumpCancelled(InputAction.CallbackContext ctx)
    {
        jumpInput = 0;
        jumpInputHold = false;
    }

    private void FixedUpdate()
    {
        // Ground Check
        touchingGround = GroundCheck.OnGround(transform.position, jumpChecks, width, distanceToJump);
        if (touchingGround) anim.SetBool("Jump", false);

        // Input Hold
        DoubleJumpHold();

        // Movement
        Move();
        Jump();
    }

    private void Move()
    {
        // Uses the previous input to check if both are empty. If so, don't apply a velocity.
        if (movementInput[1] == 0 && movementInput[0] == 0) return;
        movementInput[1] = movementInput[0];
        if (movementInput[0] == 0 && !touchingGround) return;
        float speed = stats.movementSpeed;
        if (!touchingGround && movementInput[0] != jumpDirection) speed *= stats.jumpMoveSpeedReductionModifier;

        rb.velocity = new Vector2(movementInput[0] * speed, rb.velocity.y);

       Flip();
    }

    private void Jump()
    {
        float jumpSpeed = 0;

        // Check to reset jump trackers once landing
        if (jumpPerformed || doubleJumpPerformed)
        {
            if (touchingGround)
            {
                jumpPerformed = false;
                doubleJumpPerformed = false;
            }
        }

        // Jump Checks
        if (jumpInput == 0) return;
        if (!touchingGround)
        {
            if (!stats.doubleJump) return;
            if (doubleJumpPerformed) return;
            jumpSpeed = stats.doubleJumpSpeed;
            doubleJumpPerformed = true;
            // x velocity of second jump should be in the direction being held
            rb.velocity = new Vector2(movementInput[0] * stats.movementSpeed, rb.velocity.y);
        }
        else
        {
            jumpSpeed = stats.jumpSpeed;
            jumpPerformed = true;
            jumpInputHold = true;
        }
        
        jumpDirection = movementInput[0];
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

        // Setup for jump hold
        jumpInput = 0;
        jumpInputHoldTime = inputHoldTime;

        anim.SetBool("Jump", true);
        Flip(true);
    }

    private void DoubleJumpHold()
    {
        if (!stats.doubleJump) return;
        if (!jumpPerformed || !jumpInputHold || doubleJumpPerformed) return;
        if (jumpInputHoldTime <= 0)
        {
            jumpInputHold = false;
            jumpInput = 1;
            return;
        }

        jumpInputHoldTime--;
    }

    private void Flip(bool ignoreGround = false) {
        if (!touchingGround && !ignoreGround) return;
        // Flip X
        bool flipChange = true;
 
        if (sprite.flipX && movementInput[0] == 1) sprite.flipX = false;
        else if (!sprite.flipX && movementInput[1] == -1) sprite.flipX = true;
        else flipChange = false;

        if (flipChange) {
            weaponHolderParent.localPosition = new Vector3(-weaponHolderParent.localPosition.x, weaponHolderParent.localPosition.y, weaponHolderParent.localPosition.z);
            float angle = 0;
            if (sprite.flipX) angle = 60;
            weaponHolderParent.localEulerAngles = new Vector3(weaponHolderParent.localEulerAngles.x, weaponHolderParent.localEulerAngles.y, angle);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showRays) return;
        Gizmos.color = Color.red;

        for (int i = 1; i <= jumpChecks; i++)
        {
            Gizmos.DrawLine(new Vector2((transform.position.x + width / jumpChecks * i) - width / 2 - width / jumpChecks / 2, transform.position.y), new Vector2((transform.position.x + width / jumpChecks * i) - width / 2 - width / jumpChecks / 2, transform.position.y - distanceToJump));
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
