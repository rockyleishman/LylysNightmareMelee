using UnityEngine;

public class PlayerData : ScriptableObject
{
    [SerializeField] public PlayerController Player;
    [SerializeField] public GameObject Cursor;

    [SerializeField] public float MaxHP = 100.0f;
    internal float CurrentHP;

    [SerializeField] public float MovementSpeed = 5.0f;

    internal float SpecialCharge;
}
