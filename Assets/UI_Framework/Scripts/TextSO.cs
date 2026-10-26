using TMPro;
using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "TextSO", menuName = "CustomUI/TextSO")]
    public class TextSo : ScriptableObject
    {
        public ThemeSo _theme;

        public TMP_FontAsset _font;
        public float _fontSize;
    }
}