using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeadState : ZombieStates
{
    private static readonly int MovementZHash = Animator.StringToHash("MovementZ");
    private static readonly int IsDeadHash = Animator.StringToHash("isDead");

    public ZombieDeadState(ZombieComponent zombie, ZombieStateMachine stateMachine) : base(zombie, stateMachine)
    {
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ownerZombie.zombieNavMeshAgent.isStopped = true;
        ownerZombie.zombieNavMeshAgent.ResetPath();
    
        ownerZombie.zombieAnimator.SetFloat(MovementZHash, 0);
        ownerZombie.zombieAnimator.SetBool(IsDeadHash, true);
    }

    public override void Exit()
    {
        base.Exit();
        ownerZombie.zombieNavMeshAgent.isStopped = false;
        ownerZombie.zombieAnimator.SetBool(IsDeadHash, false);
    }
}
