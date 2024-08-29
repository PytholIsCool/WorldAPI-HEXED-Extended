using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;

namespace WorldAPI.ButtonAPI.Buttons.Groups;

public class DuoButtons : Root {
    public Transform ObjectHolder { get; private set; }

    public VRCButton ButtonOne { get; private set; }
    public VRCButton ButtonTwo { get; private set; }

    public DuoButtons(GameObject menu, string buttonOne, string buttonOneTooltip, Action<DuoButtons> btnAction, string buttonTwo,
        string buttonTwoTooltip, Action<DuoButtons> buttonTwoAction) 
    {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        (transform = (gameObject = new("Button_DuoToggles")).transform).parent = menu.transform;
        QMUtils.ResetTransform(transform);
        gameObject.AddComponent<LayoutElement>();

        (ObjectHolder = new GameObject("Holder").transform).parent = gameObject.transform;
        QMUtils.ResetTransform(ObjectHolder);
        ObjectHolder.localPosition = new Vector3(0f, -3f, 0f);

        (ButtonOne = new(ObjectHolder, buttonOne, buttonOneTooltip, () => btnAction.Invoke(this), true)).transform.localPosition = new Vector3(0f, 50f, 0f);
        (ButtonTwo = new(ObjectHolder, buttonTwo, buttonTwoTooltip, () => buttonTwoAction.Invoke(this), true)).transform.localPosition = new Vector3(0f, -51f, 0f);
    }

    public DuoButtons(ButtonGroupControl grp, string buttonOne, string buttonOneTooltip, Action btnAction, string buttonTwo, string buttonTwoTooltip, Action buttonTwoAction) :
        this(grp.GroupContents, buttonOne, buttonOneTooltip, (_) => btnAction(),
            buttonTwo, buttonTwoTooltip, (_) => buttonTwoAction())
    { }

    public DuoButtons(ButtonGroupControl grp, string buttonOne, string buttonOneTooltip, Action<DuoButtons> btnAction, string buttonTwo, string buttonTwoTooltip, Action<DuoButtons> buttonTwoAction) :
    this(grp.GroupContents, buttonOne, buttonOneTooltip, btnAction,
        buttonTwo, buttonTwoTooltip, buttonTwoAction) 
    { }
}

