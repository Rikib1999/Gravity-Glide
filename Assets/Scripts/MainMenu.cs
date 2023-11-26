using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void PlayLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }
}