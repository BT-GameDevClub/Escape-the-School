using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyManager : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;

    [Header("Debug Options")]
    public bool showRays;

    private EnemyInteraction interaction;

    private static GameObject player;

    // Action Trackers
    private float timeBetweenActions;

    private void Awake() {
        interaction = new EnemyInteraction(this, stats, gameObject);
        if (player == null) UpdatePlayer();
    }

    private void Update() {
        InRange();

        ActionTimeout();
    }

    private void InRange() {
        if (timeBetweenActions > 0) return;
        if (!GroundCheck.OnGround(transform.position, 3, 3, stats.distanceToGround)) return;
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
        if (timeBetweenActions < 0) return;
        timeBetweenActions-=Time.deltaTime;
    }

    public void DealDamage(float damage) {
        interaction.DealDamage(damage);
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

    public void UpdatePlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Kill() {
        Destroy(gameObject);
    }
    

}