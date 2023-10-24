using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SpawnPoolObject
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance => instance;

    [Header("Prefabs")]
    [SerializeField] private readonly static string _enemySkeleton = "Enemy_Skeleton";
    public static string EnemySkeleton => _enemySkeleton;

    protected override void Awake()
    {
        if (EnemySpawner.instance != null) return;

        EnemySpawner.instance = this;

        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Transform effect = EnemySpawner.Instance.SpawnObject(EnemySpawner.EnemySkeleton, this.transform.position, this.transform.rotation);
            effect.gameObject.SetActive(true);
        }
    }
}
