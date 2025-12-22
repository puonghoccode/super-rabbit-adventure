using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;
    public int maxLives => maxLivesPerLevel;
    [SerializeField] private string endMenuScene = "EndMenu";
    [SerializeField] private int maxLivesPerLevel = 3;
    [SerializeField] private string gameplayUIScene = "GameplayUI";

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }

    public void NewGame()
    {
        ResetStats();
        LoadLevel(1, 1, true);
    }

    public void ResetStats()
    {
        lives = maxLivesPerLevel;
        coins = 0;
    }

    public void GameOver()
    {
        EndMenuData.RecordGameOver(coins);

        if (!string.IsNullOrEmpty(endMenuScene))
        {
            SceneManager.LoadScene(endMenuScene);
        }
    }

    public void LoadLevel(int world, int stage)
    {
        LoadLevel(world, stage, world != this.world || stage != this.stage);
    }

    public void LoadLevel(int world, int stage, bool resetLives)
    {
        this.world = world;
        this.stage = stage;

        if (resetLives)
        {
            lives = maxLivesPerLevel;
        }

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1, true);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0) {
            LoadLevel(world, stage, false);
        } else {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives = Mathf.Min(lives + 1, maxLivesPerLevel);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplayUIScene)
        {
            return;
        }

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == gameplayUIScene)
        {
            return;
        }

        if (IsGameplayScene(activeScene.name))
        {
            LoadGameplayUI();
        }
        else
        {
            UnloadGameplayUI();
        }
    }

    private void LoadGameplayUI()
    {
        if (string.IsNullOrEmpty(gameplayUIScene))
        {
            return;
        }

        Scene uiScene = SceneManager.GetSceneByName(gameplayUIScene);
        if (!uiScene.isLoaded)
        {
            SceneManager.LoadSceneAsync(gameplayUIScene, LoadSceneMode.Additive);
        }
    }

    private void UnloadGameplayUI()
    {
        if (string.IsNullOrEmpty(gameplayUIScene))
        {
            return;
        }

        Scene uiScene = SceneManager.GetSceneByName(gameplayUIScene);
        if (uiScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameplayUIScene);
        }
    }

    private bool IsGameplayScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        string[] parts = sceneName.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        return int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
    }

}
