using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "SO/BulletInstansiator_Double")]
public class BulletInstantiatorDouble : BaseBulletInstantiatorSO
{
    public override void InstantiateBullet(Transform instansiateTransform, Vector3 direction, BaseBullet prefab)
    {
        void Create(Vector3 dir)
        {
            dir.Normalize();
            BaseBullet result = Instantiate(prefab, instansiateTransform.position, Quaternion.identity);
            result.Init(dir);
        }
        Create(direction);
        Create(-direction);
    }
}
