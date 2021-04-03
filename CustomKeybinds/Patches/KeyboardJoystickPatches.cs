using CustomKeyBinds.Tools;
using HarmonyLib;
using UnityEngine;

// DestroyableSingleton -> PPAEIPHJPDH;
// DestroyableSingleton.InstanceExists -> PPAEIPHJPDH.JECNDKBIOFO
// DestroyableSingleton.Instance -> CMJOLNCMAPD
// MapBehaviour.Instance.IsOpen -> CBAGIJCCEGG.Instance.GELKOGPNIBJ (/!\ it's maybe IsOpenStopped
// PlayerControl.LocalPlayer.Data.IsImpostor -> PlayerControl.LocalPlayer.IDOFAMCIJKE.DAPKNDBLKIA


namespace CustomKeyBinds.Patches
{
    internal static class KeyboardJoystickPatches
    {
        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
        public static class PatchMainMenuManagerUpdate
        {
            public static bool Prefix(KeyboardJoystick __instance)
            {
                if (!PlayerControl.LocalPlayer) return false;
                var del = Vector2.zero;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Right]))
                    del.x += 1f;
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Left]))
                    del.x -= 1f;
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Forward]))
                    del.y += 1f;
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(ConfigManager.keyBinds[KeyAction.Backward]))
                    del.y -= 1f;
                del.Normalize();
                __instance.GOJFEOOBJKI = del;

                HandleHud();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (Minigame.Instance)
                        Minigame.Instance.Close();
                    else if (DestroyableSingleton<HudManager>.JECNDKBIOFO && MapBehaviour.Instance &&
                             MapBehaviour.Instance.LFGAAGECFFO)
                        MapBehaviour.Instance.Close();
                    else if (CustomPlayerMenu.Instance) CustomPlayerMenu.Instance.Close(true);
                }
                
                return false;
            }

            private static void HandleHud()
            {
                if (!DestroyableSingleton<HudManager>.JECNDKBIOFO) return;
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Report]))
                    DestroyableSingleton<HudManager>.CMJOLNCMAPD.ReportButton.DoClick();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Use]))
                    DestroyableSingleton<HudManager>.CMJOLNCMAPD.UseButton.DoClick();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Map]))
                    DestroyableSingleton<HudManager>.CMJOLNCMAPD.OpenMap();
                if (Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Tasks])) Utils.ToggleTab();
                /*
                if((PlayerControl.LocalPlayer.IDOFAMCIJKE != null))
                {
                    System.Console.WriteLine("1: {0}", PlayerControl.LocalPlayer.IDOFAMCIJKE.BGJOHABEFMJ);
                    System.Console.WriteLine("2: {0}", PlayerControl.LocalPlayer.IDOFAMCIJKE.GBPMEHJFECK);
                    System.Console.WriteLine("3: {0}", PlayerControl.LocalPlayer.IDOFAMCIJKE.CIDDOFDJHJH);
                    System.Console.WriteLine("4: {0}", PlayerControl.LocalPlayer.IDOFAMCIJKE.FGNJJFABIHJ);
                }
                */
                if (PlayerControl.LocalPlayer.IDOFAMCIJKE != null && PlayerControl.LocalPlayer.IDOFAMCIJKE.CIDDOFDJHJH &&
                    Input.GetKeyDown(ConfigManager.keyBinds[KeyAction.Kill]))
                    DestroyableSingleton<HudManager>.CMJOLNCMAPD.KillButton.PerformKill();
            }
        }
    }
}