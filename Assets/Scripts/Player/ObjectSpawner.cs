using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _splatterEffect;
    [SerializeField] private const int MaxSpawnedObjects = 3;

    [Header("Events")]
    [SerializeField] private GameEvent _onObjectSpawn;

    private Queue<GameObject> _spawnedObjects = new Queue<GameObject>();
    

    public void SpawnObject(Component sender, object data)
    {
        if(data is GameObject)
        {
            GameObject prefab = (GameObject)data;
            GameObject spawnedObject = Instantiate(prefab, transform.position, prefab.transform.rotation);

            if (_splatterEffect != null)
            {
                GameObject effect = Instantiate(_splatterEffect, transform.position, transform.rotation);
                Destroy(effect, 2f);
            }

            _spawnedObjects.Enqueue(spawnedObject);

            if (_spawnedObjects.Count > MaxSpawnedObjects)
            {
                GameObject oldestObject = _spawnedObjects.Dequeue();
                if (oldestObject != null)
                {
                    Destroy(oldestObject);
                }
            }

            //_onObjectSpawn?.Raise(this, data);
            GameManager.Instance.ChangeState(GameState.Gameplay);
        }
    }
}
