using System.Collections.Generic;
using HarmonyLib;
using SkyHook;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace IMBlocker
{
    public static class Patches
    {
        /// <summary>
        /// The current beta removed LAlt from
        /// <c>RDInputType_AsyncKeyboard.SpecialKeys</c>, while the legacy keyboard
        /// path still filters LeftAlt/RightAlt. With async input enabled, Alt-based
        /// IME shortcuts therefore count as gameplay input. Re-add both keys for
        /// the async path; the list is only consulted on "went down", so holding
        /// Alt never suppresses a real hit.
        /// </summary>
        [HarmonyPatch(typeof(RDInputType_AsyncKeyboard), "GetSpecialInput")]
        public static class RDInputType_AsyncKeyboard_GetSpecialInput
        {
            public static void Postfix(List<AsyncKeyCode> keys)
            {
                if (!Main.Settings.Enabled || !Main.Settings.BlockIme)
                    return;

                AddIfPressed(keys, KeyLabel.LAlt);
                AddIfPressed(keys, KeyLabel.RAlt);
            }

            private static void AddIfPressed(List<AsyncKeyCode> keys, KeyLabel label)
            {
                if (!AsyncInput.GetKey(label))
                    return;

                var code = new AsyncKeyCode(label);
                if (!keys.Contains(code))
                    keys.Add(code);
            }
        }
    }
}
