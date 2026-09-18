using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCoolDown : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly float timeToStay;
    private float timeLeft;
    private Monster monster;
    private bool canSwitch;
    public MonsterCoolDown(Monster_FSM fsm, NavMeshAgent agent, Animator animator, float timeStaying, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.timeToStay = timeStaying;
        this.monster = monster;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: COOLDOWN");
        timeLeft = timeToStay;
        agent.isStopped = true;
        animator.SetTrigger("STAY");
        canSwitch = true;
    }
    public override void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && canSwitch)
        {
            fsm.SetState<MonsterLooking>();
            canSwitch = false;
        }
    }
    public override void Exit()
    {
        agent.isStopped = false;
        //agent.ResetPath();   // добавить
    }
}
