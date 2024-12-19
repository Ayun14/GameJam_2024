using System.Collections.Generic;
using UnityEngine;

public class Tutorial_RespawnBullet : MonoBehaviour
{
    [SerializeField] private List<Transform> bulletSpawnTransform;
    private List<BaseBullet> spawnedBullets = new();
    [SerializeField] private BaseBullet bulletToSpawn;
    [SerializeField] private GameObject spawnParticle;
    public void OnEnter()
    {
        for (int i = spawnedBullets.Count - 1; i >= 0; i--)
        {
            var result = spawnedBullets[i];
            if (result == null) continue;
            result.Kill();
        }
        spawnedBullets.Clear();
        foreach (var item in bulletSpawnTransform)
        {
            BaseBullet bullet = Instantiate(bulletToSpawn, item.position, Quaternion.identity, item);
            spawnedBullets.Add(bullet);
        }
    }
}
