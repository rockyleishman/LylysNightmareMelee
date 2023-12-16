using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Attack Events")]
    [SerializeField] public GameEvent PillowAttackTriggered;
    [SerializeField] public GameEvent SecondaryAttackTriggered;
    [SerializeField] public GameEvent SpecialAttackTriggered;
    [SerializeField] public GameEvent TrailOfAssuranceTriggered;

    [Header("Game Events")]
    [SerializeField] public GameEvent PauseGame;
    [SerializeField] public GameEvent ResumeGame;
    [SerializeField] public GameEvent GameOverTriggered;
    [SerializeField] public GameEvent VictoryTriggered;
    [SerializeField] public GameEvent IncreaseThreat;
    [SerializeField] public GameEvent TriggerUpgrade;

    [Header("Player Events")]
    [SerializeField] public GameEvent PlayerDamaged;
    [SerializeField] public GameEvent PlayerKilled;

    [Header("Enemy Events")]
    [SerializeField] public GameEvent EnemyDamaged;
    [SerializeField] public GameEvent EnemyKilled;

    [Header("Camera Events")]
    [SerializeField] public GameEvent ScreenShakeTriggered;
    [SerializeField] public GameEvent LookAtMirrorTriggered;
}
