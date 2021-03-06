using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName="ScriptableObjects/EnemyStats", order = 2)]
public class EnemyStats : ScriptableObject {
    [Header("Vision Stats")]
    public float range;
    public float attackRange;

    [Header("Raycast Stats")]
    public float width;
    public int jumpChecks;
    public float distanceToGround;

    [Header("Movement Stats")]
    public float movementSpeed;
    public float jumpSpeed;
    [Range(0, 1f)]
    public float moveJumpRatio;
    [Range(0,1f)]
    public float inRangeWaitChance;
    [Range(0,1f)]
    public float outRangeWaitChance;
    [Range(0, 1f)]
    public float horizontalVerticalJumpRatio;
    public Vector2 jumpRandomRange;
    public float jumpInRange;

    [Header("Attack Stats")]
    public float health;
    public float height;

    [Header("Stun Stats")]
    public float stunTime;
    public float stunKnockback;
    public float flickerRate;

    [Header("Additional Stats")]
    public float timeBetweenActions;
    public float additionalJumpTime;
    public float minTimeMoving;
    public float maxTimeMoving;
    
}