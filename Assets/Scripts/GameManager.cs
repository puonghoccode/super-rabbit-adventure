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
    public int stars { get; private set; } = 0;
    public int maxLives => maxLivesPerLevel;
    [SerializeField] private string endMenuScene = "EndMenu";
    [SerializeField] private int maxLivesPerLevel = 1;
    [SerializeField] private string gameplayUIScene = "GameplayUI";
    [SerializeField] private bool useLoadingScene = false;
    [SerializeField] private string loadingScene = "Loading";
    [SerializeField] private bool allowContinue = true;
    [SerializeField] private string continueScene = "ContinueScene";
    [SerializeField] private float continueInvincibilitySeconds = 5f;

    private bool lastDeathWasFall;
    private Vector3 lastDeathPosition;
    private bool continueOpen;
    private float previousTimeScale = 1f;

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
        stars = 0;
    }

    public void GameOver()
    {
        EndMenuData.RecordGameOver(coins, stars);

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
            coins = 0;
            stars = 0;
        }

        string targetScene = $"{world}-{stage}";

        if (useLoadingScene && !string.IsNullOrEmpty(loadingScene))
        {
            if (SceneManager.GetActiveScene().name != loadingScene)
            {
                LoadingScreenData.SetTarget(targetScene);
                SceneManager.LoadScene(loadingScene);
                return;
            }
        }

        SceneManager.LoadScene(targetScene);
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
        if (allowContinue && !lastDeathWasFall) {
            OpenContinueMenu();
        } else {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;
    }

    public void AddStar()
    {
        int maxStars = LevelProgress.GetMaxStars(world, stage);
        stars = Mathf.Clamp(stars + 1, 0, maxStars);
    }

    public void AddLife()
    {
        lives = Mathf.Min(lives + 1, maxLivesPerLevel);
    }

    public void RegisterDeath(bool isFallDeath, Vector3 deathPosition)
    {
        lastDeathWasFall = isFallDeath;
        lastDeathPosition = deathPosition;
    }

    public bool TryContinueWithCoins(int cost)
    {
        if (!continueOpen)
        {
            return false;
        }

        if (!PlayerWallet.TrySpendCoins(cost))
        {
            return false;
        }

        ContinueFromPurchase();
        return true;
    }

    public void ContinueWithAd()
    {
        if (!continueOpen)
        {
            return;
        }

        ContinueFromPurchase();
    }

    public void SkipContinue()
    {
        if (continueOpen)
        {
            CloseContinueMenu();
        }

        GameOver();
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

    private void OpenContinueMenu()
    {
        if (continueOpen)
        {
            return;
        }

        if (string.IsNullOrEmpty(continueScene))
        {
            GameOver();
            return;
        }

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(continueScene, LoadSceneMode.Additive);
        continueOpen = true;
    }

    private void CloseContinueMenu()
    {
        if (!continueOpen)
        {
            return;
        }

        Time.timeScale = previousTimeScale;

        if (!string.IsNullOrEmpty(continueScene))
        {
            Scene scene = SceneManager.GetSceneByName(continueScene);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(continueScene);
            }
        }

        continueOpen = false;
    }

    private void ContinueFromPurchase()
    {
        lives = 1;

        CloseContinueMenu();
        RespawnPlayer(lastDeathPosition, continueInvincibilitySeconds);
    }

    private void RespawnPlayer(Vector3 position, float invincibilitySeconds)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            return;
        }

        playerObject.transform.position = position;

        if (playerObject.TryGetComponent(out Player player))
        {
            player.Revive(position, invincibilitySeconds);
        }
    }

}
