using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IMBlocker
{
    /// <summary>
    /// Disables the Windows IME while the game is not editing text, and restores
    /// it as soon as a text field is focused. The game also uses SkyHook raw
    /// keyboard events for asynchronous input, which bypass Unity's IME
    /// composition; <see cref="Patches"/> re-adds the Alt keys to that path.
    /// </summary>
    internal static class ImeBlocker
    {
        private static bool _blocked;
        private static bool _hasOriginalMode;
        private static IMECompositionMode _originalMode;

        private static IntPtr _window;
        private static IntPtr _savedContext;
        private static bool _hasSavedContext;

        public static void Initialize()
        {
            Restore();
            _window = IntPtr.Zero;
            _savedContext = IntPtr.Zero;
            _hasSavedContext = false;
            _hasOriginalMode = false;
            _blocked = false;
        }

        /// <summary>Called once per frame from the main thread.</summary>
        public static void Update()
        {
            if (!Main.Settings.Enabled || !Main.Settings.BlockIme)
            {
                Restore();
                return;
            }

            // Never touch another application's IME context while the game is
            // in the background.
            if (!Application.isFocused)
                return;

            if (IsTextInputFocused())
                EnableIme();
            else
                DisableIme();
        }

        public static void OnSettingChanged()
        {
            if (!Main.Settings.BlockIme)
                Restore();
        }

        public static void Reset()
        {
            Restore();
            _window = IntPtr.Zero;
            _savedContext = IntPtr.Zero;
            _hasSavedContext = false;
            _hasOriginalMode = false;
            _blocked = false;
        }

        /// <summary>Restore the IME to the state it had before we touched it.</summary>
        public static void Restore()
        {
            if (_blocked)
            {
                SetImeMode(_hasOriginalMode ? _originalMode : IMECompositionMode.Auto);
                RestoreContext();
                _blocked = false;
            }
            else if (_hasOriginalMode)
            {
                SetImeMode(_originalMode);
                _hasOriginalMode = false;
            }
        }

        private static void DisableIme()
        {
            if (_blocked)
            {
                SetImeMode(IMECompositionMode.Off);
                return;
            }

            if (!_hasOriginalMode)
            {
                _originalMode = Input.imeCompositionMode;
                _hasOriginalMode = true;
            }

            // Remove the IMM context first: Unity's own Off switch clears it,
            // which would make it impossible to save the original HIMC.
            DisableContext();
            SetImeMode(IMECompositionMode.Off);
            _blocked = true;
        }

        private static void EnableIme()
        {
            if (!_blocked)
                return;

            SetImeMode(IMECompositionMode.On);
            RestoreContext();
            _blocked = false;
        }

        private static void SetImeMode(IMECompositionMode mode)
        {
            try
            {
                if (Input.imeCompositionMode != mode)
                    Input.imeCompositionMode = mode;
            }
            catch (Exception ex)
            {
                Main.Handler?.Warning($"[IMBlocker][IME] failed to set {mode}: {ex.Message}");
            }
        }

        private static bool IsTextInputFocused()
        {
            // IMGUI text fields (UMM's own settings/search boxes, debug consoles)
            // keep keyboard focus through GUIUtility rather than EventSystem.
            if (GUIUtility.keyboardControl != 0)
                return true;

            var eventSystem = EventSystem.current;
            var selected = eventSystem != null ? eventSystem.currentSelectedGameObject : null;
            if (selected == null)
                return false;

            var tmp = selected.GetComponentInParent<TMP_InputField>();
            if (tmp != null)
                return tmp.isFocused;

            var legacy = selected.GetComponentInParent<InputField>();
            if (legacy != null)
                return legacy.isFocused;

            return false;
        }

        private static void DisableContext()
        {
            if (!IsWindows())
                return;

            var hwnd = GetGameWindow();
            if (hwnd == IntPtr.Zero)
                return;

            if (!_hasSavedContext)
            {
                _savedContext = ImmGetContext(hwnd);
                _hasSavedContext = _savedContext != IntPtr.Zero;
            }

            ImmAssociateContext(hwnd, IntPtr.Zero);
        }

        private static void RestoreContext()
        {
            if (!IsWindows())
                return;

            // Only use the cached game window. Calling GetActiveWindow while the
            // game is unfocused could associate a context with another process.
            var hwnd = _window;
            if (hwnd == IntPtr.Zero)
                return;

            if (_hasSavedContext)
            {
                ImmAssociateContext(hwnd, _savedContext);
                _savedContext = IntPtr.Zero;
                _hasSavedContext = false;
            }
            else
            {
                ImmAssociateContextEx(hwnd, IntPtr.Zero, IACE_DEFAULT);
            }
        }

        private static IntPtr GetGameWindow()
        {
            if (_window != IntPtr.Zero)
                return _window;

            try
            {
                _window = GetActiveWindow();
                if (_window == IntPtr.Zero)
                    _window = FindWindow(null, Application.productName);
            }
            catch (Exception ex)
            {
                Main.Handler?.Warning($"[IMBlocker][IME] failed to find game window: {ex.Message}");
            }

            return _window;
        }

        private static bool IsWindows()
        {
            var platform = Application.platform;
            return platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor;
        }

        private const uint IACE_DEFAULT = 0x0010;

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ImmAssociateContextEx(IntPtr hWnd, IntPtr hIMC, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);
    }
}
