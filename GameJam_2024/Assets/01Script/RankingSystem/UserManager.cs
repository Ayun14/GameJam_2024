using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    [Header("Main Pannel")]
    [SerializeField] private GameObject _loginPannel; 
    [SerializeField] private GameObject _registerPannel;

    #region Main Buttons

    public void LoginPannelOn()
    {
        _loginPannel.SetActive(true);
    }
    public void LoginPannelOff()
    {
        _loginPannel.SetActive(false);
    }
    public void RegisterPannelOn()
    {
        _registerPannel.SetActive(true);
    }
    public void RegisterPannelOff()
    {
        _registerPannel.SetActive(false);
    }

    #endregion
}
