# Theme Switch Package Architecture

## Overview
The `ThemeSwitch` package is designed as a reusable local Unity Package Manager (UPM) package. It provides a runtime theme switching system with a small core API, asset-driven theme configuration, and optional runtime behavior control.

## Core Components

- `ThemeManager` (`Runtime/Core/ThemeManager.cs`)
  - Central controller for switching themes.
  - References a `ThemeRegistry` asset containing all available themes.
  - Applies the initial theme on startup and keeps track of the current theme index.
  - Publishes theme changes through `ThemeEvents.OnThemeChanged` when `SwitchTheme()` or `ApplyTheme(int)` is called.
  - Exposes helpers such as `GetCurrentTheme()` and `GetAllThemeSprites()` for consumers.

- `ThemeRegistry` (`Runtime/Core/ThemeRegistry.cs`)
  - ScriptableObject asset that stores `ThemeData[]`.
  - Keeps all in-project theme assets in one place.

- `ThemeData` (`Runtime/Data/ThemeData.cs`)
  - ScriptableObject describing a theme.
  - Contains a `Sprite[] BackgroundSprites` collection for the theme's visual sequence.
  - These sprites are consumed by mood-driven background components such as `ThemeBGMood`.
  - The package remains intentionally simple, while allowing each theme asset to supply multiple visual frames for richer transitions.

- `IThemeable` (`Runtime/Interfaces/IThemeable.cs`)
  - Interface for components that apply theme updates.
  - Enables any component to subscribe to theme updates through `ThemeEvents`.

- `ThemeBGMood` (`../Assets/Game/Component/ThemeBGMood.cs`)
  - Example consumer that listens for theme-change events.
  - Receives the new `ThemeData` and forwards its `BackgroundSprites` array into the `BGMood` component for playback.

- `ThemeEvents` (`Runtime/Core/ThemeEvents.cs`)
  - Static event broadcaster for runtime theme changes.
  - Keeps the runtime API decoupled from consumers.

## Runtime Configuration

- `ThemeSwitchSettings` (`Runtime/Core/ThemeSwitchSettings.cs`)
  - ScriptableObject asset used to configure runtime behavior.
  - Can disable the theme system globally or on specific platforms.

- `ThemeSwitchConfig` (`Runtime/Core/ThemeSwitchConfig.cs`)
  - Static runtime loader that reads `ThemeSwitchSettings` from `Resources/ThemeSwitchSettings`.
  - Provides global enable/disable state.
  - Ensures default behavior remains enabled if no settings asset is present.

## Design Decisions

### Local UPM Package
- The package is placed under `Packages/ThemeSwitch` to support Unity's local package workflow.
- This minimizes integration cost for legacy projects and avoids requiring external downloads or online package sources.
- The package manifest is intentionally small and focused on runtime content only.

### ScriptableObjects for Data & Settings
- `ThemeData` and `ThemeRegistry` are `ScriptableObject` assets, making themes easy to author and manage in the Unity editor.
- `ThemeSwitchSettings` is also a `ScriptableObject` so runtime toggles and platform-specific settings are editable without code changes.
- Keeping data in assets makes serialization and reuse simple across projects.

### Event-based Decoupling
- `ThemeManager` only knows about the theme registry and broadcasts changes.
- Components implement `IThemeable` and listen for `ThemeEvents.OnThemeChanged` to apply theme-specific updates.
- The current example flow is: `ThemeManager` publishes a `ThemeData`, `ThemeBGMood` receives it, and the attached `BGMood` component applies the theme's sprite sequence.
- This design supports extensibility: any new themable component can be added without changing the core package.

### Performance and Frame Stability
- Theme switching is synchronous and lightweight—there are no coroutines or deferred updates.
- Events are published immediately to all subscribers, keeping the flow simple and predictable.

## Trade-offs

### Simplicity vs. Flexibility
- The package favors a lightweight core API over a fully general theme engine.
- Themes are represented as a simple `ThemeData` object with a sprite-array field; adding new themeable content types requires extending `ThemeData` and implementing corresponding consumers.
- This keeps the package maintainable and predictable, but advanced features (nested themes, runtime asset bundles, or DLC) are intentionally left out.

### Runtime Toggle vs. Compile-time Stripping
- `ThemeSwitchSettings` provides runtime disabling, including platform-specific disable options.
- Compile-time removal is not built into the package itself; downstream projects may optionally use conditional compilation or stripped assembly definitions if needed.
- This trade-off reduces package complexity while still supporting most use cases.

### In-Project Resources Only
- The package explicitly avoids DLC or remote content support.
- All assets are expected to be authored inside the project, which simplifies packaging and ensures predictable performance.
- It also means remote delivery or downloadable themes are not part of the current architecture.

## Extensibility

- Add new themeable component types by implementing `IThemeable` and subscribing to `ThemeEvents.OnThemeChanged`.
- Extend `ThemeData` with new fields for additional theme assets.
- Use `ThemeRegistry` to group and version theme sets.

## Integration Notes

- To integrate into another project, copy `Packages/ThemeSwitch` and add it to `Packages/manifest.json` as a local path package, or simply keep the package folder under `Packages`.
- Create `ThemeRegistry` and `ThemeData` assets to define available themes.
- Create a `ThemeSwitchSettings` asset under `Assets/Resources/ThemeSwitchSettings.asset` when runtime control is desired.
- Attach `ThemeManager` to a GameObject and hook up the registry to drive theme switching.

## Build/Platform Control

- `ThemeSwitchSettings.DisabledPlatforms` allows disabling the system automatically on selected runtime platforms.
- This supports the requirement to turn the whole system off for specific platforms or build targets without changing code.

## Summary
The package is designed as a small, synchronous theme switching system with minimal integration cost, local resources only, and runtime options for platform control. It is intentionally extensible through interface-based theme consumers and asset-driven theme definitions, with a focus on simplicity and predictability.
