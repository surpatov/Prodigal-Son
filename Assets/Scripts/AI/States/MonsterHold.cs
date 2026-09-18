using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterHold : Monster_State
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private Monster monster;
    private Slider progressBar;
    public MonsterHold(Monster_FSM fsm, NavMeshAgent agent, Animator animator, Monster monster) : base(fsm)
    {
        this.agent = agent;
        this.animator = animator;
        this.monster = monster;
    }

    public override void Enter()
    {
        Debug.Log("Monster enter state: HOLD");
        animator.SetTrigger("HOLD");
        agent.isStopped = true;

        monster.jumpscareCamera.gameObject.SetActive(true);
        monster.player.GetComponent<PlayerController>().canInteract = false;
        monster.player.GetComponent<PlayerController>().canWalk = false;
        monster.player.GetComponent<PlayerController>().canRotation = false;
        progressBar = monster.player.GetComponent<PlayerController>().progressBarHold;
        monster.player.GetComponent<PlayerController>().gameplayMenu.gameObject.SetActive(false);
        monster.player.GetComponent<PlayerController>().holdMenu.gameObject.SetActive(true);
        monster.player.GetComponent<PlayerController>().CameraHead.SetActive(false);
        //monster.player.GetComponent<PlayerController>().canvas.gameObject.SetActive(false);

        progressBar.value = 0.1f;
    }

    public override void Update()
    {
        bool pressed = Input.GetKeyDown(KeyCode.F) || ControllerInput.BDown();

        progressBar.value += monster.progressBarSpeed * Time.deltaTime;
        if(pressed) progressBar.value -= monster.progressBarMinus;

        pressed = false;

        if(progressBar.value <= 0f)
        {
            monster.fsm.SetState<MonsterCoolDown>();
        }
        if(progressBar.value >= 1f)
        {
            monster.animator.SetTrigger("JUMPSCARE");
            monster.player.GetComponent<PlayerController>().canvas.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
        agent.isStopped = false;
        monster.player.GetComponent<PlayerController>().canInteract = true;
        monster.player.GetComponent<PlayerController>().canWalk = true;
        monster.player.GetComponent<PlayerController>().canRotation = true;
        monster.player.GetComponent<PlayerController>().gameplayMenu.gameObject.SetActive(true);
        monster.player.GetComponent<PlayerController>().holdMenu.gameObject.SetActive(false);
        monster.player.GetComponent<PlayerController>().CameraHead.SetActive(true);
        monster.player.GetComponent<PlayerController>().canvas.gameObject.SetActive(true);
        monster.jumpscareCamera.gameObject.SetActive(false);
        monster.progressBarSpeed *= 1.5f;
    }
}
