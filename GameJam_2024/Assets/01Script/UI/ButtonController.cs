using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("Setting Canvas")]
    [SerializeField] private GameObject _settingCanvas;
    [SerializeField] private CanvasGroup _settingCanvasGroup;
    [SerializeField] private RectTransform _settingRectTransform;

    [Header("RankingBoard Canvas")]
    [SerializeField] private GameObject _rankingCanvas;
    [SerializeField] private CanvasGroup _rankingCanvasGroup;
    [SerializeField] private RectTransform _rankingRectTransform;

    [Header("Fade Canvas")]
    [SerializeField] private CanvasGroup _fadeCanvas;

    [Header("Setting Value")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioMixer _audioMixer;

    [Header("MainGame Value")]
    [SerializeField] private Player _player;
    private Vector2 _playerStartPos;

    private bool _isSettingOpen = false;
    private bool _isBestOpen = false;

    private void Start()
    {
        DataLoad();

        if(_player != null)
            _playerStartPos = _player.transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isSettingOpen)
                OnXSettingButton();
            else if (_isBestOpen)
                OnXRankingButton();
            else if(SceneManager.GetActiveScene().name == "MainScene")
                OnSettingButton();
        }
    }

    private void DataLoad()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            SoundController.Instance.PlayBGM(0);
        }

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
        StartCoroutine(Fade("MainScene"));
    }

    public void OnTutotialButton()
    {
        ButtonClickSound();
        // ø©±‚ ∆©≈‰∏ÆæÛ æ¿¿∏∑Œ πŸ≤„¡‡æﬂ«‘
        StartCoroutine(Fade("TutorialScene"));
    }

    public void OnRankingButton()
    {
        _isBestOpen = true;

        ButtonClickSound();
        _rankingCanvas.SetActive(true);
        _rankingCanvasGroup.alpha = 1;
        StartCoroutine(ShowRanking(true));
    }

    public void OnSettingButton()
    {
        _isSettingOpen = true;
        //if (_player != null)
        //    _player.InputCompo.enabled = false;

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

    public void OnXSettingButton()
    {
        _isSettingOpen = false;
        //if (_player != null)
        //    _player.InputCompo.enabled = true;

        ButtonClickSound();
        StartCoroutine(ShowSetting(false));
    }

    public void OnXRankingButton()
    {
        _isBestOpen = false;

        ButtonClickSound();
        StartCoroutine(ShowRanking(false));
    }

    private IEnumerator Fade(string sceneName)
    {
        _fadeCanvas.alpha = 0;
        Tween tween = _fadeCanvas.DOFade(1f, 1f);
        yield return tween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator ShowSetting(bool isOpen)
    {
        float minPosX = SceneManager.GetActiveScene().name == "MainScene" ? 0f : 380f;
        float endPosX = isOpen ? minPosX : 1300f;
        Tween tween = _settingRectTransform.DOAnchorPosX(endPosX, 0.5f);
        yield return tween.WaitForCompletion();
        if (!isOpen)
        {
            _settingCanvasGroup.alpha = 0;
            _settingCanvas.SetActive(false);
        }
    }

    private IEnumerator ShowRanking(bool isOpen)
    {
        float endPosX = isOpen ? 380f : 1300f;
        Tween tween = _rankingRectTransform.DOAnchorPosX(endPosX, 0.5f);
        yield return tween.WaitForCompletion();
        if (!isOpen)
        {
            _rankingCanvasGroup.alpha = 0;
            _rankingCanvas.SetActive(false);
        }
    }

    public void BGMVolumeChange()
    {
        SaveController.Instance.SaveBGM(_musicSlider.value);

        _audioMixer.SetFloat("BGM", Mathf.Log10(_musicSlider.value) * 20);
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

    public void OnReStartButton()
    {
        _player.transform.position = _playerStartPos;
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
