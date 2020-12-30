using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

namespace CustomKeyBinds.Tools
{
    public static class ConfigManager
    {
        private static Configuration _config;

        public static Dictionary<KeyAction, KeyCode> keyBinds;

        public static readonly Dictionary<KeyAction, KeyCode> DefaultKeyBinds = new Dictionary<KeyAction, KeyCode>
        {
            {KeyAction.Forward, KeyCode.W},
            {KeyAction.Left, KeyCode.A},
            {KeyAction.Backward, KeyCode.S},
            {KeyAction.Right, KeyCode.D},
            {KeyAction.Use, KeyCode.Space},
            {KeyAction.Report, KeyCode.R},
            {KeyAction.Kill, KeyCode.Q},
            {KeyAction.Map, KeyCode.Tab},
            {KeyAction.Tasks, KeyCode.T}
        };

        public static void LoadKeybinds()
        {
            if (keyBinds != null)
                return;

            keyBinds = new Dictionary<KeyAction, KeyCode>(DefaultKeyBinds);

            var roamingConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = roamingConfig.FilePath
            };

            _config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var keybindsSection = _config.GetSection("keybinds") as KeybindSection;
            if (keybindsSection == null || keybindsSection.Settings.Count == 0)
                //Write default config
                WriteDefaultConfig(keybindsSection);
            else
                //Read default config file
                ApplyConfigToKeybinds(keyBinds, keybindsSection);
        }

        private static void WriteDefaultConfig(KeybindSection keybindsSection)
        {
            if (keybindsSection == null)
            {
                keybindsSection = new KeybindSection();
                _config.Sections.Add("keybinds", keybindsSection);
            }

            if (DefaultKeyBinds.TryGetValue(KeyAction.Forward, out var bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Forward), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Left, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Left), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Backward, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Backward), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Right, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Right), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Use, out bind))
                keybindsSection.Settings.Add(new NameValueConfigurationElement(nameof(KeyAction.Use), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Report, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Report), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Kill, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Kill), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Map, out bind))
                keybindsSection.Settings.Add(new NameValueConfigurationElement(nameof(KeyAction.Map), bind.ToString()));
            if (DefaultKeyBinds.TryGetValue(KeyAction.Tasks, out bind))
                keybindsSection.Settings.Add(
                    new NameValueConfigurationElement(nameof(KeyAction.Tasks), bind.ToString()));


            _config.Save(ConfigurationSaveMode.Modified);
        }

        private static void ApplyConfigToKeybinds(Dictionary<KeyAction, KeyCode> keybinds,
            KeybindSection keybindsSection)
        {
            foreach (NameValueConfigurationElement e in keybindsSection.Settings)
            {
                var action = (KeyAction) Enum.Parse(typeof(KeyAction), e.Name);

                if (!Enum.IsDefined(typeof(KeyAction), action)) // Ignore, old config ? malformed ? -> delete ?
                    return;
                var code = (KeyCode) Enum.Parse(typeof(KeyCode), e.Value);
                if (!Enum.IsDefined(typeof(KeyCode), code))
                {
                    //Wrong key, write default value instead
                    if (DefaultKeyBinds.TryGetValue(action, out var bind))
                        keybindsSection.Settings.Add(new NameValueConfigurationElement(e.Name, bind.ToString()));
                }
                else
                {
                    keybinds[action] = code;
                }
            }

            _config.Save(ConfigurationSaveMode.Full);
        }

        public static void UpdateKey(KeyAction? action, KeyCode code)
        {
            if (action == null)
                return;
            keyBinds[action.GetValueOrDefault()] = code;
            var keybindsSection = _config.GetSection("keybinds") as KeybindSection;

            keybindsSection?.Settings.Add(new NameValueConfigurationElement(action.ToString(), code.ToString()));
        }

        private class KeybindSection : ConfigurationSection
        {
            [ConfigurationProperty("", IsDefaultCollection = true)]
            public NameValueConfigurationCollection Settings => (NameValueConfigurationCollection) base[""];
        }
    }
}