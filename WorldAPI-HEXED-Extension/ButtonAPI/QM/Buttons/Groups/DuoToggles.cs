using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;

namespace WorldAPI.ButtonAPI.Buttons.Groups;

public class DuoToggles : Root {
    public Transform ObjectHolder { get; private set; }

    public VRCToggle ToggleOne { get; private set; }
    public VRCToggle ToggleTwo { get; private set; }

    public DuoToggles(GameObject menu, string text, string Ontooltip, string OffTooltip, Action<bool> BoolStateChange, 
        string text2, string Ontooltip2, string OffTooltip2, Action<bool> BoolStateChange2, 
        Sprite OnImageSprite = null, Sprite OffImageSprite = null,
        float FirstFontSize = 24f, float SecondFontSize = 24f, bool FirstState = false, bool SecondState = false)
    {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        (transform = (gameObject = new("Button_DuoToggles")).transform).parent = menu.transform;
        QMUtils.ResetTransform(transform);
        gameObject.AddComponent<LayoutElement>();

        (ObjectHolder = new GameObject("Holder").transform).parent = gameObject.transform;
        QMUtils.ResetTransform(ObjectHolder);
        ObjectHolder.localPosition = new Vector3(0f, -3f, 0f);

        (ToggleOne = new VRCToggle(ObjectHolder, text, BoolStateChange, FirstState, Ontooltip, OffTooltip, OnImageSprite, OffImageSprite))
            .TurnHalf(new Vector3(0f, 50f, 0f), FirstFontSize);
        (ToggleTwo = new VRCToggle(ObjectHolder, text2, BoolStateChange2, SecondState, Ontooltip2, OffTooltip2, OnImageSprite, OffImageSprite))
            .TurnHalf(new Vector3(0f, -51f, 0f), SecondFontSize);
    }

    public DuoToggles(ButtonGroupControl btnGrp, string text, string Ontooltip, string OffTooltip, Action<bool> BoolStateChange,
        string text2, string Ontooltip2, string OffTooltip2, Action<bool> BoolStateChange2,
        Sprite OnImageSprite = null, Sprite OffImageSprite = null,
        float FirstFontSize = 24f, float SecondFontSize = 24f, bool FirstState = false, bool SecondState = false) :
        this(btnGrp.GroupContents, text, Ontooltip, OffTooltip, BoolStateChange, 
            text2, Ontooltip2, OffTooltip2, BoolStateChange2,
            OnImageSprite, OffImageSprite, FirstFontSize, SecondFontSize, FirstState, SecondState)
    { }
}
