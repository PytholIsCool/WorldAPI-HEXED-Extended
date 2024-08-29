using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Buttons;

public class VRCButton : ExtentedControl
{
    public VRCButton(Transform menu, string text, string tooltip, Action<VRCButton> listener, bool Half = false,
        bool SubMenuIcon = false, Sprite Icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
    {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");
        Icon ??= APIBase.DefaultButtonSprite;

        (transform = Object.Instantiate(APIBase.Button, menu)).name = $"Button_{text}";
        gameObject = transform.gameObject;
        gameObject.SetActive(true);

        TMProCompnt = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (TMProCompnt == null)
            throw new NullReferenceException("Unable to grab Text Object");

        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        Text = text;

        ButtonCompnt = transform.GetComponent<Button>();
        if (ButtonCompnt == null)
            throw new NullReferenceException("Unable to grab Button Componet");

        ButtonCompnt.onClick = new();
        if (listener != null) SetAction(listener);
        else ButtonCompnt.interactable = false;


        ImgCompnt = transform.transform.Find("Icons/Icon").GetComponent<Image>();
        //var elemetn = ImgCompnt.gameObject.GetComponent<StyleElement>();
        //if (elemetn != null) elemetn.enabled = false; Set as off to fix styling

        if (ImgCompnt.color == Color.black)
            ImgCompnt.color = Color.white;

        Object.Destroy(transform.transform.Find("Icons/Icon_Secondary").gameObject);
        if (Icon != null)
            SetSprite(Icon);
        else
        {
            transform.transform.Find("Icons/Icon").gameObject.active = false;
            ResetTextPox();
        }

        ShowSubMenuIcon(SubMenuIcon);
        this.SetToolTip(tooltip);
        if (Half) TurnHalf(Type, IsGroup);
        inst = this;
    }

    public VRCButton(Transform menu, string text, string tooltip, Action click, bool Half = false, bool subMenuIcon = false, Sprite icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
        : this(menu, text, tooltip, (_) => click(), Half, subMenuIcon, icon, Type, IsGroup) { }

    public VRCButton(ButtonGroupControl buttonGroup, string text, string tooltip, Action click, bool Half = false, bool subMenuIcon = false, Sprite icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
        : this(buttonGroup, text, tooltip, (_) => click(), Half, subMenuIcon, icon, Type, IsGroup) { }

    public VRCButton(ButtonGroupControl buttonGroup, string text, string tooltip, Action<VRCButton> click, bool Half = false, bool subMenuIcon = false, Sprite icon = null, HalfType Type = HalfType.Normal, bool IsGroup = false)
        : this(buttonGroup.GroupContents.transform, text, tooltip, click, Half, subMenuIcon, icon, Type, IsGroup) => buttonGroup._buttons.Add(this);
}
