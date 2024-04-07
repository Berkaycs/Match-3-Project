using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string UserName;
    public int Score;
}

[System.Serializable]
public class LeaderboardList
{
    public LeaderboardEntry[] entries;
}
