using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;

    // Component References
    private SpriteRenderer sprite;

    // Attack Trackers
    private float health;

    // Interaction Trackers
    private bool interactive;
    private float stunTime;
    private float flickerRate;

    private void Awake() {
        health = stats.health;
        stunTime = stats.stunTime;
        flickerRate = stats.flickerRate;

        interactive = true;

        sprite = GetComponent<SpriteRenderer>();
    }
    public void Move(Vector2 direction) {
        
    }

    public void Jump() {

    }

    public void Attack() {

    }

    public void DealDamage(float damage) {
        if (!interactive) return; 
        interactive = false;
        
        health -= damage;
        if (health <= 0) {
            // Play Animation, call KillEnemy() as a keyed event
            KillEnemy();
        }
    }

    private void Update() {
        FlickerUpdate();
        StunUpdate();
    }

    private void FlickerUpdate() {
        if (interactive) return;
        if (flickerRate <= 0) {
            sprite.enabled = !sprite.enabled;
            flickerRate = stats.flickerRate;
            return;
        }

        flickerRate-= Time.deltaTime;
    }

    private void StunUpdate() {
        if (interactive) return;
        if (stunTime <= 0) 
        {
            interactive = true;
            stunTime = stats.stunTime;
            flickerRate = stats.flickerRate;
            sprite.enabled = true;
            return;
        }

        stunTime-= Time.deltaTime;
    }

    private void KillEnemy() {
        Destroy(gameObject);
    }

}
