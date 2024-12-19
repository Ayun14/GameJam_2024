using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    [SerializeField] private string _effectName;
    [SerializeField] private float _disableTime;
    private float _currentTime = 0;

    private void OnEnable()
    {
        _currentTime = 0;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _disableTime)
        {
            _currentTime = 0;
            PoolManager.Instance.Push(_effectName, gameObject);
        }
    }
}
