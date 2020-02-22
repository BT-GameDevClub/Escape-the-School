using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    // Component References
    private Animator anim;

    // Input Trackers
    private PlayerInputActions input;
    private float attackCooldownTracker;

    // Attack Trackers
    private bool attackHold;

    private void Awake()
    {
        // Component References
        anim = GetComponent<Animator>();

        // Setup Input
        input = new PlayerInputActions();
        input.Game.Attack.performed += AttackInput;
        input.Game.Attack.canceled += ctx => attackHold = false;

        // Default values
        attackCooldownTracker = stats.attackCooldown;
    }

    private void AttackInput(InputAction.CallbackContext ctx)
    {
        attackHold = true; // Used so the button can be held down instead of repeatedly pressed
        Attack();
    }

    // Damage and effects are dealt with by the animation via keys
    private void Attack() {
        // Done indivdually for no particular reason
        if (!stats.hasWeapon) return; // Player must have a weapon to attack
        if (anim.GetBool("Attack")) return;
        anim.SetBool("Attack", true); // Animation will set parameter to false
    }

    private void FixedUpdate() {
        AttackHold();
    }

    private void AttackHold() {
        if (!attackHold) return;

        Attack();
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