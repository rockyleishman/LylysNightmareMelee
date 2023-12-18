using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Attack Events")]
    [SerializeField] public GameEvent PillowAttackTriggered;
    [SerializeField] public GameEvent SecondaryAttackTriggered;
    [SerializeField] public GameEvent SpecialAttackTriggered;
    [SerializeField] public GameEvent TrailOfAssuranceTriggered;

    [Header("Game Events")]
    [SerializeField] public GameEvent PauseGameTriggered;
    [SerializeField] public GameEvent OnGamePaused;
    [SerializeField] public GameEvent ResumeGameTriggered;
    [SerializeField] public GameEvent OnGameResumed;
    [SerializeField] public GameEvent GameOverTriggered;
    [SerializeField] public GameEvent VictoryTriggered;
    [SerializeField] public GameEvent IncreaseThreat;
    [SerializeField] public GameEvent UpgradeTriggered;

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
