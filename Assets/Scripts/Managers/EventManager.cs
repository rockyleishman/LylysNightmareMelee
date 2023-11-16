using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Attack Events")]
    [SerializeField] public AttackEvent PillowAttackTriggered;

    [Header("Camera Events")]
    [SerializeField] public GameEvent ScreenShakeTriggered;
}
