using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private LevelSelectItem itemPrefab;
    [SerializeField] private MenuClickSound clickSound;
    [SerializeField] private bool showWorldStageLabel = true;

    private void Start()
    {
        BuildList();
    }

    public void BuildList()
    {
        if (contentRoot == null || itemPrefab == null)
        {
            return;
        }

        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }

        var levels = LevelProgress.GetLevels();
        for (int i = 0; i < levels.Count; i++)
        {
            LevelProgress.LevelInfo info = levels[i];
            bool unlocked = LevelProgress.IsUnlocked(info.world, info.stage);
            int stars = LevelProgress.GetStars(info.world, info.stage);
            int maxStars = LevelProgress.GetMaxStars(info.world, info.stage);
            string label = showWorldStageLabel ? $"{info.world}-{info.stage}" : info.sceneName;

            LevelSelectItem item = Object.Instantiate(itemPrefab, contentRoot);
            item.Configure(label, stars, maxStars, unlocked);

            bool isUnlocked = unlocked;
            item.SetOnClick(() => OnLevelSelected(info, isUnlocked));
        }
    }

    private void OnLevelSelected(LevelProgress.LevelInfo info, bool unlocked)
    {
        if (!unlocked)
        {
            return;
        }

        if (clickSound != null)
        {
            clickSound.Play();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(info.world, info.stage, true);
        }
        else
        {
            SceneManager.LoadScene(info.sceneName);
        }
    }
}
