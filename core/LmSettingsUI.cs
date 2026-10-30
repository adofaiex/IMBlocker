using System;
using UnityEngine;

namespace IMBlocker
{
    /// <summary>
    /// Settings page drawn by the loader's OnGUI hook. Uses the default IMGUI
    /// skin (the same controls UMM uses) so the mod does not need to ship a
    /// custom layout/texture stack.
    /// </summary>
    public static class LmSettingsUI
    {
        private static GUIStyle? _descriptionStyle;

        public static void Draw()
        {
            try
            {
                var s = Main.Settings;

                GUILayout.BeginVertical(GUI.skin.box);

                GUILayout.Label($"{BuildInfo.DisplayName} v{Main.Handler?.ModVersion}", GUI.skin.label);
                GUILayout.Space(4);

                DrawLanguage(s);

                GUILayout.Space(8);
                bool enabled = GUILayout.Toggle(s.Enabled, " " + LmI18n.Get("settings.enabled"));
                if (enabled != s.Enabled)
                {
                    Main.SetEnabled(enabled);
                    GUI.changed = true;
                }
                GUILayout.Label("  " + LmI18n.Get("settings.enabled.Description"), DescriptionStyle);

                GUILayout.Space(6);
                bool blockIme = GUILayout.Toggle(s.BlockIme, " " + LmI18n.Get("settings.blockIme"));
                if (blockIme != s.BlockIme)
                {
                    s.BlockIme = blockIme;
                    ImeBlocker.OnSettingChanged();
                    GUI.changed = true;
                }
                GUILayout.Label("  " + LmI18n.Get("settings.blockIme.Description"), DescriptionStyle);

                GUILayout.Space(8);
                GUILayout.Label(LmI18n.Get("settings.hint"), DescriptionStyle);

                GUILayout.EndVertical();

                if (GUI.changed)
                    Main.Handler?.SaveSettings(s);
            }
            catch (Exception ex)
            {
                Main.Handler?.Error($"[IMBlocker][Settings] draw failed: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void DrawLanguage(Settings s)
        {
            var langs = LmI18n.AvailableLanguages();
            var labels = new string[langs.Length + 1];
            labels[0] = LmI18n.Get("settings.languageAuto");

            int selected = 0;
            for (int i = 0; i < langs.Length; i++)
            {
                labels[i + 1] = LmI18n.DisplayName(langs[i]);
                if (string.Equals(s.Language, langs[i], StringComparison.OrdinalIgnoreCase))
                    selected = i + 1;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(LmI18n.Get("settings.language"), GUILayout.Width(180f));
            int next = GUILayout.Toolbar(selected, labels);
            GUILayout.EndHorizontal();

            if (next == selected)
                return;

            s.Language = next == 0 ? null : langs[next - 1];
            LmI18n.ForceReload();
            GUI.changed = true;
        }

        private static GUIStyle DescriptionStyle
        {
            get
            {
                if (_descriptionStyle != null)
                    return _descriptionStyle;

                _descriptionStyle = new GUIStyle(GUI.skin.label)
                {
                    wordWrap = true,
                    richText = true
                };
                return _descriptionStyle;
            }
        }
    }
}
