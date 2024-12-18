using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "SO/BulletInstantiator_Single")]
public class BulletInstantiatorSingle : BaseBulletInstantiatorSO
{
    public override void InstantiateBullet(Transform instansiateTransform, Vector3 direction, BaseBullet prefab)
    {
        direction.Normalize();
        BaseBullet result = Instantiate(prefab, instansiateTransform.position, Quaternion.identity);
        result.Init(direction);
    }
}
