using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] public PlayerData PlayerDataObject;
    [SerializeField] public LevelData LevelDataObject;
}
