using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyManager : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;
    
    [Header("Debug Options")]
    public bool showRays;

    private EnemyInteraction interaction;
    private EnemyStateManager stateManager;

    private static GameObject player;

    // Action Trackers
    private float timeBetweenActions;

    private void Awake() {
        stateManager = new EnemyStateManager(this, gameObject);
        interaction = new EnemyInteraction(this, stats, gameObject, stateManager);
        if (player == null) UpdatePlayer();
    }

    private void Update() {
        interaction.Update();
        InRange();

        ActionTimeout();
    }

    private void InRange() {
        if (!GroundCheck.OnGround(transform.position, 3, 3, stats.distanceToGround)) return;
        if (stateManager.GetState() == EnemyState.JUMP) {
            stateManager.SetState(EnemyState.JUMPEND);
            AddJumpTime();
            return;
        }

        stateManager.SetState(EnemyState.IDLE);
        if (timeBetweenActions > 0) return;
        RaycastHit2D rayHit = Physics2D.BoxCast(transform.position, new Vector2(stats.range, stats.height), 0, new Vector2(0, 0), 1, 1 << LayerMask.NameToLayer("Player"));
        if (rayHit) {
            if (Physics2D.BoxCast(transform.position, new Vector2(stats.attackRange, stats.height), 0, new Vector2(0,0), 1, 1 << LayerMask.NameToLayer("Player"))) {
                // In Attack Range
                interaction.Attack(rayHit);
            }
            else {               
                // In Movement Range
                interaction.Move(rayHit);
            }
        }
        else {
            interaction.Move(default);
        }

        timeBetweenActions = stats.timeBetweenActions;
    }

    private void ActionTimeout() {
        if (timeBetweenActions < 0 || stateManager.GetState() == EnemyState.JUMPSTART) return;
        timeBetweenActions-=Time.deltaTime;
    }

    public void DealDamage(Transform player, float damage) {
        interaction.DealDamage(player, damage);
    }

    private void OnDrawGizmos() {
        if (!showRays) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(stats.range, stats.height));
        Gizmos.color = Color.red;

        for (int i = 1; i <= stats.jumpChecks; i++)
        {
            Gizmos.DrawLine(new Vector2((transform.position.x + stats.width / stats.jumpChecks * i) - stats.width / 2 - stats.width / stats.jumpChecks / 2, transform.position.y), new Vector2((transform.position.x + stats.width / stats.jumpChecks * i) - stats.width / 2 - stats.width / stats.jumpChecks / 2, transform.position.y - stats.distanceToGround));
        }
    }

    public void ApplyJump() {
        interaction.ApplyJump();
    }

    public void AddJumpTime() {
        timeBetweenActions += stats.additionalJumpTime;
    }

    public void UpdatePlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public void Kill() {
        Destroy(gameObject);
    }
}