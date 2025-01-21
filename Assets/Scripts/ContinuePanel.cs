using Destructible;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField] private GameObject _continuePanel;
    private PlayerController _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void Died()
    {
        _continuePanel.SetActive(false);
        Time.timeScale = 0;
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        StartCoroutine(_player.Shielded(3f));
        Time.timeScale = 1;
        _continuePanel.SetActive(false);
    }
}