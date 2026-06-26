using UnityEngine;

namespace ThemeSwitch.Runtime.Core
{
    [CreateAssetMenu(menuName = "Theme Switch/Settings")]
    public class ThemeSwitchSettings : ScriptableObject
    {
        [Header("Runtime toggles")]
        public bool Enabled = true;

        [Header("Disable on these platforms (runtime)")]
        public RuntimePlatform[] DisabledPlatforms;
    }
}