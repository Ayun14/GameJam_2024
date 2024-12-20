using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private float _meterY; // ���� y���� 1M�� �� �� ����.
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _playTimeText;

    private float _meter = 0;
    private float _maxMeter = 0;
    public float Meter => _meter;

    private float _sec = 0;
    private int _min = 0;
    private float _allTime = 0;

    private bool isPlaying = true;

    private void Start()
    {
        _meter = 0;
        SoundController.Instance.PlayBGM(1);
    }

    private void Update()
    {
        if (isPlaying)
            Timer();
    }

    private void FixedUpdate()
    {
        MeterCalculation();
    }

    private void MeterCalculation()
    {
        _meter = _playerTrm.position.y / _meterY;
        _text.text = $"{(int)_meter} M";

        if (_meter > _maxMeter)
        {
            _maxMeter = _meter;
            SaveController.Instance.SaveHeight(_maxMeter);
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

    // Ŭ���� Ÿ�� ������ �� ���
    public void SaveTime()
    {
        _playTimeText.text = $"Ŭ���� �ϴµ� �ɸ� �ð��� {_min}�� {(int)_sec}�� �Դϴ�!";

        isPlaying = false;

        if (PlayerPrefs.GetFloat("MaxTime") < _allTime)
        {
            SaveController.Instance.SaveMaxTime(_allTime);
            SaveController.Instance.SaveTime(_min, _sec);
        }

        _min = 0;
        _sec = 0;
        _allTime = 0;
    }
}
