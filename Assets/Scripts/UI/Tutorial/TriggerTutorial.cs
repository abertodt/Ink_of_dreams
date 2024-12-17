using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialToTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("hola");
            _tutorialToTrigger.SetActive(true);
        }
    }
}
