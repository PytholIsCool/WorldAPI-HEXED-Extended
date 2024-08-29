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

namespace WCv2.ButtonAPI.QM.Carousel.Items
{
    public class QMCToggle : Root //good chunk of this was taken from the CToggle class.
    {
        public Action<bool> Listener { get; set; }
        public Toggle ToggleCompnt { get; private set; }
        public UiToggleTooltip ToolTip { get; private set; }
        public Transform Handle { get; private set; }

        private RadioButton toggleSwitch;
        private bool shouldInvoke = true;

        private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
        public QMCToggle(QMCGroup group, string text, Action<bool> stateChange, bool defaultState = false, string toolTip = "", bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            gameObject = Object.Instantiate(APIBase.QMCarouselToggleTemplate, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
            transform = gameObject.transform;
            gameObject.name = text;

            TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;

            (ToolTip = gameObject.GetComponent<UiToggleTooltip>())._localizableString = toolTip.Localize();

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
                seB.name = "Separator";
            }

            toggleSwitch = transform.Find("RightItemContainer/Cell_MM_OnOffSwitch").GetComponent<RadioButton>();
            toggleSwitch.Method_Public_Void_Boolean_0(defaultState);

            (Handle = toggleSwitch._handle)
                .transform.localPosition = defaultState ? onPos : offPos;

            ToggleCompnt = gameObject.GetComponent<Toggle>();
            ToggleCompnt.onValueChanged = new();
            ToggleCompnt.isOn = defaultState;
            Listener = stateChange;
            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) => {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, Listener, text);
                APIBase.Events.onQMCToggleValChange?.Invoke(this, val);
                toggleSwitch.Method_Public_Void_Boolean_0(val);
                Handle.localPosition = val ? onPos : offPos;
            }));
            gameObject.GetComponent<SettingComponent>().enabled = false;
        }
        public void SoftSetState(bool value)
        {
            shouldInvoke = false;
            ToggleCompnt.isOn = value;
            shouldInvoke = true;
        }
    }
}
