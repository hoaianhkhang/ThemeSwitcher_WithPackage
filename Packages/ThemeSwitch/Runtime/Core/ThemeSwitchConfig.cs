using UnityEngine;

namespace ThemeSwitch.Runtime.Core
{
    public static class ThemeSwitchConfig
    {
        public static bool Enabled { get; private set; } = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            var settings = Resources.Load<ThemeSwitchSettings>("ThemeSwitchSettings");
            if (settings != null)
            {
                Enabled = settings.Enabled;

                var platform = Application.platform;
                if (settings.DisabledPlatforms != null)
                {
                    foreach (var p in settings.DisabledPlatforms)
                    {
                        if (p == platform)
                        {
                            Enabled = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}