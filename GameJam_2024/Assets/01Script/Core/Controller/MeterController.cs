using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{

    [Header("Persent")]
    [SerializeField] private float _endY = 917f;
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private TextMeshProUGUI _persentText;
    private int _maxPersent = 0;

    [Header("Time")]
    [SerializeField] private TextMeshProUGUI _playTimeText;
    [SerializeField] private TextMeshProUGUI _endingTimeText;
    private float _sec = 0;
    private int _min = 0;
    private int _hour = 0;
    private float _currentTime = 0;

    private bool isPlaying = true;

    private void Start()
    {
        SoundController.Instance.PlayBGM(1);
    }

    private void Update()
    {
        if (isPlaying)
        {
            PersentCalculation();
            Timer();
        }
    }

    private void PersentCalculation()
    {
        float persent = (_playerTrm.position.y / _endY) * 100f;
        int persentClamp = Mathf.Clamp((int)persent, 0, 100);
        _persentText.text = $"{persentClamp} %";

        if (persentClamp > _maxPersent)
        {
            _maxPersent = persentClamp;
            SaveController.Instance.SavePersent(_maxPersent);
        }
    }

    private void Timer()
    {
        _currentTime += Time.deltaTime;
        _sec += Time.deltaTime;
        if (_sec >= 60f)
        {
            _min += 1;
            _sec = 0;

            if (_min >=60f)
            {
                _hour += 1;
                _min = 0;
            }
        }
        _playTimeText.text = $"{_hour.ToString("D2")}h {_min.ToString("D2")}m {Mathf.FloorToInt(_sec):D2}s";
    }

    // 클리어 타임 저장할 때 사용
    public void SaveTime()
    {
        isPlaying = false;
        _endingTimeText.text = $"The time it took to clear the stage is {_hour:D2}h {_min:D2}m {Mathf.FloorToInt(_sec):D2}s!";
        SaveController.Instance.SaveTime(_hour, _min, Mathf.FloorToInt(_sec));

        _hour = 0;
        _min = 0;
        _sec = 0;
        _currentTime = 0;
    }
}
