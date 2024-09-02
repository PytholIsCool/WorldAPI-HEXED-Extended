using System;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Buttons;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Controls;

public class ExtentedControl : Root
{
    public Button ButtonCompnt { get; internal set; }
    public Image ImgCompnt { get; internal set; }
    public Action onClickAction { get; set; }
    public string ToolTip { get; internal set; }

    internal VRCButton inst;

    public void SetSprite(Sprite sprite) => ImgCompnt.overrideSprite = sprite;
    public Sprite GetSprite() => ImgCompnt.sprite;
    public void ShowSubMenuIcon(bool state) => gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(state);
    public void SetIconColor(Color color) => ImgCompnt.color = color;

    public override string SetToolTip(string tip)
    {
        ToolTip = tip;
        return base.SetToolTip(tip);
    }

    public void SetAction(Action newAction) => SetAction((_) => newAction());

    public void SetAction(Action<VRCButton> newAction)
    {
        ButtonCompnt.onClick = new();
        onClickAction = () => newAction.Invoke(inst);
        ButtonCompnt.onClick.AddListener(new Action(() => APIBase.SafelyInvolk(onClickAction, Text)));
    }

    public void SetBackgroundImage(Sprite newImg)
    {
        gameObject.transform.Find("Background").GetComponent<Image>().sprite = newImg;
        gameObject.transform.Find("Background").GetComponent<Image>().overrideSprite = newImg;
    }


    internal void ResetTextPox() => TMProCompnt.transform.localPosition = new Vector3(0, 0, 0);

    public void TurnHalf(HalfType Type, bool IsGroup)
    {
        ImgCompnt.gameObject.active = false;
        TMProCompnt.fontSize = 22;

        var Jmp = gameObject.transform.Find("Badge_MMJump");
        Jmp.localPosition = new Vector3(Jmp.localPosition.x, Jmp.localPosition.y - 34, Jmp.localPosition.z);

        if (IsGroup) gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -115);
        else gameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -80);

        switch (Type)
        {
            case HalfType.Top:
                ImgCompnt.transform.localPosition = new Vector3(0f, 0f, 0f);
                TMProCompnt.transform.localPosition = new Vector3(0, 90, 0);
                gameObject.transform.Find("Background").localPosition = new Vector3(0, 53, 0);
                break;
            case HalfType.Normal:
                ImgCompnt.transform.localPosition = new Vector3(0f, 0f, 0f);
                TMProCompnt.transform.localPosition = new Vector3(0, 43, 0);
                break;
            case HalfType.Bottom:
                ImgCompnt.transform.localPosition = new Vector3(0f, 0f, 0f);
                TMProCompnt.transform.localPosition = new Vector3(0, -5, 0);
                gameObject.transform.Find("Background").localPosition = new Vector3(0, -53, 0);
                break;
        }

    }

    public enum HalfType
    {
        Top,
        Normal,
        Bottom
    }
}
