using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameEvent _tutorialStarted;
    [SerializeField] private GameEvent _tutorialEnded;

    private void Start()
    {
        _tutorialStarted?.Raise(this, true);
    }

    private void OnDisable()
    {
        _tutorialStarted?.Raise(this, false);
    }

    void Update()
    {
        if(InputManager.Instance.IsContinuePressed) 
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
