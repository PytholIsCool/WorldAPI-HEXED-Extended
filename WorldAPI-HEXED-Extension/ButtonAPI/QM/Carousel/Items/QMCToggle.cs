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
using WorldAPI.ButtonAPI.QM.Controls;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items;

public class QMCToggle : QMCControl
{
    public Action<bool> ListenerC { get; set; }
    public RadioButton toggleSwitch { get; set; }
    private bool shouldInvoke = true;

    private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
    public QMCToggle(Transform parent, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
    {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (transform = (gameObject = Object.Instantiate(APIBase.QMCarouselToggleTemplate, parent)).transform).name = text;

        Text = (TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;

        (ToggleToolTip = gameObject.GetComponent<UiToggleTooltip>())._localizableString = tooltip.Localize();

        if (separator != false)
            AddSeparator(parent);
        (toggleSwitch = transform.Find("RightItemContainer/Cell_MM_OnOffSwitch").GetComponent<RadioButton>()).Method_Public_Void_Boolean_0(defaultState);

        (Handle = toggleSwitch._handle).transform.localPosition = defaultState ? onPos : offPos;

        (ToggleCompnt = gameObject.GetComponent<Toggle>()).onValueChanged = new();
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
    public QMCToggle(QMCGroup group, string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, stateChange, tooltip, defaultState, separator) { }
    public QMCToggle(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action<bool> stateChange, bool defaultState = false, bool separator = false)
        : this(buttonGroup.QMCParent, text, stateChange, tooltip, defaultState, separator) { }
}
