using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Attack Events")]
    [SerializeField] public AttackEvent PillowAttackTriggered;
    [SerializeField] public AttackEvent SpecialAttackTriggered;

    [Header("Camera Events")]
    [SerializeField] public GameEvent ScreenShakeTriggered;
}
