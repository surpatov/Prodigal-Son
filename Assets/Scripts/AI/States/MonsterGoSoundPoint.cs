using UnityEngine;
using UnityEngine.AI;

public class MonsterGoSoundPoint : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private Monster monster;
    private Transform pointTo;
    public MonsterGoSoundPoint(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.monster = monster;
    }
    public override void Enter()
    {
        Debug.Log("Monster enter state: GO TO SOUND POINT");
        animator.SetTrigger("WALK");
        pointTo = monster.point.transform;
        agent.SetDestination(pointTo.position);
    }
    public override void Update()
    {
        Seek();
        if (Vector3.Distance(agent.transform.position, agent.destination) < 0.5f)
        {
            fsm.SetState<MonsterLooking>();
        }
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
