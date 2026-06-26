using System;

using ThemeSwitch.Runtime.Data;

namespace ThemeSwitch.Runtime.Core
{
    public static class ThemeEvents
    {
        public static Action<ThemeData>
            OnThemeChanged;
    }
}