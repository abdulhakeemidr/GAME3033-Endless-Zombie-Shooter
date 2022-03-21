using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : ZombieStates
{
    GameObject followTarget;
    float AttackRange = 2f;
    
    // Interface for damagable object here
    int MovementZHash = Animator.StringToHash("MovementZ");
    int IsAttackingHash = Animator.StringToHash("isAttacking");

    public ZombieAttackState(GameObject _followTarget, ZombieComponent zombie, ZombieStateMachine stateMachine) : base(zombie, stateMachine)
    {
        followTarget = _followTarget;
        UpdateInterval = 2.0f;
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        //base.Start();
        ownerZombie.zombieNavMeshAgent.isStopped = true;
        ownerZombie.zombieNavMeshAgent.ResetPath();
        ownerZombie.zombieAnimator.SetFloat(MovementZHash, 0.0f);
        ownerZombie.zombieAnimator.SetBool(IsAttackingHash , true);
        ownerZombie.zombieAnimator.SetBool(IsAttackingHash, true);
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        //TODO: Add Damage to object.
    }

    // Update is called once per frame
    public override void Update()
    {
        // base.Update();
        ownerZombie.transform.LookAt(followTarget.transform.position, Vector3.up);

        float distanceBetween = Vector3.Distance(ownerZombie.transform.position, followTarget.transform.position);
        if (distanceBetween > AttackRange)
        {
            stateMachine.ChangeState(ZombieStateType.Following);
        }
        
        //TODO: Zombie Health < 0 Die.
    }

    public override void Exit()
    {
        base.Exit();
        ownerZombie.zombieNavMeshAgent.isStopped = false;
        ownerZombie.zombieAnimator.SetBool(IsAttackingHash , false);
    }
}
