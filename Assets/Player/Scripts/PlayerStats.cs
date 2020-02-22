using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject {

    [Header("Basic Stats")]
    public float movementSpeed;
    public float jumpSpeed;

    [Header("Modifiers")]
    public float jumpMoveSpeedReductionModifier;

    [Header("Upgrades")]
    public bool doubleJump;

    [Header("Upgrade Stats")]
    public float doubleJumpSpeed;

}