using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBulletInstantiatorSO : ScriptableObject
{
    public abstract void InstantiateBullet(Transform instansiateTransform, Vector3 direction, BaseBullet prefab, float speed);
}
