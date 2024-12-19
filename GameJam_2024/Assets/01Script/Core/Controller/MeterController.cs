using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private float _meterY; // 얼마의 y값이 1M가 될 것 인지.
    [SerializeField] private TextMeshProUGUI _text;

    private float _meter = 0;
    private float _maxMeter =0;
    public float Meter => _meter;

    private float _sec = 0;
    private int _min = 0;

    private void Start()
    {
        _meter = 0;
    }

    private void Update()
    {
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

        if(_meter > _maxMeter)
        {
            _maxMeter = _meter;
            SaveController.Instance.SaveHeight(_maxMeter);
        }
    }

    private void Timer()
    {
        _sec += Time.deltaTime;
        if (_sec >= 60f)
        {
            _min += 1;
            _sec = 0;
        }
    }

    // 클리어 타임 저장할 때 사용
    private void SaveTime()
    {
        SaveController.Instance.SaveTime(_min, _sec);
        _min = 0;
        _sec = 0;
    }
}
