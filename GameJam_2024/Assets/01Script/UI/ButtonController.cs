using DG.Tweening;
using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _settingCanvas;
    [SerializeField] private CanvasGroup _settingCanvasGroup;
    [SerializeField] private RectTransform _settingRectTransform;

    [SerializeField] private CanvasGroup _fadeCanvas;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private AudioMixer _audioMixer;

    private void Start()
    {
        SoundController.Instance.PlayBGM(0);

        DataLoad();
    }

    private void DataLoad()
    {
        SaveController.Instance.LoadData();

        if (!PlayerPrefs.HasKey("BGM"))
            SaveController.Instance.SaveBGM(_musicSlider.value);
        else
        {
            _musicSlider.value = PlayerPrefs.GetFloat("BGM");
            _audioMixer.SetFloat("BGM", Mathf.Log10(_musicSlider.value) * 20);
        }

        if (!PlayerPrefs.HasKey("SFX"))
            SaveController.Instance.SaveSFX(_sfxSlider.value);
        else
        {
            _sfxSlider.value = PlayerPrefs.GetFloat("SFX");
            _audioMixer.SetFloat("SFX", Mathf.Log10(_sfxSlider.value) * 20);
        }
    }

    public void OnStartButton()
    {
        ButtonClickSound();
        StartCoroutine(Fade());
    }

    public void OnSettingButton()
    {
        ButtonClickSound();
        _settingCanvas.SetActive(true);
        _settingCanvasGroup.alpha = 1;
        StartCoroutine(ShowSetting(true));
    }

    public void OnExitButton()
    {
        ButtonClickSound();

        Application.Quit();
    }

    public void OnXButton()
    {
        ButtonClickSound();
        StartCoroutine(ShowSetting(false));
    }

    private IEnumerator Fade()
    {
        _fadeCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 1f);
        yield return tween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator ShowSetting(bool isOpen)
    {
        float endPosX = isOpen ? 380f : 1300f;
        Tween tween = _settingRectTransform.DOAnchorPosX(endPosX, 0.5f);
        yield return tween.WaitForCompletion();
        if(!isOpen)
        {
            _settingCanvasGroup.alpha = 0;
            _settingCanvas.SetActive(false);
        }
    }

    public void BGMVolumeChange()
    {
        SaveController.Instance.SaveBGM(_musicSlider.value);

        _audioMixer.SetFloat("BGM", Mathf.Log10(_musicSlider.value)*20);
    }

    public void SFXVolumeChange()
    {
        SaveController.Instance.SaveSFX(_sfxSlider.value);

        _audioMixer.SetFloat("SFX", Mathf.Log10(_sfxSlider.value) * 20);
    }

    private void ButtonClickSound()
    {
        SoundController.Instance.PlaySFX(0);
    }
}
