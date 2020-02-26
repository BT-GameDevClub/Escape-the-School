using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    [Header("Player Parts")]
    public Transform weaponHolder;
    public Transform weapon;

    // Component References
    private Animator anim;
    private SpriteRenderer sprite;

    // Input Trackers
    private PlayerInputActions input;
    private float attackCooldownTracker;

    // Attack Trackers
    private bool attackHold;

    private void Awake()
    {
        // Component References
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

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

    private void AttackDamage() {
        int direction = 1;
        if (sprite.flipX) direction = -1;
        RaycastHit2D[] rays = Physics2D.RaycastAll(weaponHolder.transform.position, new Vector2(Mathf.Cos(Mathf.Deg2Rad * weaponHolder.eulerAngles.z), Mathf.Sin(Mathf.Deg2Rad*weaponHolder.eulerAngles.z)), stats.weaponRadius, (1 << LayerMask.NameToLayer("Interactable") | 1 << LayerMask.NameToLayer("Enemy")));
        foreach(RaycastHit2D ray in rays) {
            ray.transform.GetComponent<EnemyInteraction>().DealDamage(stats.weaponAttack);
        }

    }
}