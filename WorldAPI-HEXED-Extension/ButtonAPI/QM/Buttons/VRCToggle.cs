using System;
using TMPro;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Buttons;

public class VRCToggle : ToggleControl
{
    private static Transform ToggleTemplate;

    public VRCToggle(Transform menu, string text, Action<bool, VRCToggle> listener, bool DefaultState = false, string OffTooltip = null,
        string OnToolTip = null, Sprite onimage = null, Sprite offimage = null, bool half = false)
    {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");
        OffTooltip ??= $"Turn On {text.Replace("\n", string.Empty)}";
        OnToolTip ??= $"Turn Off {text.Replace("\n", string.Empty)}";

        if (ToggleTemplate == null)
        {
            var btn = new VRCButton(APIBase.ButtonGrp.transform, text, string.Empty, () => { });
            ToggleTemplate = btn.transform;

            Object.DestroyImmediate(ToggleTemplate.GetComponent<Button>());
            Object.DestroyImmediate(ToggleTemplate.Find("Icons/Icon_Secondary"));
            Object.DestroyImmediate(ToggleTemplate.Find("Badge_Close"));
            Object.DestroyImmediate(ToggleTemplate.Find("Badge_MMJump"));
            ToggleTemplate.gameObject.AddComponent<Toggle>();
            var defaultImageObj = btn.ImgCompnt.transform;
            defaultImageObj.name = "Icon_Off";
            var onImge = Object.Instantiate(defaultImageObj, defaultImageObj.parent);
            onImge.name = "Icon_On";
            defaultImageObj.gameObject.active = true;
            onImge.gameObject.active = true;
            defaultImageObj.transform.localScale = new Vector3(.7f, .7f, .7f);
            ToggleTemplate.gameObject.SetActive(false);
        }

        gameObject = (transform = Object.Instantiate(ToggleTemplate, menu)).gameObject;
        gameObject.SetActive(true);
        gameObject.name = text;

        ToggleCompnt = transform.GetOrAddComponent<Toggle>();
        TMProCompnt = transform.GetComponentInChildren<TextMeshProUGUI>();
        Text = text;
        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        OnImage = gameObject.transform.Find("Icons/Icon_On").GetComponent<Image>();
        OnImage.overrideSprite = APIBase.OnSprite;
        OffImage = gameObject.transform.Find("Icons/Icon_Off").GetComponent<Image>();
        OffImage.overrideSprite = APIBase.OffSprite;
        OffImage.transform.localPosition = new Vector3(-46f, 0, 0);
        OnImage.transform.localPosition = new Vector3(49, 0, 0);

        Listener = listener;
        SoftSetState(DefaultState);
        SetImages(true, onimage, offimage);
        SetToolTip(DefaultState ? OnToolTip : OffTooltip);
        if (half) TurnHalf();

        inst = this;
    }

    public VRCToggle RSetActive(bool val)
    {
        SetActive(val);
        return this;
    }

    public VRCToggle(ButtonGroupControl buttonGroup, string text, Action<bool, VRCToggle> stateChanged, bool DefaultState = false, string OffTooltip = "Off",
        string OnToolTip = "On", Sprite onimage = null, Sprite offimage = null, bool half = false)
        : this(buttonGroup.GroupContents.transform, text, stateChanged, DefaultState, OffTooltip, OnToolTip, onimage, offimage, half) => buttonGroup._toggles.Add(this);

    public VRCToggle(ButtonGroupControl buttonGroup, string text, Action<bool> stateChanged, bool DefaultState = false, string OffTooltip = "Off",
        string OnToolTip = "On", Sprite onimage = null, Sprite offimage = null, bool half = false)
        : this(buttonGroup, text, (val, _) => stateChanged(val), DefaultState, OffTooltip, OnToolTip, onimage, offimage, half) { }

    public VRCToggle(Transform menu, string text, Action<bool> stateChanged, bool DefaultState = false, string OffTooltip = "Off",
        string OnToolTip = "On", Sprite onimage = null, Sprite offimage = null, bool half = false)
        : this(menu, text, (val, _) => stateChanged(val), DefaultState, OffTooltip, OnToolTip, onimage, offimage, half) { }

}