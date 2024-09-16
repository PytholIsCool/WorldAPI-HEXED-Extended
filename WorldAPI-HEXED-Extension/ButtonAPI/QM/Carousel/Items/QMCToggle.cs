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
using Serpentine.ButtonAPI.QM.Controls;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items
{
    public class QMCToggle : QMCControl //good chunk of this was taken from the CToggle class.
    {
        public Action<bool> ListenerC { get; set; }
        public RadioButton toggleSwitch {  get; set; }
        private bool shouldInvoke = true;

        private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
        public QMCToggle(Transform parent, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search has FAILED!");

            gameObject = Object.Instantiate(APIBase.QMCarouselToggleTemplate, parent);
            transform = gameObject.transform;
            gameObject.name = text;

            TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;

            (ToggleToolTip = gameObject.GetComponent<UiToggleTooltip>())._localizableString = tooltip.Localize();

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
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ListenerC, text);
                APIBase.Events.onQMCToggleValChange?.Invoke(this, val);
                toggleSwitch.Method_Public_Void_Boolean_0(val);
                Handle.localPosition = val ? onPos : offPos;
            }));
            gameObject.GetComponent<SettingComponent>().enabled = false;
        }        
        public QMCToggle(QMCGroup group, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
            : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, stateChange, tooltip, defaultState, separator)
        { }
        public QMCToggle(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action<bool> stateChange, bool defaultState = false, bool separator = false)
            : this(buttonGroup.QMCParent, text, stateChange, tooltip, defaultState, separator)
        { }
    }
}
