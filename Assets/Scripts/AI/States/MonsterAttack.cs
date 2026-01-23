using UnityEngine;
using UnityEngine.AI;

public class MonsterAttack : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private GameObject player;
    private Monster monster;
    public MonsterAttack(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.monster = monster;
    }
    public override void Enter()
    {
        monster.student.IQUp = 30;
        Debug.Log("Monster enter state: ATTACK");
        animator.SetTrigger("RUN");
        player = monster.GetPlayer;
        agent.speed = monster.speedRun;
    }
    public override void Update()
    {
        monster.Seek(false);
        agent.SetDestination(player.transform.position);
        monster.patience -= Time.deltaTime;
        if (monster.patience < 0 || player.activeInHierarchy == false)
        {
            fsm.SetState<MonsterLooking>();
        }
    }
    public override void Exit()
    {
        agent.speed = monster.speedWalk;
    }
}
