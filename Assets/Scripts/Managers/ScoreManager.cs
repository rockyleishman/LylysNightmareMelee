using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] public TextMeshProUGUI ScoreTextField;

    public void AddScore(int score)
    {
        DataManager.Instance.PlayerDataObject.Score += score;
        ScoreTextField.text = "Score: " + DataManager.Instance.PlayerDataObject.Score;
    }
}
