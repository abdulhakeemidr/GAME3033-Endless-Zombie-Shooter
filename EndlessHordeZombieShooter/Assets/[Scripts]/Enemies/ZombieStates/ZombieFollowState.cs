using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowState : ZombieStates
{
    GameObject FollowTarget;
    const float StopDistance = 1.5f;
    int MovementZHash = Animator.StringToHash("MovementZ");
    
    public ZombieFollowState(GameObject followTarget, ZombieComponent zombie, ZombieStateMachine stateMachine) : base(zombie, stateMachine)
    {
        FollowTarget = followTarget;
        UpdateInterval = 2.0f;
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ownerZombie.zombieNavMeshAgent.SetDestination(FollowTarget.transform.position);
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        ownerZombie.zombieNavMeshAgent.SetDestination(FollowTarget.transform.position);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float moveZ = ownerZombie.zombieNavMeshAgent.velocity.normalized.z != 0f ? 1f : 0f;
        ownerZombie.zombieAnimator.SetFloat(MovementZHash, moveZ);
        //ownerZombie.zombieAnimator.SetFloat(MovementZHash, ownerZombie.zombieNavMeshAgent.velocity.normalized.z);

        float distanceBetween = Vector3.Distance(ownerZombie.transform.position, FollowTarget.transform.position);
        if(distanceBetween < StopDistance)
        {
            stateMachine.ChangeState(ZombieStateType.Attacking);
        }
               
       //TODO: Zombie Health < 0 Die.
        
    }
}
