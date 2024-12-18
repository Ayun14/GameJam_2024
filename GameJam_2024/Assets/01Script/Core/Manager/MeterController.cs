using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private int _meterY; // ���� y���� 1M�� �� �� ����.
    [SerializeField] private TextMeshProUGUI _text;

    private int _meter = 0;
    public int Meter => _meter;

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
        _meter = (int)_playerTrm.position.y / _meterY;
        _text.text = $"{_meter} M";
    }
}
