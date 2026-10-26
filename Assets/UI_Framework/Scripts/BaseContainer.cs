using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseContainer : CustomUIComponent
    {
        public ViewSo _viewData;

        public Style _style;

        private Image image;

        public override void Setup()
        {
            image = GetComponent<Image>();
        }

        public override void Configure()
        {
            image.color = _viewData._theme.GetBgColor(_style);
        }
    }
}