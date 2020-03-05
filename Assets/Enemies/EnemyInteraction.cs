using UnityEngine;

public class EnemyInteraction
{
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
    private float moveTime;

    // Other Trackers
    private Vector2 direction;
    private bool movementLast;

    public EnemyInteraction(EnemyManager manager, EnemyStats stats, GameObject self) {
        this.stats = stats;
        this.self = self;
        this.manager = manager;
        rb = self.GetComponent<Rigidbody2D>();

        health = stats.health;
        stunTime = stats.stunTime;
        flickerRate = stats.flickerRate;
        moveTime = stats.maxTimeMoving;

        interactive = true;

        sprite = self.GetComponent<SpriteRenderer>();
    }

    public void Move(RaycastHit2D player) {
        float random = Random.Range(0.00f, 1.00f);
        Debug.Log(random);
        if (random < 0.5f) {
            PerformJump(GetPlayerDirection(player), player);
        } else if (random >= 0.5f && random <=1f) {
            PerformMovement(GetPlayerDirection(player));
        }

        if (!interactive) {
            if (random < 0.45f) {
                ContinueMovement();
            }
        }
    }

    public Vector2 GetPlayerDirection(RaycastHit2D player) {
        if (player == default) {
            int sign = Random.Range(0,1);
            if (sign == 0) sign = -1;
            return new Vector2(sign, 0);
        }

        Vector2 direction = player.point - new Vector2(self.transform.position.x, self.transform.position.y);
        direction.Normalize();
        return direction;
    }

    private void PerformMovement(Vector2 direction) {
        if (!interactive) return;
        this.direction = direction;
        movementLast = true;

        ContinueMovement();
    }

    private void ContinueMovement() {
        if (!movementLast) return;
        if (moveTime <= 0) return;
        rb.velocity = Vector2.right * direction.x * stats.movementSpeed;
    }

    private void PerformJump(Vector2 direction, RaycastHit2D player) {
        // Get Angle
        float xAngle = stats.horizontalVerticalJumpRatio;
        float yAngle = 1-stats.horizontalVerticalJumpRatio;

        // Time for jump
        float ySpeed = yAngle * stats.jumpSpeed;
        float time = ySpeed/Physics2D.gravity.y;

        // Horizotal Speed Needed
        float xSpeed = xAngle * stats.jumpSpeed;
        if (player != default) {
            if ((Mathf.Abs(self.transform.position.x - player.transform.position.x) - stats.jumpInRange) < xSpeed*time) {
                xSpeed = Mathf.Abs(self.transform.position.x - player.transform.position.x)/time;
                xSpeed += Random.Range(stats.jumpRandomRange.x,stats.jumpRandomRange.y);
                xSpeed = xSpeed > stats.jumpSpeed ? stats.jumpSpeed : xSpeed;
            }
        } else {
            xSpeed += Random.Range(stats.jumpRandomRange.x, stats.jumpRandomRange.y);
        }
        
        // Apply Movement
        rb.velocity = new Vector2(xSpeed*direction.x, ySpeed);

        movementLast = false;
    }

    public void Attack(RaycastHit2D player) {
        // This is just an animation
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
        MoveTimeUpdate();
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

    private void MoveTimeUpdate() {
        if (interactive) { 
            moveTime = stats.maxTimeMoving;
            return;
        }
        if (moveTime > 0) {
            moveTime -= Time.deltaTime;
        }
        if (Mathf.Abs(rb.velocity.x) < 0.75f * stats.movementSpeed) {
            moveTime = 0;
        }
    }


}
