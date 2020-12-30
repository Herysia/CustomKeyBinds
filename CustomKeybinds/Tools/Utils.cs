using System.Linq;
using UnityEngine;

namespace CustomKeyBinds.Tools
{
    public static class Utils
    {
        //https://forum.unity.com/threads/get-ui-components-by-name-with-getcomponent.408004/
        public static T GetChildComponentByName<T>(Component parent, string name) where T : Component
        {
            return parent.GetComponentsInChildren<T>(true)
                .FirstOrDefault(component => component.gameObject.name == name);
        }

        public static T GetChildComponentByName<T>(GameObject parent, string name) where T : Component
        {
            return parent.GetComponentsInChildren<T>(true)
                .FirstOrDefault(component => component.gameObject.name == name);
        }

        internal static void ToggleTab()
        {
            var taskStuff = DestroyableSingleton<HudManager>.Instance?.TaskStuff;

            if (taskStuff == null || !taskStuff.active)
                return;

            var buttonObject = GetChildComponentByName<Component>(taskStuff, "Tab");

            if (buttonObject == null)
                return;

            var button = buttonObject.gameObject.GetComponent<PassiveButton>();

            button?.ReceiveClickDown();
        }
    }
}