using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "ViewSO", menuName = "CustomUI/ViewSO")]
    public class ViewSO : ScriptableObject
    {
        public ThemeSO theme;
    
        public RectOffset padding;
        public float spacing;
    }
}
