using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelProgress
{
    public struct LevelInfo
    {
        public string sceneName;
        public int world;
        public int stage;
    }

    private const string StarsKeyPrefix = "level_stars_";
    private const string HighestUnlockedKey = "level_highest_unlocked";
    private static List<LevelInfo> cachedLevels;

    public static IReadOnlyList<LevelInfo> GetLevels()
    {
        if (cachedLevels == null)
        {
            BuildLevelList();
        }

        return cachedLevels;
    }

    public static int GetStars(int world, int stage)
    {
        return PlayerPrefs.GetInt(StarsKey(world, stage), 0);
    }

    public static bool IsUnlocked(int world, int stage)
    {
        int index = GetLevelIndex(world, stage);
        if (index < 0)
        {
            return false;
        }

        return index <= GetHighestUnlockedIndex();
    }

    public static void RecordCompletion(int world, int stage, int stars)
    {
        int clampedStars = Mathf.Clamp(stars, 0, 3);
        string key = StarsKey(world, stage);
        int previousStars = PlayerPrefs.GetInt(key, 0);
        if (clampedStars > previousStars)
        {
            PlayerPrefs.SetInt(key, clampedStars);
        }

        int index = GetLevelIndex(world, stage);
        if (index >= 0)
        {
            int nextIndex = index + 1;
            int highestUnlocked = PlayerPrefs.GetInt(HighestUnlockedKey, 0);
            if (nextIndex > highestUnlocked)
            {
                PlayerPrefs.SetInt(HighestUnlockedKey, nextIndex);
            }
        }

        PlayerPrefs.Save();
    }

    public static int GetMaxStars(int world, int stage)
    {
        return (world == 1 && stage == 1) ? 1 : 3;
    }

    private static void BuildLevelList()
    {
        cachedLevels = new List<LevelInfo>();
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(path);

            if (TryParseLevelName(sceneName, out int world, out int stage))
            {
                cachedLevels.Add(new LevelInfo
                {
                    sceneName = sceneName,
                    world = world,
                    stage = stage
                });
            }
        }

        cachedLevels.Sort((a, b) =>
        {
            int worldCompare = a.world.CompareTo(b.world);
            return worldCompare != 0 ? worldCompare : a.stage.CompareTo(b.stage);
        });
    }

    private static bool TryParseLevelName(string sceneName, out int world, out int stage)
    {
        world = 0;
        stage = 0;

        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        string[] parts = sceneName.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        return int.TryParse(parts[0], out world) && int.TryParse(parts[1], out stage);
    }

    private static int GetLevelIndex(int world, int stage)
    {
        IReadOnlyList<LevelInfo> levels = GetLevels();
        for (int i = 0; i < levels.Count; i++)
        {
            LevelInfo info = levels[i];
            if (info.world == world && info.stage == stage)
            {
                return i;
            }
        }

        return -1;
    }

    private static int GetHighestUnlockedIndex()
    {
        int highest = PlayerPrefs.GetInt(HighestUnlockedKey, 0);
        int maxIndex = Mathf.Max(0, GetLevels().Count - 1);
        return Mathf.Clamp(highest, 0, maxIndex);
    }

    private static string StarsKey(int world, int stage)
    {
        return $"{StarsKeyPrefix}{world}_{stage}";
    }
}
