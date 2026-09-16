using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

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

        if(Vector3.Distance(monster.transform.position, player.transform.position) <= monster.distanceToJumpscare && monster.patience > 0)
        {
            monster.jumpscareCamera.gameObject.SetActive(true);
            monster.animator.SetTrigger("JUMPSCARE");
            player.GetComponent<PlayerController>().canInteract = false;
            player.GetComponent<PlayerController>().canRotation = false;
            player.GetComponent<PlayerController>().canWalk = false;
            player.GetComponent<PlayerController>().CameraHead.SetActive(false);
            player.GetComponent<PlayerController>().canvas.gameObject.SetActive(false);
            //monster.jumpscareCamera.transform.position = player.GetComponent<PlayerController>().cameraObject.transform.position;
            //monster.jumpscareCamera.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 0.4f).SetEase(Ease.InOutSine);
        }
    }
    public override void Exit()
    {
        agent.speed = monster.speedWalk;
    }
}
