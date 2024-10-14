using WorldAPI.ButtonAPI.QM.Wings.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Controls;
using Object = UnityEngine.Object;
using WorldAPI.ButtonAPI.QM.Wings.Items;
using Cysharp.Threading.Tasks.Linq;

namespace WorldAPI.ButtonAPI.QM.Wings;

public class WingPage : PyWingPage {
    public WingSide wingSide { get; set; }
    public UIPage page { get; set; }
    private static readonly string LPar = "CanvasGroup/Container/Window/Wing_Left/Container/InnerContainer/WingMenu";
    private static readonly string RPar = "CanvasGroup/Container/Window/Wing_Right/Container/InnerContainer/WingMenu";

    public WingPage(string pageName, WingSide WingSide) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        wingSide = WingSide; //setting wing side

        (transform = (gameObject = Object.Instantiate(APIBase.WingPage, APIBase.WingPage.transform.parent)).transform).name = $"{pageName}_{Guid.NewGuid()}";

        (page = gameObject.GetComponent<UIPage>()).field_Public_String_0 = gameObject.name;
        page.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
        page.field_Private_List_1_UIPage_0.Add(page); //adding the page so it can be used

        (PageContents = transform.Find("ScrollRect/Viewport/VerticalLayoutGroup")).GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true; ;
        PageContents.DestroyChildren(); //getting rid of the children so we may add our own

        TextMeshProUGUI Header = gameObject.transform.Find("WngHeader_H1/LeftItemContainer/Text_QM_H2 (1)").GetComponent<TextMeshProUGUIEx>();
        Header.text = pageName;
        Header.richText = true; //setting the header's text

        Transform BackButton = transform.Find("WngHeader_H1/LeftItemContainer/Button_Back");
        BackButton.GetComponent<Button>().onClick.AddListener(new Action(() => {
            if (wingSide == WingSide.Left) {
                QMUtils.GetWngLMenuStateControllerInstance.Method_Public_Void_PDM_2();
                page.Method_Protected_Void_Boolean_TransitionType_0(false, UIPage.TransitionType.Right);
            }
            else {
                QMUtils.GetWngRMenuStateControllerInstance.Method_Public_Void_PDM_2();
                page.Method_Protected_Void_Boolean_TransitionType_0(false, UIPage.TransitionType.Left);
            }
        }));

        if (wingSide == WingSide.Left) QMUtils.GetWngLMenuStateControllerInstance.field_Private_Dictionary_2_String_UIPage_0.Add(pageName + Guid.NewGuid(), page);
        else QMUtils.GetWngRMenuStateControllerInstance.field_Private_Dictionary_2_String_UIPage_0.Add(pageName + Guid.NewGuid(), page);

        page.transform.Find("ScrollRect").GetComponent<VRCScrollRect>().field_Public_Boolean_0 = true;
        page.GetComponent<Canvas>().enabled = true;
        page.GetComponent<CanvasGroup>().enabled = true;
        page.GetComponent<UIPage>().enabled = true;
        page.GetComponent<GraphicRaycaster>().enabled = true;

        gameObject.SetActive(false);
    }

    public void OpenMenu() {
        if (wingSide == WingSide.Left) {
            APIBase.QuickMenu?.transform.Find(LPar).gameObject.SetActive(false);
            page.gameObject.SetActive(true);
            
            page.Method_Protected_Void_Boolean_TransitionType_0(true, UIPage.TransitionType.Right);
        }
        else {
            APIBase.QuickMenu?.transform.Find(RPar).gameObject.SetActive(false);
            page.gameObject.SetActive(true);
            
            page.Method_Protected_Void_Boolean_TransitionType_0(true, UIPage.TransitionType.Left);
        }
    }

    public WingButton AddButton(string text, string tooltip, Action listener, Sprite icon = null, string headerText = null, bool SubMenuIcon = false) {
        return new WingButton(this.PageContents, text, tooltip, listener, icon, headerText, SubMenuIcon);
    }
}