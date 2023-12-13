using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseButton : CustomUIComponent
    {
        public ThemeSO theme;
        public Style style;
        public UnityEvent onClick;
        
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
            colorBlock.normalColor = theme.GetBgColor(style);
            button.colors = colorBlock;
            
            buttonText.color = theme.GetTextColor(style);
        }
        
        public void OnClick()
        {
            onClick.Invoke();
        }
    }
}
