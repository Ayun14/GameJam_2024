using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string userName;
    public int maxPersent;
    public string clearTime;

    public UserData(string userName, int maxPersent, string clearTime)
    {
        this.userName = userName;
        this.maxPersent = maxPersent;
        this.clearTime = clearTime;
    }

    public void DebugData()
    {
        Debug.Log($"Name : {userName}, Score : {maxPersent}%, clearTime : {clearTime}");
    }
}
