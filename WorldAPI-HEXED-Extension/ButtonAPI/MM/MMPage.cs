using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRC.UI.Elements;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.MM;

public class MMPage : WorldPage {
    public int Pageint { get; private set; }

    private static bool Preped;

    private static void PrePrepMenu() {
        Preped = true;
        APIBase.MMMpageTemplate.GetComponent<UIPage>().Method_Public_Void_Boolean_TransitionType_0(true, UIPage.TransitionType.None); // open the menu so its made
        APIBase.MMMpageTemplate.GetComponent<Canvas>().enabled = false;
        APIBase.MMMpageTemplate.GetComponent<CanvasGroup>().enabled = false;
        APIBase.MMMpageTemplate.GetComponent<GraphicRaycaster>().enabled = false;

        VRCUiManager.field_Private_Static_VRCUiManager_0?.transform.Find("Canvas_MainMenu(Clone)/Container/PageButtons/HorizontalLayoutGroup/Page_Profile").GetComponent<Button>().onClick.AddListener(new System.Action(() => {
            APIBase.MMMpageTemplate.GetComponent<Canvas>().enabled = true;
            APIBase.MMMpageTemplate.GetComponent<CanvasGroup>().enabled = true;
            APIBase.MMMpageTemplate.GetComponent<GraphicRaycaster>().enabled = true;
        }));
    }

    public MMPage(string menuName, bool root = false) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        if (!Preped)
            PrePrepMenu();

        var region = 0;

        try {
            gameObject = Object.Instantiate(APIBase.MMMpageTemplate, APIBase.MMMpageTemplate.transform.parent);
            transform = gameObject.transform;
            gameObject.name = menuName;
            gameObject.transform.Find("Loading_Display").gameObject.active = false;
            MenuName = menuName;
            var ttext = gameObject.transform.Find("Header_MM_UserName/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();
            ttext.text = MenuName;
            ttext.richText = true;

            region++;
            Page = gameObject.GetComponent<UIPage>();
            var GuidName = menuName + Guid.NewGuid();
            Page.field_Public_String_0 = GuidName;
            (Page.field_Private_List_1_UIPage_0 = new()).Add(Page);
            region++;

            QMUtils.GetMainMenuStateControllerInstance.field_Private_Dictionary_2_String_UIPage_0.Add(GuidName, Page);
            var list = QMUtils.GetMainMenuStateControllerInstance.field_Public_Il2CppReferenceArray_1_UIPage_0.ToList();
            list.Add(Page);
            QMUtils.GetMainMenuStateControllerInstance.field_Public_Il2CppReferenceArray_1_UIPage_0 = list.ToArray();
            Pageint = QMUtils.GetMainMenuStateControllerInstance.field_Public_Il2CppReferenceArray_1_UIPage_0.Count;
            if (!root) transform.Find("Header_MM_UserName/LeftItemContainer/Button_Back").gameObject.active = true;

            region++;
            Page.GetComponent<Canvas>().enabled = true;
            Page.GetComponent<CanvasGroup>().enabled = true;
            Page.GetComponent<UIPage>().enabled = true;
            Page.GetComponent<GraphicRaycaster>().enabled = true;
            region++;

            Page.transform.Find("ScrollRect").GetComponent<VRC.UI.Elements.Controls.VRCScrollRect>().field_Public_Boolean_0 = true;
            (MenuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup")).DestroyChildren();

            gameObject.SetActive(false); // Set it off, as we had to enable the page comps, that shows the page, and it will be overlapping - but the controller fixes it when u select and deselect the menu
        } catch (Exception ex) {
            throw new Exception("Exception Caught When Making Page At Region: " + region + "\n\n" + ex);
        }
    }

    public void OpenMenu() {
        gameObject.SetActive(true);
        QMUtils.GetMainMenuStateControllerInstance.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(Page.field_Public_String_0, null, true, UIPage.TransitionType.Right);
        OnMenuOpen?.Invoke();
    }
}