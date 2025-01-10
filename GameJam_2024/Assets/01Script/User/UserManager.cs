using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoSingleton<UserManager>
{
    public string UserId { get; private set; } // 로그인한 유저의 ID
    public string UserName { get; private set; } // 로그인한 유저의 이름

    public void SetUserInfo(string userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}
