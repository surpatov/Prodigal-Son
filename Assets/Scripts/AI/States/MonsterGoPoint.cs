using UnityEngine;
using UnityEngine.AI;

public class MonsterGoPoint : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private Monster monster;
    private Transform pointTo;
    public MonsterGoPoint(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.monster = monster;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: GO TO POINT");
        animator.SetTrigger("WALK");
        pointTo = monster.point.transform;
        agent.SetDestination(pointTo.position);
    }
    public override void Update()
    {
        monster.Seek(false);
        if (monster.IsNearToPoint(agent.destination))
        {
            if (pointTo.GetComponent<Item>())
                fsm.SetState<MonsterPickUp>();
        }
    }
}
