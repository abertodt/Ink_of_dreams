using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _splatterEffect;

    [Header("Events")]
    [SerializeField] private GameEvent _onObjectSpawn;
    public void SpawnObject(Component sender, object data)
    {
        if(data is GameObject)
        {
            Instantiate((GameObject)data, transform.position, transform.rotation);
            //_onObjectSpawn?.Raise(this, data);
            GameManager.Instance.ChangeState(GameState.Gameplay);
        }
    }
}
