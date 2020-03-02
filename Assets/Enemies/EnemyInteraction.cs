using UnityEngine;

public class EnemyInteraction
{
    [Header("Stats")]
    public EnemyStats stats;

    // Component References
    private GameObject self;
    private EnemyManager manager;
    private SpriteRenderer sprite;

    // Attack Trackers
    private float health;

    // Interaction Trackers
    private bool interactive;
    private float stunTime;
    private float flickerRate;

    public EnemyInteraction(EnemyManager manager, EnemyStats stats, GameObject self) {
        this.stats = stats;
        this.self = self;
        this.manager = manager;

        health = stats.health;
        stunTime = stats.stunTime;
        flickerRate = stats.flickerRate;

        interactive = true;

        sprite = self.GetComponent<SpriteRenderer>();
    }
    public void Move() {
        float random = Random.Range("")
    }

    public void RandomMove() {
        
    }

    private void PerformMovement(Vector2 direction) {

    }

    private void PerformJump(Vector2 direction) {

    }

    public void Attack() {

    }

    public void DealDamage(float damage) {
        if (!interactive) return; 
        interactive = false;
        
        health -= damage;
        if (health <= 0) {
            // Play Animation, call KillEnemy() as a keyed event
            manager.Kill();
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


}
