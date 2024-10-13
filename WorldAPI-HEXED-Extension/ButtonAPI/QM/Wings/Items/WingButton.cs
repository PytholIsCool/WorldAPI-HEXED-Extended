using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Localization;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;
using VRC.UI;
using WorldAPI.ButtonAPI.QM.Wings.Controls;
using TMPro;
using UnityEngine.Experimental.U2D;
using UnityEngine.UI;

namespace WorldAPI.ButtonAPI.QM.Wings.Items;

public class WingButton : WingRoot {
    VRCButtonHandle bHan;
    public WingButton(string text, string tooltip, Action listener, Sprite icon = null) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (transform = Object.Instantiate(APIBase.WingButtonRoot, APIBase.WingButtonRoot.transform.parent).transform).name = $"Button_{text}_{Guid.NewGuid()}";

        transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>().text = text;

        if (icon != null)
            transform.Find("Container/Icon").GetComponent<Image>().overrideSprite = icon;
        else
            transform.Find("Container/Icon").gameObject.SetActive(false);

        (bHan = transform.GetComponent<VRCButtonHandle>()).onClick = new();
        bHan.onClick.AddListener(listener);

        transform.GetComponent<ToolTip>()._localizableString = tooltip.Localize();
    }
    public WingButton(Transform parent, string text, string tooltip, Action listener, Sprite icon = null, string headerText = null, bool SubMenuIcon = false) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (transform = Object.Instantiate(APIBase.WingButton, parent).transform).name = text;

        (TMProCompnt = transform.Find("Text_Avatar").GetComponent<TextMeshProUGUI>()).text = text;

        if (headerText == null) {
            transform.Find("Text_Header").gameObject.SetActive(false);
            transform.Find("Text_Avatar").localPosition = new Vector3(-82f, 0f, 0f);
        }
        else
            transform.Find("Text_Header").GetComponent<TextMeshProUGUI>().text = headerText;

        (bHan = transform.GetComponent<VRCButtonHandle>()).onClick = new();
        bHan.onClick.AddListener(listener);

        if (icon != null)
            transform.Find("Icon_Avatar").GetComponent<Image>().overrideSprite = icon;
        else
            transform.Find("Icon_Avatar").gameObject.SetActive(false);

        if (SubMenuIcon)
            transform.Find("Icon_JumpToMM").gameObject.SetActive(true);

        transform.GetComponent<ToolTip>()._localizableString = tooltip.Localize();
    }
}