namespace IMBlocker
{
    public static class Main
    {
        public static IHandler Handler { get; private set; } = null!;
        public static Settings Settings { get; private set; } = null!;

        public static bool Initialize(IHandler handler)
        {
            Handler = handler;
            Settings = handler.LoadSettings<Settings>();
            PatchManager.Initialize(BuildInfo.ModId);
            ImeBlocker.Initialize();

            handler.OnToggle += OnToggle;
            handler.OnUpdate += _ => OnGameUpdate();
            handler.OnGUI += LmSettingsUI.Draw;
            handler.OnSaveGUI += () => Handler.SaveSettings(Settings);

            SetEnabled(Settings.Enabled);

            handler.Log($"{BuildInfo.LogPrefix} v{BuildInfo.Version} initialized");
            return true;
        }

        /// <summary>
        /// Apply the user-facing enable switch immediately, without waiting for a
        /// restart. Loader-level toggles (UMM/Melon/BepInEx) also land here.
        /// </summary>
        public static void SetEnabled(bool value)
        {
            Settings.Enabled = value;

            if (value)
            {
                ImeBlocker.Reset();
                PatchManager.UpdateAllPatches();
            }
            else
            {
                ImeBlocker.Restore();
                PatchManager.UnpatchAll();
            }
        }

        private static void OnGameUpdate()
        {
            if (!Settings.Enabled)
                return;

            ImeBlocker.Update();
        }

        private static void OnToggle(bool value)
        {
            Handler.Log(value ? "Mod enabled" : "Mod disabled");
            SetEnabled(value);
        }
    }
}
