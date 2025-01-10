using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoSingleton<UserManager>
{
    public string UserId { get; private set; } // �α����� ������ ID
    public string UserName { get; private set; } // �α����� ������ �̸�

    public void SetUserInfo(string userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}
