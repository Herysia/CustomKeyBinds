using System;
using System.Collections.Generic;
using System.Linq;
using CustomKeyBinds.Tools;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


namespace CustomKeyBinds.Components
{
    public class PopUpWindow
    {
        private static readonly List<PopUpWindow> Windows = new List<PopUpWindow>();


        private Component _backgroundComponent;
        private Component _closeButton;
        private Component _controlText;

        private TextRenderer _textRenderer;


        protected GameObject holder;

        public string name;
        public Vector2 pos;
        public Vector2 size;
        public string title;

        public PopUpWindow(OptionsMenuBehaviour optionsMenu, string name, string title, Vector2 size, Vector2 pos,
            GameObject parent = null)
        {
            this.name = name;
            this.title = title;
            this.size = size;
            this.pos = pos;

            Windows.Add(this);
            Start(optionsMenu, parent);
        }

        private void Start(OptionsMenuBehaviour optionsMenu, GameObject parent)
        {
            //Get original components
            var oBackgroundComponent = Utils.GetChildComponentByName<Component>(optionsMenu, "Background");
            var oCloseButton = Utils.GetChildComponentByName<Component>(optionsMenu, "CloseButton");
            var oControlText = Utils.GetChildComponentByName<Component>(optionsMenu, "ControlText");

            //Create component and add it to the parent
            holder = new GameObject(name);

            //Set button parent
            holder.transform.parent = parent == null ? optionsMenu.transform : parent.transform;

            //Configure holder and his components
            holder.layer = 5; //UI
            holder.transform.localScale = new Vector3(1f, 1f, 1f);
            holder.transform.localPosition = Vector3.back * 20f;

            //clone original sub components
            _backgroundComponent = Object.Instantiate(oBackgroundComponent, holder.transform);
            if(oCloseButton != null)
                _closeButton = Object.Instantiate(oCloseButton, holder.transform);
            _controlText = Object.Instantiate(oControlText, holder.transform);

            //Configure background size
            _backgroundComponent.transform.localPosition = new Vector3(_backgroundComponent.transform.localPosition.x,
                _backgroundComponent.transform.localPosition.y, 9f);
            var renderer = _backgroundComponent.gameObject.GetComponent<SpriteRenderer>();
            renderer.size = size;
            var collider = _backgroundComponent.gameObject.GetComponent<BoxCollider2D>();
            collider.size = size;

            //Configure close button
            if (_closeButton != null)
            {
                _closeButton.transform.localPosition = new Vector3(pos.x, pos.y);
                var button = _closeButton.gameObject.GetComponent<PassiveButton>();
                Object.Destroy(button);
                button = _closeButton.gameObject.AddComponent<PassiveButton>();
                button.OnClick.AddListener((UnityAction)OnClose);
                button.OnMouseOver = new UnityEvent();
                button.OnMouseOut = new UnityEvent();
            }

            //Configure title title
            _textRenderer = _controlText.gameObject.GetComponent<TextRenderer>();
            _textRenderer.Centered = true;
            _controlText.transform.localPosition = new Vector3( /*-(size.x / 2) * 0.90f */ 0f, size.y / 2 * 0.92f);


            //Enable the button
            holder.gameObject.SetActive(false);
        }

        protected void SetButtonListener(Action listener)
        {
            if (_closeButton != null)
            {
                var button = _closeButton.gameObject.GetComponents<PassiveButton>().Last();
                button.OnClick.RemoveAllListeners();
                button.OnClick.AddListener(listener);
            }
        }

        public void OnClose()
        {
            holder.SetActive(false);
        }

        public void Show()
        {
            holder.SetActive(true);
        }

        public static void HudUpdate()
        {
            Windows.RemoveAll(item =>
                item.holder == null|| item._backgroundComponent == null ||
                item._controlText == null);
            foreach (var window in Windows) window.Update();
        }

        private void Update()
        {
            _textRenderer.Text = title;
        }
    }
}