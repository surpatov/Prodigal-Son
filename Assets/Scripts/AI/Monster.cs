using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
    [SerializeField] public AILearning student;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    public GameObject GetPlayer => player;
    [SerializeField] public Animator animator;
    public Monster_FSM fsm;
    [SerializeField] private Transform[] points;
    public void SetPoints(Transform[] _points) => points = _points;
    [SerializeField] public GameObject head;
    [SerializeField] public GameObject hand;
    [SerializeField] public GameObject item;
    [SerializeField] private float timeLooking;
    [SerializeField] public float timePicking;
    [SerializeField] [Range(0, 360)] private float viewAngle;
    public float GetView => viewAngle;
    [SerializeField] public float speedRun;
    [SerializeField] public float speedWalk;

    [SerializeField] public float patience;
    [SerializeField] public float patienceMax;

    [SerializeField] public GameObject point;

    [SerializeField] private List<Monster_trap> traps = new List<Monster_trap>();
    [SerializeField] private int trapsLimit;
    [SerializeField] public GameObject trap;
    [SerializeField] public LayerMask layer;
    [SerializeField] private float timeLeftTrap;
    [SerializeField] private float MAXTIMETRAP;
    public bool canTrappin;

    [SerializeField] private Transform pointDamaging;

    [SerializeField] private float distanceToPoint;

    [SerializeField] private AudioClip[] footStepClips;
    [SerializeField] private AudioClip[] roarClips;
    private int tempIndexFoot;
    private int tempIndexRoar;

    [SerializeField] private AudioSource screamSource;
    [SerializeField] private AudioSource footSource;

    [SerializeField] public Camera jumpscareCamera;
    public float distanceToJumpscare = 0.3f;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        patience = patienceMax;
        canTrappin = false;

        fsm = new Monster_FSM();
        fsm.AddState(new MonsterPatrol(fsm, agent, animator, points, gameObject.GetComponent<Monster>()));
        fsm.AddState(new MonsterLooking(fsm, agent, animator, timeLooking, gameObject.GetComponent<Monster>()));
        fsm.AddState(new MonsterAttack(fsm, agent, animator, gameObject.GetComponent<Monster>()));
        fsm.AddState(new MonsterDamaging(fsm, agent, animator, gameObject.GetComponent<Monster>(), pointDamaging));

        fsm.SetState<MonsterAttack>();
    }
    private void Update()
    {
        fsm.Update();
        DrawView();
        if (canTrappin) { TimeTickingTrap(); }
    }
    public void DeadScreen()
    {
        jumpscareCamera.transform.GetChild(0).gameObject.SetActive(true);
        jumpscareCamera.cullingMask = LayerMask.GetMask("UI");
    }
    public void SetTimer() => timeLeftTrap = MAXTIMETRAP;
    private void DrawView()
    {
        Vector3 left = head.transform.position + Quaternion.Euler(new Vector3(0, viewAngle / 2f, 0)) * (head.transform.forward);
        Vector3 right = head.transform.position + Quaternion.Euler(-new Vector3(0, viewAngle / 2f, 0)) * (head.transform.forward);
        Debug.DrawLine(head.transform.position, left, Color.yellow);
        Debug.DrawLine(head.transform.position, right, Color.yellow);
    }
    public void PickUpItem()
    {
        item = point;
        item.transform.position = hand.transform.position;
        item.transform.rotation = hand.transform.rotation;
        item.transform.parent = hand.transform;
        item.GetComponent<Collider>().enabled = false;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().freezeRotation = true;
    }
    public void SoundTrigger(GameObject pointTo)
    {
        if (fsm.currentState.GetType() != typeof(MonsterAttack))
        {
            point = pointTo;
            fsm?.SetState<MonsterGoSoundPoint>();
        }
    }
    public void SetTrap(Transform point, Quaternion rotation, GameObject trapObject)
    {
        var trp = Instantiate(trapObject, point.position, rotation);
        traps.Add(trp.GetComponent<Monster_trap>());
        if (traps.Count == 5)
        {
            var trap_remove = traps[0];
            traps.Remove(traps[0]);
            Destroy(trap_remove);
        }         
    }
    private void TimeTickingTrap()
    {
        timeLeftTrap -= Time.deltaTime;
        if (timeLeftTrap < 0)
        {
            if(fsm.currentState.GetType() == typeof(MonsterPatrol)) { fsm.SetState<MonsterTrapin>(); }
            else { timeLeftTrap = 10f; }
        }
    }
    public void GetDamaged() { fsm.SetState<MonsterDamaging>(); }
    public void Seek(bool withItem)
    {
        RaycastHit[] hits = Physics.BoxCastAll(head.transform.position, new Vector3(15f, 15f, 15f), transform.forward, Quaternion.identity, 1f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<PlayerController>())
            {
                if (DetectPlayer(hit)) break;
            }
            else if (withItem && hit.collider.GetComponent<Item>())
            {
                if (DetectItem(hit)) break;
            }
        }
    }
    private bool DetectPlayer(RaycastHit _hit)
    {
        float realAngle = Vector3.Angle(head.transform.forward, _hit.transform.position - head.transform.position);
        if (realAngle <= GetView / 2f)
        {
            RaycastHit raycastHit;
            if (Physics.Linecast(gameObject.transform.position, _hit.transform.position, out raycastHit))
            {
                if (raycastHit.collider.GetComponent<PlayerController>())
                {
                    Debug.Log("DETECT");
                    player = raycastHit.collider.gameObject;
                    fsm.SetState<MonsterAttack>();
                    patience = patienceMax;
                    return true;
                }
            }
        }
        return false;
    }

    private bool DetectItem(RaycastHit _hit)
    {
        float realAngle = Vector3.Angle(head.transform.forward, _hit.transform.position - head.transform.position);
        if (realAngle <= GetView / 2f)
        {
            RaycastHit raycastHit;
            if (Physics.Linecast(gameObject.transform.position, _hit.transform.position, out raycastHit))
            {
                if (raycastHit.collider.GetComponent<Item>() && CanDetectItem())
                {
                    point = raycastHit.collider.gameObject;
                    fsm?.SetState<MonsterGoPoint>();
                    return true;
                }
            }
        }
        return false;
    }
    private bool CanDetectItem() => fsm.currentState.GetType() != typeof(MonsterAttack) && fsm.currentState.GetType() != typeof(MonsterPickUp) && item == null;
    public bool IsNearToPoint(Vector3 point) => Vector3.Distance(agent.transform.position, point) <= distanceToPoint;
    public void DoFootStep()
    {
        int index = UnityEngine.Random.Range(0, footStepClips.Length);
        if (tempIndexFoot == index) DoFootStep();
        else
        {
            tempIndexFoot = index;
            footSource.PlayOneShot(footStepClips[index], 13);
        }
    }
    public void DoRoar()
    {
        int index = UnityEngine.Random.Range(0, roarClips.Length);
        if (tempIndexRoar == index) DoRoar();
        else
        {
            tempIndexRoar = index;
            screamSource.PlayOneShot(roarClips[index]);
        }
    }
}