using System;
using System.Collections.Generic;
using CustomKeyBinds.Tools;
using UnityEngine;

namespace CustomKeyBinds.Components
{
    public class SettingsWindow : PopUpWindow
    {
        public List<KeySelector> content = new List<KeySelector>();
        public GameObject separator;

        public SettingsWindow(OptionsMenuBehaviour optionsMenu) : base(optionsMenu, "KeyBindPopUp", "Keybinds",
            new Vector2(5.5f, 4f), new Vector2(-3f, 1.75f))
        {
            SetButtonListener(OnClose);
            Start(optionsMenu);
        }

        private void Start(OptionsMenuBehaviour optionsMenu)
        {
            //Setup separator
            separator = new GameObject("Separator");
            separator.transform.parent = holder.transform;
            var start = new Vector3(0f, size.y / 2 * 0.75f, holder.transform.position.z - 21f);
            separator.transform.position = start;
            var lr = separator.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Unlit/ColoredVertices"));
            lr.SetColors(Color.white, Color.white);
            lr.SetWidth(0.05f, 0.05f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, new Vector3(0f, -size.y / 2 * 0.9f, holder.transform.position.z - 21f));

            //Create child elements
            var i = 0;
            foreach (KeyAction action in Enum.GetValues(typeof(KeyAction)))
            {
                const int numRows = 5;
                var column = i / numRows;
                var row = i % numRows;
                const float topPercent = 0.70f;
                const float botPercent = 0.90f;

                var selectorPos = new Vector2(-size.x / 2 * botPercent + column * (size.x / 2),
                    size.y / 2 * topPercent - row * (size.y * (topPercent + botPercent) / 2 / numRows));
                content.Add(new KeySelector(optionsMenu,
                    action.ToString(),
                    action.ToString(),
                    action,
                    selectorPos,
                    selectorPos + new Vector2(size.x / 3, 0f),
                    holder));
                i++;
            }
        }

        public new void OnClose()
        {
            foreach (var selector in content) selector.OnClose();
            base.OnClose();
        }
    }
}