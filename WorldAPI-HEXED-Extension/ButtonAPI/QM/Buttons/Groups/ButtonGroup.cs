using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.QM.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.Groups;

public class ButtonGroup : ButtonGroupControl {
    private readonly GridLayoutGroup Layout;

    public ButtonGroup(Transform parent, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        if (!(WasNoText = NoText)) {
            headerGameObject = Object.Instantiate(APIBase.ButtonGrpText, parent);
            TMProCompnt = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerGameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;
        }

        gameObject = Object.Instantiate(APIBase.ButtonGrp, parent);
        gameObject.name = text;
        gameObject.transform.DestroyChildren();
        GroupContents = gameObject;
        transform = gameObject.transform;

        Layout = gameObject.GetComponent<GridLayoutGroup>();
        Layout.childAlignment = ButtonAlignment;

        parentMenuMask = parent.parent.GetComponent<RectMask2D>();
    }

    public void ChangeChildAlignment(TextAnchor ButtonAlignment = TextAnchor.UpperCenter) => Layout.childAlignment = ButtonAlignment;

    public ButtonGroup(WorldPage page, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter) : this(page.MenuContents, text, NoText, ButtonAlignment)
        { }
}
