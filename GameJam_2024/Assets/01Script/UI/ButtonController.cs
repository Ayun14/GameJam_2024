using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private CanvasGroup _settingPanel;
    [SerializeField] private FadeController _fadeController;

    public void OnStartButton()
    {
        _fadeController.FadeIn();
        SceneManager.LoadScene(0);
    }

    public void OnSettingButton()
    {
        _settingPanel.alpha = 1;
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnXButton()
    {
        _settingPanel.alpha = 0;
    }
}
