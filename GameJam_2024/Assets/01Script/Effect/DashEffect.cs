using UnityEngine;

public class DashEffect : BaseEffect
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DashEffectPlay()
    {
        _animator.SetBool("Dash", true);
    }
}
