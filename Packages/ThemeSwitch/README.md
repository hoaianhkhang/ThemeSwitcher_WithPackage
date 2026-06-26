Theme Switch (local UPM package)

Overview
- Lightweight runtime theme switching system.
- Designed as a local UPM package (Packages/ThemeSwitch).
- Keeps resources in-project; no DLC or external downloads.
- Each theme asset carries an array of background sprites, and the mood-driven background component can apply that full sprite sequence for the active theme.

Integration
- The package is already placed under `Packages/ThemeSwitch` and referenced by the project.
- To use, add a `ThemeRegistry` asset (Create -> Theme Switch -> Theme Registry) and assign themes.
- Each `ThemeData` asset holds a `BackgroundSprites` array for its visual sequence.
- Add `ThemeManager` to a GameObject and reference the registry.
- `ThemeManager` applies the initial theme on startup and broadcasts theme changes through `ThemeEvents.OnThemeChanged`.

Runtime flow
- `ThemeManager` stores the current theme index, switches to the next theme with `SwitchTheme()`, and calls `ApplyTheme(int)` to publish the selected `ThemeData`.
- `ThemeBGMood` subscribes to theme events and forwards the incoming `BackgroundSprites` to `BGMood.ApplySprites(...)` so the background can update immediately.

Runtime settings
- Create a `ThemeSwitchSettings` asset (Create -> Theme Switch -> Settings) and place it in a `Resources` folder at runtime (e.g. `Assets/Resources/ThemeSwitchSettings.asset`).
- Settings fields:
  - `Enabled`: turn the whole system on/off at runtime.
  - `DisabledPlatforms`: list of `RuntimePlatform` entries to automatically disable the system.

Performance
- Theme switching is synchronous and lightweight—events are published immediately to all subscribers.
- The event-based architecture ensures minimal overhead with no frame spikes.

Extensibility & Maintenance
- Implement `IThemeable` on any component to receive `ThemeData` updates.
- The system is intentionally small and focused; adding support for new themeable types is done by implementing `IThemeable`.
- For DLC or remote content, extend `ThemeRegistry` and resource loaders — current package keeps resources local.

Disabling for builds
- Use `ThemeSwitchSettings` to disable the system on specific platforms at runtime.
- For compile-time removal, you can wrap usage in your own code with `#if` checks or create a stripped assembly definition in downstream projects.

Notes
- Keep `ThemeData` assets and sprites in-project for best runtime performance.
- Creating a `ThemeSwitchSettings` in `Resources` is optional; default behavior is enabled globally.

Contact
- Author: Hoai Anh
