using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public void SpawnObject(Component sender, object data)
    {
        if(sender is InputManager && data is GameObject)
        {
            Instantiate((GameObject)data, transform.position, transform.rotation);
        }
    }
}
