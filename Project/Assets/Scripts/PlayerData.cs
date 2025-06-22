using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string Name = string.Empty;
    public int Highscore = 0;
    public int PlayerId = 0;
    public int Score = 0;
}

[Serializable]
public class PlayerDataList
{
    public List<PlayerData> Players;
}