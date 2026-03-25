using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Game/Score Config")]
public class ScoreConfig : ScriptableObject
{
    [Header("Score Values")]
    public int coinScore = 50;
    public int enemyScore = 0;
    public int bonusScore = 0;
    public int levelCompleteScore = 0;
}
