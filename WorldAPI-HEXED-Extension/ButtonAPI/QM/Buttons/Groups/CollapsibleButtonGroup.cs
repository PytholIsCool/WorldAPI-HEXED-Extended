using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Groups;

public class CollapsibleButtonGroup : ButtonGroupControl {
    public bool IsOpen { get; internal set; }
    public GameObject headerObj { get; internal set; }
    public ButtonGroup buttonGroup { get; internal set; }

    public Action<bool> OnClose { get; set; }

    public CollapsibleButtonGroup(Transform parent, string text, bool openByDefault = true) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        headerObj = Object.Instantiate(APIBase.ColpButtonGrp, parent);
        headerObj.name = $"{text}_CollapsibleButtonGroup";
        headerObj.transform.Find("QM_Settings_Panel/VerticalLayoutGroup").DestroyChildren();

        TMProCompnt = headerObj.transform.Find("QM_Foldout/Label").GetComponent<TMPro.TextMeshProUGUI>();
        TMProCompnt.richText = true;
        TMProCompnt.text = text;

        gameObject = (buttonGroup = new(parent, string.Empty, true)).gameObject;
        GroupContents = buttonGroup.GroupContents;

        OnClose = new((val) => {
            buttonGroup.gameObject.SetActive(val);
            IsOpen = val;
        });

        var foldout = headerObj.transform.Find("QM_Foldout/Background_Button").GetComponent<Toggle>();
        foldout.isOn = openByDefault;
        foldout.onValueChanged.AddListener(new Action<bool>(val => OnClose?.Invoke(val)));
    }

    public CollapsibleButtonGroup(WorldPage page, string text, bool openByDefault = false) : this(page.MenuContents, text, openByDefault) 
        { }
}