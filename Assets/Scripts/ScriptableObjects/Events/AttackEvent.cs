using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackEventObject", menuName = "Event/AttackEvent", order = 0)]
public class AttackEvent : ScriptableObject
{
    private List<AttackEventListener> _listeners = new List<AttackEventListener>();

    public void AddListener(AttackEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(AttackEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void TriggerEvent(Vector3 attackOrigin, Vector3 attackDirection, MonoBehaviour attackTarget)
    {
        foreach (AttackEventListener listener in _listeners)
        {
            listener.OnTriggered(attackOrigin, attackDirection, attackTarget);
        }
    }
}