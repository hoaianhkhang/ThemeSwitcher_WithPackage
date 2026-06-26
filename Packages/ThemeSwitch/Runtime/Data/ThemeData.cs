using UnityEngine;

namespace ThemeSwitch.Runtime.Data
{
    [CreateAssetMenu(
        menuName = "Theme Switch/Theme Data")]
    public class ThemeData : ScriptableObject
    {
        public Sprite[] BackgroundSprites;
    }
}