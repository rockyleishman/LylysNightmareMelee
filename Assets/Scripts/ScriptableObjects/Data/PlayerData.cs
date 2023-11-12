using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Data/PlayerDataObject", order = 0)]
public class PlayerData : ScriptableObject
{
    [SerializeField] public PlayerController Player;
    [SerializeField] public GameObject Cursor;

    [SerializeField] public float MaxHP = 100.0f;
    internal float CurrentHP;

    [SerializeField] public float MovementSpeed = 5.0f;

    [SerializeField] public float InitialPillowAttackDamage = 10.0f;
    [SerializeField] public float PillowAttackActiveTime = 0.1f;

    internal float SpecialCharge;
}
