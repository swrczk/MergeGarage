using UnityEngine;
using UnityEngine.UI;

// Pill-shaped on/off switch driven by a Toggle
[RequireComponent(typeof(Toggle))]
public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private Image _background;

    [SerializeField] private RectTransform _knob;

    [SerializeField] private Color _onColor = new(0.47f, 0.75f, 1f);

    [SerializeField] private Color _offColor = new(0.16f, 0.27f, 0.43f);

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(_ => Refresh());
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        var on = toggle.isOn;
        _background.color = on ? _onColor : _offColor;
        var x = on ? 1f : 0f;
        _knob.anchorMin = new Vector2(x, 0.5f);
        _knob.anchorMax = new Vector2(x, 0.5f);
        _knob.pivot = new Vector2(x, 0.5f);
        _knob.anchoredPosition = new Vector2(on ? -4 : 4, 0);
    }
}