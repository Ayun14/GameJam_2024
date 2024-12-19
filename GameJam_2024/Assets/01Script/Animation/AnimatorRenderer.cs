using UnityEngine;

public class AnimatorRenderer : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public void SetParam(AnimatorParamSO param, bool value) => _animator.SetBool(param.hashValue, value);
    public void SetParam(AnimatorParamSO param, float value) => _animator.SetFloat(param.hashValue, value);
    public void SetParam(AnimatorParamSO param, int value) => _animator.SetInteger(param.hashValue, value);
    public void SetParam(AnimatorParamSO param) => _animator.SetTrigger(param.hashValue);
}
