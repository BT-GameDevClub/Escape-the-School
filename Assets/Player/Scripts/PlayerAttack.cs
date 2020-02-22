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
    private bool canAttack;
    private bool attackHold;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        input = new PlayerInputActions();
        input.Game.Attack.performed += AttackInput;
        input.Game.Attack.canceled += ctx => attackHold = false;

        canAttack = true;
        attackCooldownTracker = stats.attackCooldown;
    }

    private void AttackInput(InputAction.CallbackContext ctx)
    {
        attackHold = true;
        Attack();
    }

    private void Attack() {
        if (!canAttack) return;
        if (anim.GetBool("Attack")) return;
        //anim.SetBool("Attack");
        canAttack = false;
    }

    private void FixedUpdate() {
        AttackCooldown();

        AttackHold();
    }

    private void AttackCooldown() {
        if (canAttack) return;
        if (attackCooldownTracker <= 0) {
            attackCooldownTracker = stats.attackCooldown;
            canAttack = true;
            return;
        }
        attackCooldownTracker--;
    }

    private void AttackHold() {
        if (!canAttack) return;
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