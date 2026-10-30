# IMBlocker

An IME blocker for *A Dance of Fire and Ice* (ADOFAI).

The mod keeps Chinese / Japanese / Korean IME shortcuts from leaking into gameplay and restores the IME automatically while a text field is focused.

## Layout

- `core/Patches.cs` — Harmony patches (currently restores `LAlt` / `RAlt` filtering for the async keyboard path).
- `core/ImeBlocker.cs` — per-frame IME state management (`ImmAssociateContext` + `Input.imeCompositionMode`).
- `core/LmSettingsUI.cs` — settings page built with the default UMM/IMGUI controls.
- `core/Main.cs` — loader-independent initialization, enable/disable lifecycle.
- `loaders/` — UMM / MelonLoader / BepInEx adapters.

## Supported loaders

UnityModManager, MelonLoader, BepInEx

## Supported game version

ADOFAI 3.4.0+ (including the current 3.4.0 beta async-input build).

## Build

Requires the game path in `.env` (see `.env.example`):

```cmd
dotnet build -c Release
```

Deploy + launch with a specific loader:

```cmd
dotnet build -c Release -p:Loader=UMM
```

Package `out/` into `dist/`:

```cmd
scripts\pack.cmd
```
