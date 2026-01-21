using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly Transform[] points;
    private Monster monster;
    public MonsterPatrol(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Transform[] points, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.points = points;
        this.monster = monster;
    }
    public override void Enter()
    {
        SetPoint();
        Debug.Log("Monster enter state: PATROL");
        animator.SetTrigger("WALK");
    }
    public override void Update()
    {
        Seek();

        /*if (monster.item != null && agent.transform.position == agent.destination)
        {
            fsm.SetState<MonsterDropItem>();
        }
        else*/

        if (agent.transform.position == agent.destination)
        {
            fsm.SetState<MonsterLooking>();
        }
    }
    private void SetPoint()
    {
        int rand = Random.Range(0, points.Length);
        agent.SetDestination(points[rand].position);
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
            else if (hit.collider.GetComponent<Item>())
            {
                float realAngle = Vector3.Angle(monster.head.transform.forward, hit.transform.position - monster.head.transform.position);
                if (realAngle <= monster.GetView / 2f)
                {
                    RaycastHit raycastHit;
                    if (Physics.Linecast(monster.gameObject.transform.position, hit.transform.position, out raycastHit))
                    {
                        if (raycastHit.collider.GetComponent<Item>() && fsm.currentState.GetType() != typeof(MonsterAttack) && fsm.currentState.GetType() != typeof(MonsterPickUp) && monster.item == null)
                        {
                            monster.point = raycastHit.collider.gameObject;
                            fsm?.SetState<MonsterGoPoint>();
                            break;
                        }
                    }
                }
            }
        }
    }
}