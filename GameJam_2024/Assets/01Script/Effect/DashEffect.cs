using UnityEngine;

public class DashEffect : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DashEffectPlay()
    {
        _animator.SetTrigger("Dash");
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
