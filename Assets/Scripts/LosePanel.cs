using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 0;
    }

    public void PouseLevel()
    {
        Time.timeScale = 0;
    }

    public void ContinueLevel()
    {
        Time.timeScale = 1;
    }
}
