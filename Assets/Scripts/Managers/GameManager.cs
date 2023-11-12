using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController Player;
    [SerializeField] GameObject Cursor;

    private void Awake()
    {
        //set references
        DataManager.Instance.PlayerDataObject.Player = Player;
        DataManager.Instance.PlayerDataObject.Cursor = Cursor;
    }
}
