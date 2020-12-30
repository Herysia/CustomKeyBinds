using HarmonyLib;

namespace CustomKeyBinds.Patches
{
    internal static class VersionShowerPatches
    {
        [HarmonyPatch(typeof(VersionShower), "Start")]
        public static class PatchVersionShowerStart
        {
            public static void Postfix(VersionShower __instance)
            {
                __instance.text.Text += "\n[D41313FF]CustomKeyBinds v1.0 by Herysia#4293";
            }
        }
    }
}