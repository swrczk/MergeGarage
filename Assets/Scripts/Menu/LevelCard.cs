using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One entry on the level select screen
public class LevelCard : MonoBehaviour
{
    [SerializeField] private int _level;

    [SerializeField] private Button _playButton;

    [SerializeField] private GameObject _lockIcon;

    [SerializeField] private GameObject _doneBadge;

    [SerializeField] private TMP_Text[] _texts;

    [SerializeField] private Color _lockedTextColor = new(0.59f, 0.65f, 0.73f);

    private Color[] textColors;

    private void Awake()
    {
        _playButton.onClick.AddListener(() => LevelProgress.LoadLevel(_level));
    }

    public void Refresh()
    {
        if (textColors == null)
        {
            textColors = new Color[_texts.Length];
            for (var i = 0; i < _texts.Length; i++)
                textColors[i] = _texts[i].color;
        }

        var unlocked = LevelProgress.IsUnlocked(_level);
        _playButton.gameObject.SetActive(unlocked);
        _lockIcon.SetActive(!unlocked);
        _doneBadge.SetActive(LevelProgress.IsCompleted(_level));
        for (var i = 0; i < _texts.Length; i++)
            _texts[i].color = unlocked ? textColors[i] : _lockedTextColor;
    }
}