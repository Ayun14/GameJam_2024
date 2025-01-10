using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string userName;
    public int maxPercent;
    public string clearTime;

    public UserData(string userName, int maxPercent, string clearTime)
    {
        this.userName = userName;
        this.maxPercent = maxPercent;
        this.clearTime = clearTime;
    }

    public void DebugData()
    {
        Debug.Log($"Name : {userName}, Score : {maxPercent}%, clearTime : {clearTime}");
    }
}
