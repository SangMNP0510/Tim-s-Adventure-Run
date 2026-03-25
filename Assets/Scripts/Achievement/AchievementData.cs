using System;

[Serializable]
public class AchievementData
{
    public string id;
    public string name;
    public int target;
    public int reward;

    public int current;
    public bool isCompleted;
    public bool isClaimed;
}