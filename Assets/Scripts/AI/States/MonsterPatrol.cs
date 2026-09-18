using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly Transform[] points;
    private Monster monster;
    private int temp;
    private bool movingStarted;
    public MonsterPatrol(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Transform[] points, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.points = points;
        this.monster = monster;
    }
    public override void Enter()
    {
        movingStarted = false;
        //agent.isStopped = false;
        SetPoint();
        Debug.Log("Monster enter state: PATROL");
        animator.SetTrigger("WALK");
    }

    public override void Update()
    {
        // ∆дЄм, пока агент реально начнЄт двигатьс€ и путь построитс€
        if (!movingStarted)
        {
            if (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                movingStarted = true;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (monster.item != null)
                fsm.SetState<MonsterDropItem>();
            else
                fsm.SetState<MonsterLooking>();
        }

        monster.Seek(true);
    }
    private void SetPoint()
    {
        int rand = Random.Range(0, points.Length);
        if (rand == temp) SetPoint();
        else
        {
            agent.SetDestination(points[rand].position);
            temp = rand;
        }
    }
}