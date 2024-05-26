using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField] private GameObject _continuePanel;

    public void Died()
    {
        _continuePanel.SetActive(false);
        Time.timeScale = 0;
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        _continuePanel.SetActive(false);
    }
}