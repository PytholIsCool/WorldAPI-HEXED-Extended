using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Buttons;

namespace WorldAPI.ButtonAPI.Controls;

public class ToggleControl : Root {
    public bool IsHalf { get; internal set; }
    public Image OnImage { get; internal set; }
    public Image OffImage { get; internal set; }
    public Toggle ToggleCompnt { get; internal set; }
    public VRC.UI.Elements.Tooltips.UiToggleTooltip TipComp { get; internal set; }

    internal VRCToggle inst;
    internal Action<bool, VRCToggle> Listener { get; set; }

    public bool State
    {
        get => ToggleCompnt.isOn;
        set => ToggleCompnt.isOn = value;
    }

    public void SetAction(Action<bool> newAction) => Listener = (val, _) => newAction(val);
    public void SetAction(Action<bool, VRCToggle> newAction) => Listener = newAction;

    public void SoftSetState(bool value) {
        ToggleCompnt.onValueChanged = new();
        State = value;
        ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) => {
            APIBase.SafelyInvolk(val, (va) => Listener.Invoke(va, inst), Text);
            APIBase.Events.onVRCToggleValChange?.Invoke(inst, val);
            OnImage.color = new(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 1 : 0.17f);
            OffImage.color = new(OnImage.color.r, OnImage.color.g, OnImage.color.b, val ? 0.17f : 1);
            if (IsHalf) {
                OffImage.gameObject.active = !val;
                OnImage.gameObject.active = val;
            }
        }));
        OnImage.color = new(OnImage.color.r, OnImage.color.g, OnImage.color.b, value ? 1 : 0.17f);
        OffImage.color = new(OnImage.color.r, OnImage.color.g, OnImage.color.b, value ? 0.17f : 1);
    }

    public (Sprite, Sprite) SetImages(Sprite onSprite = null, Sprite offSprite = null) {
        OffImage.sprite = offSprite;
        OnImage.sprite = onSprite;
        return (onSprite, offSprite);
    }

    public void SetInteractable(bool val) => ToggleCompnt.interactable = val;

    public (Sprite, Sprite) SetImages(bool checkForNull, Sprite onSprite = null, Sprite offSprite = null) {
        if (checkForNull) {
            onSprite ??= APIBase.OnSprite;
            offSprite ??= APIBase.OffSprite;
        }
        if (offSprite != null) OffImage.sprite = offSprite;
        if (onSprite != null) OnImage.sprite = onSprite;
        return (onSprite, offSprite);
    }

    public void TurnHalf(Vector3 TogglePoz, float FontSize = 24f) {
        gameObject.transform.localPosition = TogglePoz;
        TurnHalf(FontSize);
    }

    public void TurnHalf(float FontSize = 24f) {
        if (IsHalf) throw new Exception("Toggle is ALREADY half!");
        IsHalf = true;

        OnImage.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        OnImage.transform.localPosition = new Vector3(-52, -5, 0f);
        OnImage.gameObject.SetActive(State);

        OffImage.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        OffImage.transform.localPosition = new Vector3(-52, -5, 0f);
        OffImage.gameObject.SetActive(!State);

        TMProCompnt.fontSize = FontSize;
        TMProCompnt.transform.localPosition = new Vector3(34.5f, 33, 0);
        TMProCompnt.transform.parent.GetComponent<LayoutElement>().enabled = false;
        TMProCompnt.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 50f);


        gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -80);

        Listener += (val, _) => { // Adding Listener, so we dont have to reset it
            OffImage.gameObject.active = !val;
            OnImage.gameObject.active = val;
        };
    }
}
