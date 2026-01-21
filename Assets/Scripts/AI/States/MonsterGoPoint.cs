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
        Seek();
        if (Vector3.Distance(agent.transform.position, agent.destination) < 0.5f)
        {
            if (pointTo.GetComponent<Item>())
                fsm.SetState<MonsterPickUp>();
        }
    }
    private void Seek()
    {
        RaycastHit[] hits = Physics.BoxCastAll(monster.head.transform.position, new Vector3(15f, 15f, 15f), monster.gameObject.transform.forward, Quaternion.identity, 1f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<PlayerController>() || hit.collider.GetComponent<Item>())
            {
                float realAngle = Vector3.Angle(monster.head.transform.forward, hit.transform.position - monster.head.transform.position);
                if (realAngle <= monster.GetView / 2f)
                {
                    RaycastHit raycastHit;
                    if (Physics.Linecast(monster.gameObject.transform.position, hit.transform.position, out raycastHit))
                    {
                        if (raycastHit.collider.GetComponent<PlayerController>())
                        {
                            monster.player = hit.collider.gameObject;
                            fsm.SetState<MonsterAttack>();
                            monster.patience = monster.patienceMax;
                        }
                    }
                }
            }
        }
    }
}
