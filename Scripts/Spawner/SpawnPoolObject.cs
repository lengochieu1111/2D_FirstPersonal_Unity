using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnPoolObject : RyoMonoBehaviour
{
    [SerializeField] private List<Transform> _prefabObjects;
    [SerializeField] private List<Transform> _poolObjects;
    [SerializeField] private Transform _prefabsHolder;
    [SerializeField] private int _iSpawnCount = 0;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        /* Load Prefabs */
        if (this._prefabObjects.Count == 0)
            this.LoadPrefabs();

        /* Load Holder */
        if (_prefabsHolder == null)
            this._prefabsHolder = this.transform.Find("Holder");
    }

    protected virtual void LoadPrefabs()
    {
        // Load Prefabs
        Transform prefabs = this.transform.Find("Prefabs");
        foreach (Transform prefab in prefabs)
            this._prefabObjects.Add(prefab);

        // Hide Prefabs
        foreach (Transform prefab in this._prefabObjects)
            prefab.gameObject.SetActive(false);

    }

    public virtual Transform SpawnObject(string objectName, Vector3 spawnPosition, Quaternion rotation)
    {
        Transform prefabObject = this.GetPrefabObjectByName(objectName);

        if (prefabObject == null)
        {
            Debug.LogWarning("Prefab not found : " + objectName, this.gameObject);
            return null;
        }

        Transform poolObject = GetObjectFromPoolObjects(prefabObject);
        poolObject.SetPositionAndRotation(spawnPosition, rotation);
        this._iSpawnCount++;
        return poolObject;

    }

    protected virtual Transform GetPrefabObjectByName(string objectName)
    {
        foreach (Transform prefabObject in this._prefabObjects)
        {
            if (prefabObject.name == objectName)
                return prefabObject;
        }

        return null;
    }

    protected virtual Transform GetObjectFromPoolObjects(Transform prefabObject)
    {
        foreach (Transform poolObject in this._poolObjects)
        {
            if (poolObject.name == prefabObject.name)
            {
                this._poolObjects.Remove(poolObject);
                return poolObject;
            }
        }

        Transform newPrefab = Instantiate(prefabObject);
        newPrefab.name = prefabObject.name;
        if (this._prefabsHolder != null)
            newPrefab.SetParent(this._prefabsHolder);

        return newPrefab;
    }

    public void DestroyObject(GameObject prefabObject)
    {
        if (this._poolObjects.Contains(prefabObject.transform)) return;
        this._poolObjects.Add(prefabObject.transform);
        this._iSpawnCount--;
        prefabObject.transform.SetParent(this._prefabsHolder);
        prefabObject.SetActive(false);
    }


}
