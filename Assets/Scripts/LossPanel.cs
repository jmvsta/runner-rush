using UnityEngine;
using UnityEngine.SceneManagement;

public class LossPanel : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 0;
    }

    public void PauseLevel()
    {
        Time.timeScale = 0;
    }

    public void ContinueLevel()
    {
        Time.timeScale = 1;
    }
}
