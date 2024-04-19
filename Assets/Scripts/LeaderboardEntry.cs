using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string UserName;
    public int Score;
    public int Level;
}

[System.Serializable]
public class UserDataList
{
    public UserData[] entries;
}
