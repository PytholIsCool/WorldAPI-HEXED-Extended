using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;


namespace WorldAPI.ButtonAPI.MM;

public class MMTab : Root {
    public static Action OnClick { get; set; }

    public Image Image { get; private set; }
    public VRC.UI.Elements.Controls.ToolTip ToolTip { get; private set; }
    public Button MenuTab { get; private set; }

    private void Make(Action method, string toolTip, Sprite sprite) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search had FAILED!");

        gameObject = Object.Instantiate(APIBase.MMMTabTemplate, APIBase.MMMTabTemplate.transform.parent);
        if (sprite != null) (Image = gameObject.transform.Find("Icon").GetComponent<Image>()).sprite = sprite;
        else gameObject.transform.Find("Icon").gameObject.active = false;

        gameObject.GetComponent<StyleElement>().field_Private_Selectable_0 = gameObject.GetComponent<Button>();
        gameObject.GetComponent<Button>().onClick.AddListener(new Action(() => gameObject.SetActive(true)));

        (ToolTip = gameObject.GetComponent<VRC.UI.Elements.Controls.ToolTip>())._localizableString = toolTip.Localize();
        ((MenuTab = gameObject.GetComponent<Button>()).onClick = new()).AddListener(method);
    }

    public MMTab(MMPage page, string toolTip = "", Sprite sprite = null) => Make(page.OpenMenu, toolTip, sprite);
    public MMTab(MMCarousel page, string toolTip = "", Sprite sprite = null) => Make(page.OpenMenu, toolTip, sprite);
}
