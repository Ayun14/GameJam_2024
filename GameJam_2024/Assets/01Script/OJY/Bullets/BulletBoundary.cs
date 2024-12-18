using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-150)]
[OJYSewer.MonoSingletonUsage(OJYSewer.MonoSingletonFlags.CustomRuntimeInitialize)]
public class BulletBoundary : OJYSewer.MonoSingleton<BulletBoundary>
{
    public Vector3 Min { get; private set; }
    public Vector3 Max { get; private set; }
    [SerializeField] private Transform minTransform;
    [SerializeField] private Transform maxTransform;
    protected override void Awake()
    {
        base.Awake();
        if(minTransform != null && maxTransform != null)
        {
            Min = minTransform.position;
            Max = maxTransform.position;
            if (Min.x > Max.x || Min.y > Max.y) Debug.LogError("[ERRROR] min, max transform is wrong. min transform should be lower than max transform");
        }
    }
    protected override void CustomRuntimeInitializeEvent()
    {
        minTransform = new GameObject("minTrnasform").transform;
        minTransform.parent = transform;
        minTransform.position = new Vector3(-75, -75);

        maxTransform = new GameObject("maxTransform").transform;
        maxTransform.parent = transform;
        maxTransform.position = new Vector3(75, 75);
    }
    private void Update()
    {
        Min = minTransform.position;
        Max = maxTransform.position;
    }
    private void OnDrawGizmos()
    {
        if(minTransform != null && maxTransform != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 min = minTransform.position;
            Vector3 max = maxTransform.position;
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
