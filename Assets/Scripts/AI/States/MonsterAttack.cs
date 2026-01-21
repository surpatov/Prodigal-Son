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
        Seek();
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
    private void Seek()
    {
        RaycastHit[] hits = Physics.BoxCastAll(monster.head.transform.position, new Vector3(15f, 15f, 15f), monster.gameObject.transform.forward, Quaternion.identity, 1f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Player")
            {
                float realAngle = Vector3.Angle(monster.head.transform.forward, hit.transform.position - monster.head.transform.position);
                if (realAngle <= monster.GetView / 2f)
                {
                    RaycastHit raycastHit;
                    if (Physics.Linecast(monster.gameObject.transform.position, hit.transform.position, out raycastHit))
                    {
                        if (raycastHit.collider.tag == "Player")
                        {
                            //Debug.Log("DETECT");
                            monster.player = raycastHit.collider.gameObject;
                            fsm.SetState<MonsterAttack>();
                            monster.patience = monster.patienceMax;
                            break;
                        }
                    }
                }
            }
        }
    }
}
