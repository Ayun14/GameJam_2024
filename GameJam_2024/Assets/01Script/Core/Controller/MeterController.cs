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
    private float _sec = 0;
    private int _min = 0;
    private float _allTime = 0;

    private bool isPlaying = true;

    private void Start()
    {
        SoundController.Instance.PlayBGM(1);
    }

    private void Update()
    {
        if (isPlaying)
            Timer();
    }

    private void FixedUpdate()
    {
        PersentCalculation();
    }

    private void PersentCalculation()
    {
        float persent = (_playerTrm.position.y / _endY) * 100f;
        int persentClamp = Mathf.Clamp((int)persent, 0, 100);
        _persentText.text = $"{persentClamp} %";

        if (persentClamp > _maxPersent)
        {
            _maxPersent = persentClamp;
            SaveController.Instance.SaveHeight(_maxPersent);
        }
    }

    private void Timer()
    {
        _allTime += Time.deltaTime;
        _sec += Time.deltaTime;
        if (_sec >= 60f)
        {
            _min += 1;
            _sec = 0;
        }
    }

    // 클리어 타임 저장할 때 사용
    public void SaveTime()
    {
        _playTimeText.text = $"클리어 하는데 걸린 시간은 {_min}분 {(int)_sec}초 입니다!";

        isPlaying = false;

        if (PlayerPrefs.GetFloat("MaxTime") > _allTime)
        {
            SaveController.Instance.SaveMaxTime(_allTime);
            SaveController.Instance.SaveTime(_min, _sec);
        }

        _min = 0;
        _sec = 0;
        _allTime = 0;
    }
}
