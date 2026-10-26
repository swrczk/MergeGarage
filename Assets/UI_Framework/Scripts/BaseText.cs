using TMPro;

namespace UI_Framework.Scripts
{
    public class BaseText : CustomUIComponent
    {
        public TextSo _textData;
        public Style _style;

        private TextMeshProUGUI text;

        public override void Setup()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Configure()
        {
            text.font = _textData._font;
            text.fontSize = _textData._fontSize;
            text.color = _textData._theme.GetTextColor(_style);
        }
    }
}