using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : SpawnPoolObject
{
    private static EffectSpawner instance;
    public static EffectSpawner Instance => instance;

    [Header("Prefabs")]
    [SerializeField] private readonly static string _bloodEffect = "BloodEffect";
    public static string BloodEffect => _bloodEffect;

    protected override void Awake()
    {
        if (EffectSpawner.instance != null) return;

        EffectSpawner.instance = this;

        base.Awake();
    }

/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Transform effect = EffectSpawner.Instance.SpawnObject(EffectSpawner.BloodEffect, this.transform.position, this.transform.rotation);
            effect.gameObject.SetActive(true);
        }
    }*/

}
