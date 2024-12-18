using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private BaseBullet bullet;
    [SerializeField] private Transform target;
    [SerializeField] private Transform spawnPos;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnBullet();
    }
    private void SpawnBullet()
    {
        BaseBullet result = Instantiate(bullet, spawnPos.position, Quaternion.identity);
        result.Init(Vector3.right, target);
    }
}
