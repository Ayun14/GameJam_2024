using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private float _meterY; // 얼마의 y값이 1M가 될 것 인지.
    [SerializeField] private TextMeshProUGUI _text;

    private float _meter = 0;
    public float Meter => _meter;

    private void Start()
    {
        _meter = 0;
    }

    private void FixedUpdate()
    {
        MeterCalculation();
    }

    private void MeterCalculation()
    {
        _meter = _playerTrm.position.y / _meterY;
        _text.text = $"{(int)_meter} M";
    }
}
