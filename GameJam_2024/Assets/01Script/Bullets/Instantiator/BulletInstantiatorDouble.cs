using UnityEngine;

//[CreateAssetMenu(menuName = "SO/BulletInstansiator_Double")]
public class BulletInstantiatorDouble : BaseBulletInstantiatorSO
{
    public override void InstantiateBullet(Transform instansiateTransform, Vector3 direction, BaseBullet prefab, float speed)
    {
        void Create(Vector3 dir)
        {
            dir.Normalize();
            //BaseBullet result = Instantiate(prefab, instansiateTransform.position, Quaternion.identity);
            GameObject go = PoolManager.Instance.Pop(prefab.name, instansiateTransform.position, Quaternion.identity);

            if (go.TryGetComponent(out BaseBullet bullet))
            {
                bullet.Init(dir, speed);
            }
        }
        Create(direction);
        Create(-direction);
    }
}
