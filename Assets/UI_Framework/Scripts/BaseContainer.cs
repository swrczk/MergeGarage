using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseContainer : CustomUIComponent
    {
        public ViewSO viewData;

        private Image image;

        public Style style;

        public override void Setup()
        {
            image = GetComponent<Image>();
        }

        public override void Configure()
        {
            image.color = viewData.theme.GetBgColor(style);
        }
    }
}