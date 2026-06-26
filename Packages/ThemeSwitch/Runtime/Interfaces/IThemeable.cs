using ThemeSwitch.Runtime.Data;

namespace ThemeSwitch.Runtime.Interfaces
{
    public interface IThemeable
    {
        void ApplyTheme(
            ThemeData theme);
    }
}