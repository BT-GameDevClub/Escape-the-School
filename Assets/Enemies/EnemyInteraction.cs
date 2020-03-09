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
    private bool continueMoving;
    private bool jumping;

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
        continueMoving = false;

        sprite = self.GetComponent<SpriteRenderer>();
    }

    public void Move(RaycastHit2D player) {
        float random = Random.Range(0.00f, 1.00f);
        float waitChance = stats.inRangeWaitChance;
        if (player == default) {
            waitChance = stats.outRangeWaitChance;
        }
        if (random < stats.moveJumpRatio * (1 - waitChance)) {
            PerformJump(GetPlayerDirection(player), player);
        } else if (random >= stats.moveJumpRatio * (1- waitChance) && random <= (1 - waitChance)) {
            PerformMovement(GetPlayerDirection(player));
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
        continueMoving = true;
        moveTime = Random.Range(stats.minTimeMoving, stats.maxTimeMoving);

        ContinueMovement();
    }

    private void ContinueMovement() {
        Debug.Log("<b>Continue Moving:</b> " + continueMoving);
        Debug.Log("<b>Move Time:</b> " + moveTime);
        if (!continueMoving) return;
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
                xSpeed += Random.Range(-stats.jumpRandomRange.x,stats.jumpRandomRange.y);
                xSpeed = xSpeed > stats.jumpSpeed ? stats.jumpSpeed : xSpeed;
            }
        } else {
            xSpeed += Random.Range(stats.jumpRandomRange.x, stats.jumpRandomRange.y);
        }
        
        // Apply Movement
        rb.velocity = new Vector2(xSpeed*direction.x, ySpeed);

        jumping = true;
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

    public void Update() {
        ContinueMovement();

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
        if (continueMoving && moveTime > 0) {
            moveTime -= Time.deltaTime;
        }
        if (moveTime < 0) {
            moveTime = 0;
            continueMoving = false;
        }
    }

    public bool GetJumping() {
        return jumping;
    }

    public void SetJumping(bool jumping) {
        this.jumping = jumping;
    }


}
