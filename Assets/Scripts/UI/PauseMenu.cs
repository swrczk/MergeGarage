using UnityEngine;

// In-level pause with the settings panel
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _screen;

    private float timeScaleBeforePause = 1;

    private void Awake()
    {
        _screen.SetActive(false);
    }

    public void Open()
    {
        if (_screen.activeSelf)
            return;

        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
        _screen.SetActive(true);
    }

    public void Resume()
    {
        _screen.SetActive(false);
        Time.timeScale = timeScaleBeforePause;
    }

    public void ToMenu()
    {
        LevelProgress.LoadMenu();
    }
}