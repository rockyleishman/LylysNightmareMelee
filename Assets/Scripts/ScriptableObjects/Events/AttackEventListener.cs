using UnityEngine;
using UnityEngine.Events;

public class AttackEventListener : MonoBehaviour
{
    public AttackEvent AttackEvent;
    public UnityEvent<MonoBehaviour, MonoBehaviour> OnEventTriggered;

    private void OnEnable()
    {
        AttackEvent.AddListener(this);
    }

    private void OnDisable()
    {
        AttackEvent.RemoveListener(this);
    }

    public void OnTriggered(MonoBehaviour attacker, MonoBehaviour attackTarget)
    {
        OnEventTriggered.Invoke(attacker, attackTarget);
    }
}