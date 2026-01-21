using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDropItem : Monster_State
{
    private readonly NavMeshAgent agent;
    private Monster monster;
    public MonsterDropItem(Monster_FSM fsm, NavMeshAgent agent, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.monster = monster;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: DROP ITEM");
        agent.isStopped = true;
        DropItem();
    }
    public override void Exit()
    {
        agent.isStopped = false;
    }
    public void DropItem()
    {
        var item = monster.item;
        item.transform.parent = null;
        item.GetComponent<Collider>().enabled = true;
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().freezeRotation = false;
        agent.isStopped = false;
        monster.item = null;
        fsm.SetState<MonsterLooking>();
    }
}