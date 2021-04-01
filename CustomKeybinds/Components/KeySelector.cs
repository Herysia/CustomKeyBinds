using System;
using System.Collections.Generic;
using CustomKeyBinds.Tools;
using UnityEngine;
using Object = UnityEngine.Object;


namespace CustomKeyBinds.Components
{
    public class KeySelector
    {
        private static readonly List<KeySelector> Selectors = new List<KeySelector>();
        private static readonly KeyCode[] KeyCodes = (KeyCode[]) Enum.GetValues(typeof(KeyCode)); //.Where();

        private static KeySelector _isSelecting;
        private static Component _ignoreClose;

        public static bool canEscape = true;

        private OptionsMenuButton _button;

        private GameObject _holder;
        private Component _label;

        private TextRenderer _textRenderer;
        public Vector2 buttonPos;
        public KeyAction key;
        public string label;
        public Vector2 labelPos;

        public string name;

        //Setup key listener

        public KeySelector(OptionsMenuBehaviour optionsMenu, string name, string label, KeyAction key, Vector2 labelPos,
            Vector2 buttonPos, GameObject parent = null)
        {
            this.name = name;
            this.label = label;
            this.key = key;
            this.labelPos = labelPos;
            this.buttonPos = buttonPos;

            Selectors.Add(this);
            Start(optionsMenu, parent);
        }

        private void Start(OptionsMenuBehaviour optionsMenu, GameObject parent)
        {
            //Get original components
            var oControlGroup = Utils.GetChildComponentByName<Component>(optionsMenu, "ControlGroup");
            var oControlText = Utils.GetChildComponentByName<Component>(oControlGroup, "ControlText");
            var oIgnoreClose = Utils.GetChildComponentByName<Component>(optionsMenu, "IgnoreClose");
            if (oIgnoreClose == null)
            {
                oIgnoreClose = Utils.GetChildComponentByName<Component>(optionsMenu, "CloseBackground");
            }

            //Create component and add it to the parent
            _holder = new GameObject(name);

            //Set button parent
            _holder.transform.parent = parent == null ? optionsMenu.transform : parent.transform;

            //Configure holder and his components
            _holder.layer = 5; //UI
            _holder.transform.localScale = new Vector3(1f, 1f, 1f);
            _holder.transform.localPosition = Vector3.zero;

            //clone original sub components
            _label = Object.Instantiate(oControlText, _holder.transform);
            _ignoreClose = Object.Instantiate(oIgnoreClose, _holder.transform);

            //Configure label
            _textRenderer = _label.gameObject.GetComponent<TextRenderer>();
            _label.transform.localPosition = new Vector3(labelPos.x, labelPos.y);

            //Create button
            _button = new OptionsMenuButton(optionsMenu, key + "Button", ConfigManager.keyBinds[key].ToString(),
                OnClick, buttonPos + new Vector2(0f, -0.2f) /* Centered vertical anchor */, new Vector2(1.0f, 0.4f),
                _holder);
            _button.SetOnMouseOut(OnMouseOut);
            _button.SetOnMouseOver(OnMouseOver);

            //Configure ignoreClose
            _ignoreClose.gameObject.SetActive(false);
            _ignoreClose.transform.localPosition = new Vector3(_ignoreClose.transform.localPosition.x,
                _ignoreClose.transform.localPosition.y, _ignoreClose.transform.localPosition.z - 1000f);

            var collider = _ignoreClose.gameObject.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(Screen.width, Screen.height);
        }

        private static void OnInput()
        {
            if (_isSelecting == null)
                return;

            foreach (var code in KeyCodes)
            {
                if (!Input.GetKey(code))
                    continue;
                ConfigManager.UpdateKey(_isSelecting?.key, code);
                _isSelecting?._button.SetLabel(code.ToString());
                var backup = _isSelecting;
                _isSelecting = null;
                _ignoreClose.gameObject.SetActive(false);
                backup?.OnMouseOut();
                
                if (code == KeyCode.Escape)
                    canEscape = false;
            }
        }

        private void OnClick()
        {
            if (_isSelecting != null)
                return;
            _ignoreClose.gameObject.SetActive(true);
            _isSelecting = this;
            _button.SetButtonBgColor(new Color(1f, 0.8f, 0f, 1f));
            _button.SetLabel("...");
        }

        private void OnMouseOut()
        {
            if (_isSelecting != null)
                return;
            _button.OnOut();
        }

        private void OnMouseOver()
        {
            if (_isSelecting != null)
                return;
            _button.OnOver();
        }

        public void OnClose()
        {
            _isSelecting = null;
            _ignoreClose.gameObject.SetActive(false);
            OnMouseOut();
            _button.SetLabel(ConfigManager.keyBinds[key].ToString());
        }

        public static void HudUpdate()
        {
            Selectors.RemoveAll(item => item._holder == null);
            if (_isSelecting != null && Input.anyKeyDown) OnInput();
            foreach (var selector in Selectors) selector.Update();
        }

        private void Update()
        { 
            _textRenderer.Text = label;
        }
    }
}
