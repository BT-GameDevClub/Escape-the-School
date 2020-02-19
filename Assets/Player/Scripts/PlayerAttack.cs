using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // Component References
    private Animator anim;

    // Input Trackers
    private PlayerInputActions input;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        input = new PlayerInputActions();
        input.Game.Attack.performed += Attack;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        //anim.SetBool("Move")
    }
}
