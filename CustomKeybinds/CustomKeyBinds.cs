using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using CustomKeyBinds.Tools;
using HarmonyLib;

namespace CustomKeyBinds
{
    [BepInPlugin("com.herysia.customkeybinds", "AU Custom KeyBinds", "1.0")]
    public class CustomKeyBinds : BasePlugin
    {
        private static Harmony _harmony;

        public override void Load()
        {
            ConfigManager.LoadKeybinds();
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
    }
}