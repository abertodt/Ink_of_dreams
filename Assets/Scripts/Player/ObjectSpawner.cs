using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public void SpawnObject(Component sender, object data)
    {
        if(data is GameObject)
        {
            Debug.Log(sender.name);
            Instantiate((GameObject)data, transform.position, transform.rotation);
            GameManager.Instance.ToggleDrawMode();
        }
    }
}
