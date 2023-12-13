using UnityEngine;

namespace UI_Framework.Scripts
{
    [CreateAssetMenu(fileName = "Theme", menuName = "CustomUI/ThemeSO")]
    public class ThemeSO : ScriptableObject
    {
        [Header("Primary")]
        public Color primaryBg;

        public Color primaryText;

        [Header("Secondary")]
        public Color secondaryBg;

        public Color secondaryText;

        [Header("Tertiary")]
        public Color tertiaryBg;

        public Color tertiaryText;

        [Header("Other")]
        public Color disabledBg;
        public Color disabledText;

        public Color GetTextColor(Style style)
        {
            switch (style)
            {
                case Style.Primary:
                    return primaryText;
                case Style.Secondary:
                    return secondaryText;
                case Style.Tertiary:
                    return tertiaryText;
                default:
                    return disabledBg;
            }
        }

        public Color GetBgColor(Style style = Style.Disabled)
        {
            switch (style)
            {
                case Style.Primary:
                    return primaryBg;
                case Style.Secondary:
                    return secondaryBg;
                case Style.Tertiary:
                    return tertiaryBg;
                default:
                    return disabledText;
            }
        }
    }
}