using UnityEngine.UI;

namespace UI_Framework.Scripts
{
    public class BaseView : CustomUIComponent
    {
        public ViewSO viewData;

        private VerticalLayoutGroup verticalLayoutGroup; 
        public override void Setup()
        {
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        public override void Configure()
        {
            verticalLayoutGroup.padding = viewData.padding;
            verticalLayoutGroup.spacing = viewData.spacing;
        }
    }
}