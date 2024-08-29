using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;

namespace WorldAPI.ButtonAPI.Buttons;

public class VRCLable : Root {
    public Button ButtonCompnt { get; internal set; }

    public readonly VRCButton SButton;
    public readonly TextMeshProUGUI LowerTextUgui;

    public VRCLable(Transform menu, string text, string LowerText, Action onClick = null, bool Bg = true) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        gameObject = (SButton = new VRCButton(menu, text, null, onClick)).gameObject;
        ButtonCompnt = SButton.ButtonCompnt;

        SButton.ImgCompnt.enabled = false;

        TMProCompnt = SButton.TMProCompnt;
        TMProCompnt.richText = true;
        TMProCompnt.transform.localPosition = new Vector3(0f, 2f, 0f);
        TMProCompnt.fontSize = 38f;
        TMProCompnt.enableAutoSizing = true;

        var Text2 = UnityEngine.Object.Instantiate(SButton.TMProCompnt.gameObject, new Vector3(0, -54.75f, 0), Quaternion.Euler(0, 0, 0), SButton.transform);
        Text2.transform.localPosition = new Vector3(0, -54.75f, 0);
        Text2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        LowerTextUgui = Text2.GetComponent<TextMeshProUGUI>();
        LowerTextUgui.text = LowerText;

        Text = text;
    }

    public VRCLable(ButtonGroupControl grp, string text, string LowerText, Action onClick = null, bool Bg = false)
        : this(grp.GroupContents.transform, text, LowerText, onClick, Bg)
    { }
}
