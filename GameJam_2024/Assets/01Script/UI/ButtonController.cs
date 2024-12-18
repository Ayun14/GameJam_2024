using DG.Tweening;
using Hellmade.Sound;
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
    [SerializeField] private CanvasGroup _fadeCanvas;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        SoundController.Instance.PlayBGM(0);
    }

    public void OnStartButton()
    {
        ButtonClickSound();
        StartCoroutine(Fade());
    }

    public void OnSettingButton()
    {
        ButtonClickSound();

        _settingPanel.alpha = 1;
    }

    public void OnExitButton()
    {
        ButtonClickSound();

        Application.Quit();
    }

    public void OnXButton()
    {
        ButtonClickSound();

        _settingPanel.alpha = 0;
    }

    private IEnumerator Fade()
    {
        _fadeCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 1f);
        yield return tween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    public void BGMVolumeChange()
    {
        EazySoundManager.GlobalMusicVolume = _musicSlider.value;
    }

    public void SFXVolumeChange()
    {
        EazySoundManager.GlobalSoundsVolume = _sfxSlider.value;
    }

    private void ButtonClickSound()
    {
        SoundController.Instance.PlaySFX(0);
    }
}
