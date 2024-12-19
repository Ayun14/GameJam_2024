using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    [SerializeField] private string _effectName;

    private void OnDisable()
    {
        PoolManager.Instance.Push(_effectName, gameObject);
    }
}
