using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Binds the settings controls to GameSettings. Used in the menu and the pause screen.
public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider _music;

    [SerializeField] private TMP_Text _musicValue;

    [SerializeField] private Slider _sfx;

    [SerializeField] private TMP_Text _sfxValue;

    [SerializeField] private Toggle _vibration;

    [SerializeField] private Toggle _fullscreen;

    private void Awake()
    {
        _music.onValueChanged.AddListener(v =>
        {
            GameSettings.MusicVolume = v;
            _musicValue.text = Percent(v);
        });
        _sfx.onValueChanged.AddListener(v =>
        {
            GameSettings.SfxVolume = v;
            _sfxValue.text = Percent(v);
        });
        _vibration.onValueChanged.AddListener(v => GameSettings.Vibration = v);
        _fullscreen.onValueChanged.AddListener(v => GameSettings.Fullscreen = v);
    }

    private void OnEnable()
    {
        _music.SetValueWithoutNotify(GameSettings.MusicVolume);
        _musicValue.text = Percent(_music.value);
        _sfx.SetValueWithoutNotify(GameSettings.SfxVolume);
        _sfxValue.text = Percent(_sfx.value);
        _vibration.SetIsOnWithoutNotify(GameSettings.Vibration);
        _fullscreen.SetIsOnWithoutNotify(GameSettings.Fullscreen);

        // Vibration only exists on phones, fullscreen only on desktop/web
        _vibration.transform.parent.gameObject.SetActive(Application.isMobilePlatform);
        _fullscreen.transform.parent.gameObject.SetActive(!Application.isMobilePlatform);

        _vibration.GetComponent<SwitchToggle>().Refresh();
        _fullscreen.GetComponent<SwitchToggle>().Refresh();
    }

    private static string Percent(float v)
    {
        return Mathf.RoundToInt(v * 100) + "%";
    }
}