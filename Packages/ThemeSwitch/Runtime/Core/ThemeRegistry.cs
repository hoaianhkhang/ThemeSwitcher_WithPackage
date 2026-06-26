using UnityEngine;
using ThemeSwitch.Runtime.Data;

namespace ThemeSwitch.Runtime.Core
{
    [CreateAssetMenu(
        menuName = "Theme Switch/Theme Registry")]
    public class ThemeRegistry :
        ScriptableObject
    {
        public ThemeData[] Themes;
    }
}