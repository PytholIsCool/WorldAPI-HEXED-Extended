using System;
using TMPro;
using UnityEngine.UI;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.MM.Carousel.Items;

public class CGrp : WorldPage
{
    public Toggle Togl { get; private set; }
    public bool IsOpen { get; private set; }

    public CGrp(CMenu menu, string text, bool expandable = true, bool defaultState = true)
    {
        if (!APIBase.IsReady()) throw new Exception();

        transform = Object.Instantiate(APIBase.MMBtnGRP, menu.RootMenu.MenuContents).transform;
        transform.name = "BtnGrp_" + text;
        gameObject = transform.gameObject;

        (MenuContents = transform.Find("Settings_Panel_1/VerticalLayoutGroup"))
            .DestroyChildren(a => a.name != "Background_Info");

        TMProCompnt = transform.Find("MM_Foldout/Label").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        Togl = transform.Find("MM_Foldout/Background_Button").GetComponent<Toggle>();
        Togl.onValueChanged = new();
        Togl.isOn = defaultState;
        Togl.onValueChanged.AddListener(new Action<bool>(val => {
            MenuContents.gameObject.SetActive(val);
            IsOpen = val;
            OnMenuOpen?.Invoke();
        }));
        Togl.gameObject.active = expandable;

        menu.ChlidrenObjects.Add(transform.gameObject);
        MenuName = text;
    }
}