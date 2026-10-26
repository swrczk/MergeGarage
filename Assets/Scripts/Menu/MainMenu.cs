using TMPro;
using UnityEngine;

// Start scene: play, level select and settings
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;

    [SerializeField] private GameObject _levelsScreen;

    [SerializeField] private GameObject _settingsScreen;

    [SerializeField] private LevelCard[] _levelCards;

    [SerializeField] private TMP_Text _progressText;

    private void Start()
    {
        Time.timeScale = 1;
        ShowMain();
    }

    public void Play()
    {
        LevelProgress.LoadLevel(LevelProgress.NextLevelToPlay);
    }

    public void ShowMain()
    {
        _mainPanel.SetActive(true);
        _levelsScreen.SetActive(false);
        _settingsScreen.SetActive(false);
        _progressText.text = $"{LevelProgress.CompletedCount} / {LevelProgress.LevelCount} levels completed";
    }

    public void ShowLevels()
    {
        foreach (var card in _levelCards)
            card.Refresh();
        _mainPanel.SetActive(false);
        _levelsScreen.SetActive(true);
    }

    public void ShowSettings()
    {
        _mainPanel.SetActive(false);
        _settingsScreen.SetActive(true);
    }
}