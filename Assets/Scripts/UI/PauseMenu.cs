using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RetryLevel()
    {
        //Esto no deberia estar aqui, cambiar mas adelante para hacerlo bien (cambiar de gamestate deberia ser responsabilidad de gamemanager)
        GameManager.Instance.ChangeState(GameState.Gameplay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
