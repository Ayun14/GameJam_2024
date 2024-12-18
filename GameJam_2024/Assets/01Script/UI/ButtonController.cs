using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _exitButton;

    public void OnStartButton()
    {
        Debug.Log("Start");
    }
}
