using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraShakeController : MonoBehaviour, IEntityComponent
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _shakeTime;

    public void Initialize(Entity entity)
    {
    }

    public void CameraShake()
    {
        float endValue = 6.8f;
        DOTween.To(() => _virtualCamera.m_Lens.OrthographicSize,
                   x => _virtualCamera.m_Lens.OrthographicSize = x,
                   endValue, _shakeTime)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                endValue = 7f;
                DOTween.To(() => _virtualCamera.m_Lens.OrthographicSize,
                           x => _virtualCamera.m_Lens.OrthographicSize = x,
                           endValue, _shakeTime)
                    .SetEase(Ease.InExpo);
            });
    }
}
