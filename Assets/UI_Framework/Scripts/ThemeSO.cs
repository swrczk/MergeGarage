using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "Theme", menuName = "CustomUI/ThemeSO")]
    public class ThemeSo : ScriptableObject
    {
        [Header("Primary")] public Color _primaryBg;

        public Color _primaryText;

        [Header("Secondary")] public Color _secondaryBg;

        public Color _secondaryText;

        [Header("Tertiary")] public Color _tertiaryBg;

        public Color _tertiaryText;

        [Header("Other")] public Color _disabledBg;

        public Color _disabledText;

        public Color GetTextColor(Style style)
        {
            switch (style)
            {
                case Style.Primary:
                    return _primaryText;
                case Style.Secondary:
                    return _secondaryText;
                case Style.Tertiary:
                    return _tertiaryText;
                default:
                    return _disabledBg;
            }
        }

        public Color GetBgColor(Style style = Style.Disabled)
        {
            switch (style)
            {
                case Style.Primary:
                    return _primaryBg;
                case Style.Secondary:
                    return _secondaryBg;
                case Style.Tertiary:
                    return _tertiaryBg;
                default:
                    return _disabledText;
            }
        }
    }
}