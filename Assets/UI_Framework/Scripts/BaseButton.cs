using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseButton : CustomUIComponent
    {
        public ThemeSo _theme;
        public Style _style;
        public UnityEvent _onClick;

        private Button button;
        private TextMeshProUGUI buttonText;

        public override void Setup()
        {
            button = GetComponentInChildren<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Configure()
        {
            var colorBlock = button.colors;
            colorBlock.normalColor = _theme.GetBgColor(_style);
            button.colors = colorBlock;

            buttonText.color = _theme.GetTextColor(_style);
        }

        public void OnClick()
        {
            _onClick.Invoke();
        }
    }
}