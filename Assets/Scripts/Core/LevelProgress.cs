using UnityEngine;
using UnityEngine.SceneManagement;

// Which levels are completed/unlocked. Levels are numbered from 1;
// the menu is build index 0 and level N is build index N.
public static class LevelProgress
{
    private const int MenuBuildIndex = 0;
    private const string CompletedKey = "Progress.LevelCompleted.";

    public static int LevelCount => SceneManager.sceneCountInBuildSettings - 1;

    public static int CurrentLevel => SceneManager.GetActiveScene().buildIndex;

    public static int CompletedCount
    {
        get
        {
            var count = 0;
            for (var level = 1; level <= LevelCount; level++)
                if (IsCompleted(level))
                    count++;
            return count;
        }
    }

    // First level not yet completed, or the last one when everything is done
    public static int NextLevelToPlay
    {
        get
        {
            for (var level = 1; level <= LevelCount; level++)
                if (!IsCompleted(level))
                    return level;
            return LevelCount;
        }
    }

    public static bool IsCompleted(int level)
    {
        return PlayerPrefs.GetInt(CompletedKey + level, 0) == 1;
    }

    public static bool IsUnlocked(int level)
    {
        return level <= 1 || IsCompleted(level - 1);
    }

    public static void MarkCompleted(int level)
    {
        PlayerPrefs.SetInt(CompletedKey + level, 1);
        PlayerPrefs.Save();
    }

    public static void LoadLevel(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }

    public static void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MenuBuildIndex);
    }
}