using TMPro;
using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "TextSO", menuName = "CustomUI/TextSO")]
    public class TextSO : ScriptableObject
    {
        public ThemeSO theme;
    
        public TMP_FontAsset font;
        public float fontSize;
    }
}
