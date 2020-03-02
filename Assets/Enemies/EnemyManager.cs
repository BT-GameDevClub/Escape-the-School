using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;

    private EnemyInteraction interaction;

    private static GameObject player;

    private void Awake() {
        interaction = new EnemyInteraction(this, stats, gameObject);
        if (player == null) UpdatePlayer();
    }

    private void FixedUpdate() {
        InRange();
    }

    private void InRange() {
        if(Physics2D.BoxCast(transform.position, new Vector2(stats.range, stats.height), 0, new Vector2(0,0), 1, 1 << LayerMask.NameToLayer("Player"))) {
            if (Physics2D.BoxCast(transform.position, new Vector2(stats.attackRange, stats.height), 0, new Vector2(0,0), 1, 1 << LayerMask.NameToLayer("Player"))) {
                // In Attack Range
                interaction.Attack();
            }
            else {               
                // In Movement Range
                interaction.Move();
            }
        }
        else {
            interaction.RandomMove();
        }

        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(stats.range, stats.height));
    }

    public void UpdatePlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Kill() {
        Destroy(gameObject);
    }
    

}