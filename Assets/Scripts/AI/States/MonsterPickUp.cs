using UnityEngine;
using UnityEngine.AI;

public class MonsterPickUp : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly float timePicking;
    private float timeLeft;
    private Monster monster;
    public MonsterPickUp(Monster_FSM fsm, NavMeshAgent agent, Animator animator, float timePicking, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.timePicking = timePicking;
        this.monster = monster;
    }
    public override void Enter()
    {
        monster.student.IQUp = 30;
        Debug.Log("Monster enter state: PICKING UP");
        agent.isStopped = true;
        timeLeft = timePicking;
        animator.SetTrigger("PICK");
    }
    public override void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            fsm.SetState<MonsterPatrol>();
        }
    }
    public override void Exit()
    {
        agent.isStopped = false;
    }
}
