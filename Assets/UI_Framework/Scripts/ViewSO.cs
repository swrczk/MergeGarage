using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "ViewSO", menuName = "CustomUI/ViewSO")]
    public class ViewSo : ScriptableObject
    {
        public ThemeSo _theme;

        public RectOffset _padding;
        public float _spacing;
    }
}