using System;
using System.Collections.Generic;
using UnityEngine;
using ThemeSwitch.Runtime.Data;

namespace ThemeSwitch.Runtime.Core
{
    public class ThemeManager : MonoBehaviour
    {
        [SerializeField]
        private ThemeRegistry _registry;

        private int _currentThemeIndex;

        private void Start()
        {
            ApplyTheme(_currentThemeIndex);
        }
        
        public ThemeData GetCurrentTheme()
        {
            if (_registry == null || _registry.Themes == null || _registry.Themes.Length == 0)
                return null;

            if (_currentThemeIndex < 0 || _currentThemeIndex >= _registry.Themes.Length)
                _currentThemeIndex = 0;

            return _registry.Themes[_currentThemeIndex];
        }

        public Sprite[] GetAllThemeSprites()
        {
            if (_registry == null || _registry.Themes == null)
                return Array.Empty<Sprite>();

            var spriteList = new List<Sprite>();
            foreach (var theme in _registry.Themes)
            {
                if (theme?.BackgroundSprites == null)
                    continue;

                spriteList.AddRange(theme.BackgroundSprites);
            }

            return spriteList.ToArray();
        }

        public void SwitchTheme()
        {
            _currentThemeIndex =
                (_currentThemeIndex + 1) %
                _registry.Themes.Length;

            ApplyTheme(_currentThemeIndex);
        }

            public void ApplyTheme(int index)
        {
            _currentThemeIndex = index;

            ThemeEvents.OnThemeChanged?.Invoke(
                _registry.Themes[index]);
        }
    }
}