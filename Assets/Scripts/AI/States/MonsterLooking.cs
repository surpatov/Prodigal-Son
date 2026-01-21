using UnityEngine;
using UnityEngine.AI;

public class MonsterLooking : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly float timeLooking;
    private float timeLeft;
    private Monster monster;
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
    }
    public override void Update()
    {
        Seek();
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
                            Debug.Log("DETECT");
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