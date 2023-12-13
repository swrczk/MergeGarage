using TMPro;

namespace UI_Framework.Scripts
{
    public class BaseText : CustomUIComponent
    {
        public TextSO textData;
        public Style style;

        private TextMeshProUGUI text;

        public override void Setup()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Configure()
        {
            text.font = textData.font;
            text.fontSize = textData.fontSize;
            text.color = textData.theme.GetTextColor(style);
        }
    }
}