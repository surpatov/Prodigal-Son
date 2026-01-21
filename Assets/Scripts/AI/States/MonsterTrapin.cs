using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterTrapin : Monster_State
{
    private NavMeshAgent agent;
    private Monster monster;
    private GameObject trap;
    private Transform rayPoint;
    private LayerMask layer;
    private RaycastHit hit;
    public MonsterTrapin(Monster_FSM fsm, NavMeshAgent agent, Monster monster, GameObject trap, LayerMask layer) : base(fsm)
    {
        this.agent = agent;
        this.monster = monster;
        this.trap = trap;
        this.layer = layer;
    }
    public override void Enter()
    {
        rayPoint = monster.transform;
        //agent.isStopped = true;
    }
    public override void Update()
    {
        Debug.Log("MONSTER TRAPPIN");
        monster.SetTrap(monster.transform, Quaternion.identity, trap);
        monster.SetTimer();
        fsm.SetState<MonsterPatrol>();
    }
    public override void Exit()
    {
        //agent.isStopped = false;
    }
}