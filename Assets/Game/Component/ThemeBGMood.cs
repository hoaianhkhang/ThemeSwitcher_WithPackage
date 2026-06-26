using UnityEngine;

using ThemeSwitch.Runtime.Core;
using ThemeSwitch.Runtime.Data;
using ThemeSwitch.Runtime.Interfaces;

public class ThemeBGMood :
    MonoBehaviour,
    IThemeable
{
    [SerializeField]
    private BGMood _bgMood;

    [SerializeField]
    private ThemeManager _themeManager;

    private void OnEnable()
    {
        ThemeEvents.OnThemeChanged +=
            ApplyTheme;
    }

    private void OnDisable()
    {
        ThemeEvents.OnThemeChanged -=
            ApplyTheme;
    }

    public void ApplyTheme(ThemeData theme)
    {
        if (_bgMood == null || theme == null)
            return;

        _bgMood.ApplySprites(theme.BackgroundSprites);
    }
}