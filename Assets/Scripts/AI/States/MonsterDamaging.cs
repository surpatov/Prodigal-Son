using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDamaging : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private Monster monster;
    private Transform damagingPoint;
    public MonsterDamaging(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Monster monster, Transform damagingPoint) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.monster = monster;
        this.damagingPoint = damagingPoint;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: DAMAGING");
        agent.SetDestination(damagingPoint.position);
        animator.SetTrigger("RUN");
        agent.speed = monster.speedRun;
    }
    public override void Update()
    {
        // кричит от боли
        if (agent.transform.position == agent.destination)
        {
            fsm.SetState<MonsterLooking>();
        }
    }
    public override void Exit()
    {
        agent.speed = monster.speedWalk;
    }
}