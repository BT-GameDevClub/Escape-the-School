using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName="ScriptableObjects/EnemyStats", order = 2)]
public class EnemyStats : ScriptableObject {

    [Header("Movement Stats")]
    public float movementSpeed;
    public float jumpSpeed;

    [Header("Attack Stats")]
    public float health;

    [Header("Stun Stats")]
    public float stunTime;
    public float stunKnockback;
    public float flickerRate;
    
}