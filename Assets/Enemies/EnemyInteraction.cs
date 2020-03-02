using UnityEngine;

public class EnemyInteraction
{
    [Header("Stats")]
    public EnemyStats stats;

    // Component References
    private GameObject self;
    private EnemyManager manager;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

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
        rb = self.GetComponent<Rigidbody2D>();

        health = stats.health;
        stunTime = stats.stunTime;
        flickerRate = stats.flickerRate;

        interactive = true;

        sprite = self.GetComponent<SpriteRenderer>();
    }
    public void Move(RaycastHit2D player) {
        float random = Random.Range(0, 1);
        if (random < stats.moveJumpRatio) {
            PerformJump(GetPlayerDirection(player));

        }
    }

    public Vector2 GetPlayerDirection(RaycastHit2D player) {
        if (player == default) return new Vector2(Mathf.RoundToInt(Random.Range(0, 1)), 0);
        Vector2 direction = player.point - new Vector2(self.transform.position.x, self.transform.position.y);
        direction.Normalize();
        return direction;
    }

    private void PerformMovement(Vector2 direction) {

    }

    private void PerformJump(Vector2 direction) {
        rb.velocity = new Vector2(direction.x * stats.horizontalVerticalJumpRatio * stats.jumpSpeed, (1 - stats.horizontalVerticalJumpRatio) * stats.jumpSpeed);
    }

    public void Attack(RaycastHit2D player) {

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
