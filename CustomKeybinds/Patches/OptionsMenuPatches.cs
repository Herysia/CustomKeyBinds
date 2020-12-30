using CustomKeyBinds.Components;
using HarmonyLib;
using UnityEngine;

using OptionsMenuBehaviour = BOMIGDLINBO;

namespace CustomKeyBinds.Patches
{
    internal static class OptionsMenuPatches
    {
        private static SettingsWindow _keyBindsPopUp;

        private static void OpenKeyBindMenu()
        {
            _keyBindsPopUp.Show();
        }

        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
        public static class PatchOptionsMenuStart
        {
            public static void Postfix(OptionsMenuBehaviour __instance)
            {
                new OptionsMenuButton(__instance, "OpenKeyBindMenuButton", "KeyBinds", OpenKeyBindMenu,
                    new Vector2(0f, -0.9f));
                _keyBindsPopUp = new SettingsWindow(__instance);
            }
        }

        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Close))]
        public static class PatchOptionsMenuClose
        {
            public static void Postfix()
            {
                _keyBindsPopUp?.OnClose();
            }
        }

        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Update))]
        public static class PatchOptionsMenuUpdate
        {
            public static void Postfix()
            {
                OptionsMenuButton.HudUpdate();
                KeySelector.HudUpdate();
                PopUpWindow.HudUpdate();
            }
        }
    }
}