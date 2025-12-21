using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIController : MonoBehaviour
{
    [SerializeField] private string homeSceneName;

    private int nextWorld;
    private int nextStage;
    private bool hasNextLevel;

    public void ConfigureNextLevel(int world, int stage)
    {
        nextWorld = world;
        nextStage = stage;
        hasNextLevel = true;
    }

    public void RestartLevel()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.LoadLevel(GameManager.Instance.world, GameManager.Instance.stage);
    }

    public void NextLevel()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (hasNextLevel)
        {
            GameManager.Instance.LoadLevel(nextWorld, nextStage);
        }
        else
        {
            GameManager.Instance.NextLevel();
        }
    }

    public void GoHome()
    {
        if (string.IsNullOrEmpty(homeSceneName))
        {
            return;
        }

        SceneManager.LoadScene(homeSceneName);
    }
}
