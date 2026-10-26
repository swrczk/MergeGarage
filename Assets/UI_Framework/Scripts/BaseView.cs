using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseView : CustomUIComponent
    {
        public ViewSo _viewData;

        private VerticalLayoutGroup verticalLayoutGroup;

        public override void Setup()
        {
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        public override void Configure()
        {
            verticalLayoutGroup.padding = _viewData._padding;
            verticalLayoutGroup.spacing = _viewData._spacing;
        }
    }
}