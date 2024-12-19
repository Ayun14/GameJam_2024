using UnityEngine;

//[CreateAssetMenu(menuName = "SO/BulletInstantiator_Single")]
public class BulletInstantiatorSingle : BaseBulletInstantiatorSO
{
    public override void InstantiateBullet(Transform instansiateTransform, Vector3 direction, BaseBullet prefab, float speed)
    {
        direction.Normalize();
        // BaseBullet result = Instantiate(prefab, instansiateTransform.position, Quaternion.identity);
        //result.Init(direction, speed);
        GameObject go = PoolManager.Instance.Pop(prefab.name, instansiateTransform.position, Quaternion.identity);

        if (go.TryGetComponent(out BaseBullet bullet))
        {
            bullet.Init(direction, speed);
        }
    }
}
