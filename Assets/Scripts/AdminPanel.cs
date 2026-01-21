using UnityEngine;
using UnityEngine.AI;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] Monster monster;
    [SerializeField] AILearning student;
    [SerializeField] NavMeshAgent agent;
    private void Start()
    {
        monster = FindObjectOfType<Monster>();
        student = FindObjectOfType<AILearning>();
        agent = FindObjectOfType<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            student.IQUp += 80;
        if (!agent.isStopped)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                agent.isStopped = true;
                Debug.Log("MONSTER: STOP");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                agent.isStopped = false;
                Debug.Log("MONSTER: NON STOP");
            }
        }
    }
}