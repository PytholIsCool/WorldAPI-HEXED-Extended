using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using VRC.Localization;
using VRC.UI.Element;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI;
using Object = UnityEngine.Object;
using WorldAPI.ButtonAPI.Groups;
using Valve.VR.InteractionSystem;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items
{
    public class QMCToggle : ToggleControl //good chunk of this was taken from the CToggle class.
    {
        public Action<bool> ListenerC { get; set; }
        public Action<bool> SecondaryListener {  get; set; } //this will be used later on...
        public UiToggleTooltip ToolTip { get; private set; }
        public UiToggleTooltip SecondaryToolTip { get; private set; } //this will be used later on...
        public Transform AdditionalToggle { get; set; } //this will be used later on...
        public Transform Handle { get; private set; }
        public Transform SecondaryHandle { get; private set; } //this will be used later on...
//oh also, when i say that those will be used later on, i mean ill be deleting those and doing something else thats way more optimised!
        private RadioButton toggleSwitch;
        private bool shouldInvoke = true;

        private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
        public QMCToggle(Transform parent, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            gameObject = Object.Instantiate(APIBase.QMCarouselToggleTemplate, parent);
            transform = gameObject.transform;
            gameObject.name = text;

            TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;

            (ToolTip = gameObject.GetComponent<UiToggleTooltip>())._localizableString = tooltip.Localize();

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, parent);
                seB.name = "Separator";
            }
            toggleSwitch = transform.Find("RightItemContainer/Cell_MM_OnOffSwitch").GetComponent<RadioButton>();
            toggleSwitch.Method_Public_Void_Boolean_0(defaultState);

            (Handle = toggleSwitch._handle)
                .transform.localPosition = defaultState ? onPos : offPos;

            ToggleCompnt = gameObject.GetComponent<Toggle>();
            ToggleCompnt.onValueChanged = new();
            ToggleCompnt.isOn = defaultState;
            ListenerC = stateChange;
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) => {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ListenerC, text);
                APIBase.Events.onQMCToggleValChange?.Invoke(this, val);
                toggleSwitch.Method_Public_Void_Boolean_0(val);
                Handle.localPosition = val ? onPos : offPos;
            }));
            gameObject.GetComponent<SettingComponent>().enabled = false;
        }
        public QMCToggle SetState(bool value)
        {
            shouldInvoke = false;
            ToggleCompnt.isOn = value;
            shouldInvoke = true;

            return this;
        }

        //public void HardSetState(bool value) i hate you cyconi
        //{
        //    shouldInvoke = false;
        //    ToggleCompnt.isOn = value;
        //    shouldInvoke = true;

        //    Listener?.Invoke(value);
        //    ToggleCompnt.onValueChanged.Invoke(value);
        //}
        public QMCToggle(QMCGroup group, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
            : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, stateChange, tooltip, defaultState, separator)
        { }
        public QMCToggle(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action<bool> stateChange, bool defaultState = false, bool separator = false)
            : this(buttonGroup.QMCParent, text, stateChange, tooltip, defaultState, separator)
        { }
    }
}
