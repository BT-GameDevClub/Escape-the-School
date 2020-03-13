
using UnityEngine;

public class EnemyStateManager
{
    private EnemyState state;

    // Component References
    private GameObject self;
    private EnemyManager manager;
    private Animator anim;

    public EnemyStateManager(EnemyManager manager, GameObject self) {
        this.self = self;
        this.manager = manager;
        anim = self.GetComponent<Animator>();
    }

    public void SetState(EnemyState state) {
        this.state = state;
        switch (state) {
            case EnemyState.IDLE:
                anim.SetBool("Idle", true);
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.JUMPSTART:
                anim.SetBool("Idle", false);
                anim.SetBool("Jump", true);
                break;
            case EnemyState.JUMP:
                break;
            case EnemyState.JUMPEND:
                anim.SetBool("Jump", false);
                break;
            case EnemyState.KILL:
                break;
            case EnemyState.STUN:
                break;
            case EnemyState.WALK:
                break;
        }
    }

    public EnemyState GetState() {
        return state;
    }
}
