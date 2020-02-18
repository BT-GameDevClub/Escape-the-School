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
    public bool showRays;
    public float inputHoldTime;

    // Component References
    private Rigidbody2D rb;

    // Input Trackers
    private PlayerInputActions input;
    private float[] movementInput = new float[2];
    private float jumpInput;
    private bool jumpInputHold;
    private float jumpInputHoldTime;

    // Movement Trackers
    private bool jumpPerformed;
    private bool doubleJumpPerformed;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Setup for the new Input System
        input = new PlayerInputActions();
        input.Game.Move.performed += ctx => movementInput[0] = ctx.ReadValue<float>();
        input.Game.Move.canceled += ctx => movementInput[0] = ctx.ReadValue<float>();
        input.Game.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();
        input.Game.Jump.canceled += JumpCancelled;
    }

    private void JumpCancelled(InputAction.CallbackContext ctx)
    {
        jumpInput = 0;
        jumpInputHold = false;
    }

    private void FixedUpdate()
    {
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
        rb.velocity = new Vector2(movementInput[0] * stats.movementSpeed, rb.velocity.y);
        movementInput[1] = movementInput[0];
    }

    private void Jump()
    {
        float jumpSpeed = 0;

        // Check to reset jump trackers once landing
        if (jumpPerformed || doubleJumpPerformed)
        {
            if (onGround())
            {
                jumpPerformed = false;
                doubleJumpPerformed = false;
            }
        }

        // Jump Checks
        if (jumpInput == 0) return;
        if (!onGround())
        {
            if (!stats.doubleJump) return;
            if (doubleJumpPerformed) return;
            jumpSpeed = stats.doubleJumpSpeed;
            doubleJumpPerformed = true;
        }
        else
        {
            jumpSpeed = stats.jumpSpeed;
            jumpPerformed = true;
            jumpInputHold = true;
        }

        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

        // Setup for jump hold
        jumpInput = 0;
        jumpInputHoldTime = inputHoldTime;
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

    private bool onGround()
    {
        // Checks if the player is on the ground. Allows for multiple rays since the player might be slightly off the edge.
        for (int i = 0; i < jumpChecks; i++)
        {
            if (Physics2D.Raycast(new Vector2((transform.position.x + transform.localScale.x / jumpChecks * i) - transform.localScale.x / 2 - transform.localScale.x / jumpChecks / 2, transform.position.y), Vector2.down, distanceToJump, 1 << LayerMask.NameToLayer("Interactable"))) return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!showRays) return;
        Gizmos.color = Color.red;

        for (int i = 1; i <= jumpChecks; i++)
        {
            Gizmos.DrawLine(new Vector2((transform.position.x + transform.localScale.x / jumpChecks * i) - transform.localScale.x / 2 - transform.localScale.x / jumpChecks / 2, transform.position.y), new Vector2((transform.position.x + transform.localScale.x / jumpChecks * i) - transform.localScale.x / 2 - transform.localScale.x / jumpChecks / 2, transform.position.y - distanceToJump));
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
