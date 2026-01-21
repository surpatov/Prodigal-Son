using UnityEngine;
using UnityEngine.Events;

public class AILearning : MonoBehaviour
{
    private Monster monster;
    [SerializeField] private int IQ;
    public int GetIQ => IQ;
    public int IQUp
    {
        get => IQ;
        set
        {
            IQ += Mathf.Abs(value);
            OnIQSet();
        }
    }

    [SerializeField] private int IqLevel;
    public int GetIQLevel() => IqLevel;
    private void OnIQSet()
    {
        if (Mathf.RoundToInt(IQ / 100) > IqLevel)
        {
            IqLevel += 1;
            switch (IqLevel)
            {
                case 1:
                    Debug.Log("IQ up level: 1");
                    monster.fsm.AddState(new MonsterGoSoundPoint(monster.fsm, monster.agent, monster.animator, monster));
                    break;

                    case 2:
                    Debug.Log("IQ up level: 2");
                    monster.fsm.AddState(new MonsterPickUp(monster.fsm, monster.agent, monster.animator, monster.timePicking, monster));
                    monster.fsm.AddState(new MonsterGoPoint(monster.fsm, monster.agent, monster.animator, monster));
                    monster.fsm.AddState(new MonsterDropItem(monster.fsm, monster.agent, monster));
                    break;

                case 3:
                    Debug.Log("IQ up level: 3");
                    monster.fsm.AddState(new MonsterTrapin(monster.fsm, monster.agent, monster, monster.trap, monster.layer));
                    monster.SetTimer();
                    monster.canTrappin = true;
                    break;
            }
        }
    }
    private void Start()
    {
        IqLevel = 0;
        monster = GetComponent<Monster>();
    }
}