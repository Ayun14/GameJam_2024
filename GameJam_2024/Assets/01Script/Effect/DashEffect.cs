using Unity.VisualScripting;
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
        _animator.SetBool("Dash", true);
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
