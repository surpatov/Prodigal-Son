using UnityEngine;
using UnityEngine.AI;

public class MonsterLooking : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly float timeLooking;
    private float timeLeft;
    private Monster monster;
    private bool canSwitch;
    public MonsterLooking(Monster_FSM fsm, NavMeshAgent agent, Animator animator, float timeLooking, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.timeLooking = timeLooking;
        this.monster = monster;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: LOOKING");
        timeLeft = timeLooking;
        agent.isStopped = true;
        animator.SetTrigger("LOOK");
        canSwitch = true;
    }
    public override void Update()
    {
        monster.Seek(true);
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && canSwitch)
        {
            fsm.SetState<MonsterPatrol>();
            canSwitch = false;
        }
    }
    public override void Exit()
    {
        agent.isStopped = false;
        //agent.ResetPath();   // добавить
    }
}