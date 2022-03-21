using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : ZombieStates
{
    int MovementZHash = Animator.StringToHash("MovementZ");

    public ZombieIdleState(ZombieComponent zombie, ZombieStateMachine stateMachine) : base(zombie, stateMachine)
    {
    }

    public override void Start()
    {
        base.Start();
        ownerZombie.zombieNavMeshAgent.isStopped = true;
        ownerZombie.zombieNavMeshAgent.ResetPath();
        ownerZombie.zombieAnimator.SetFloat(MovementZHash, 0.0f);
    }

    public override void Exit()
    {
        base.Exit();
        ownerZombie.zombieNavMeshAgent.isStopped = false;
    }
}
