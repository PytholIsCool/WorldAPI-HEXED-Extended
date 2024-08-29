using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Element;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.MM.Carousel.Items;

public class CToggle : Root {
    public Action<bool> Listener { get; set; }

    public Toggle ToggleCompnt { get; private set; }
    public UiToggleTooltip ToolTip { get; private set; }
    public Transform Handle { get; private set; }

    private RadioButton toggleSwitch;
    private bool shouldInvoke = true;

    private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);

    public CToggle(CGrp grp, string text, Action<bool> stateChange, bool defaultState = false, string toolTip = "", string toolTip2 = "") {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        gameObject = Object.Instantiate(APIBase.MMCTgl, grp.MenuContents.transform);
        transform = gameObject.transform;
        gameObject.name = text;

        TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = text;
        TMProCompnt.richText = true;
        Text = text;

        (ToolTip = gameObject.GetComponent<UiToggleTooltip>())._localizableString = toolTip.Localize();

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
            APIBase.Events.onCToggleValChange?.Invoke(this, val);
            toggleSwitch.Method_Public_Void_Boolean_0(val);
            Handle.localPosition = val ? onPos : offPos;
        }));
        gameObject.GetComponent<SettingComponent>().enabled = false;

    }

    public void SoftSetState(bool value) {
        shouldInvoke = false;
        ToggleCompnt.isOn = value;
        shouldInvoke = true;
    }
}