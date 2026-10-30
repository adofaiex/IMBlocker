# IMBlocker 0.1.7

Stops Chinese / Japanese / Korean IME shortcuts from interfering with *A Dance of Fire and Ice* gameplay, while keeping IME input available when you actually need to type.

## What it does

- **While playing** — disables the Windows IME for the game window, so IME shortcuts and candidate windows no longer steal keystrokes.
- **While editing text** — detects focused `TMP_InputField` / `InputField` controls and restores the IME automatically.
- **Async input fix** — the current beta removed `LAlt` from the async keyboard's special-key list. IMBlocker re-adds `LAlt` / `RAlt` so switching IME or using Alt-based IME shortcuts does not count as a gameplay hit.

## Settings

Open the mod settings page in your loader (UMM: **Mods → IMBlocker → Settings**):

- **Enable IMBlocker** — turn the mod on/off without restarting.
- **Block IME outside text fields** — the main feature; leave this on unless another IME tool conflicts with it.
- **Language** — English / 简体中文, or follow the game language.

## Requirements

- Windows
- ADOFAI with UMM, MelonLoader, or BepInEx
- Works with both the legacy keyboard path and the async (SkyHook) keyboard path

## Notes

IMBlocker uses the Windows IMM32 context for the game window and also sets Unity's `Input.imeCompositionMode`. If your IME uses a custom overlay that bypasses IMM32 (some third-party IME skins do), the IME may still appear; the Alt-key patch will still keep those keys out of gameplay input.
