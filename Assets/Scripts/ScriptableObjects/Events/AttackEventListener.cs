using UnityEngine;
using UnityEngine.Events;

public class AttackEventListener : MonoBehaviour
{
    public AttackEvent AttackEvent;
    public UnityEvent<Vector3, Vector3, MonoBehaviour> OnEventTriggered;

    private void OnEnable()
    {
        AttackEvent.AddListener(this);
    }

    private void OnDisable()
    {
        AttackEvent.RemoveListener(this);
    }

    public void OnTriggered(Vector3 attackOrigin, Vector3 attackDirection, MonoBehaviour attackTarget)
    {
        OnEventTriggered.Invoke(attackOrigin, attackDirection, attackTarget);
    }
}