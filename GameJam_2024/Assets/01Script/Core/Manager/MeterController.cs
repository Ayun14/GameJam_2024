using TMPro;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private int _meterY; // ���� y���� 1M�� �� �� ����.

    [Header("Camera")]
    [SerializeField] private Transform _cameraPointTrm;
    private Vector2 _cameraPointOrigin;
    [SerializeField] private int _zoneY; // ���� y���� ������
    [SerializeField] private TextMeshProUGUI _text;

    private int _meter = 0;
    public int Meter => _meter;

    private void Start()
    {
        _meter = 0;
        _cameraPointOrigin = _cameraPointTrm.position;
    }

    private void FixedUpdate()
    {
        MeterCalculation();
        CameraUpdater();
    }

    private void MeterCalculation()
    {
        _meter = (int)_playerTrm.position.y / _meterY;
        _text.text = $"{_meter} M";
    }

    private void CameraUpdater()
    {
        int zoneMeter = _zoneY / _meterY; // ������ �� M����
        if (_meter % zoneMeter == 0)
        {
            Debug.Log("Camera +");
            float y = _zoneY * (_meter / zoneMeter);
            _cameraPointTrm.position = new Vector2(_cameraPointTrm.position.x,
                _cameraPointOrigin.y + y);
        }
    }
}
