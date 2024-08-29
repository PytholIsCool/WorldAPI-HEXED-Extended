using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.MM.Carousel;

public class CMenu : Root
{
    internal List<GameObject> ChlidrenObjects { get; set; } = new(); // This prlly isn't the best way to do this
    public Action OnClick { get; set; }

    public Button Button { get; private set; }
    public Image ImageComp { get; private set; }
    public MMCarousel RootMenu { get; private set; }
    public VRC.UI.Elements.Controls.ToolTip ToolTip { get; private set; }


    public CMenu(Transform transform, Action onClick, string buttonText, string toolTip = "", Sprite Icon = null) {
        if (!APIBase.IsReady())
            throw new Exception();
        if (Icon == null)
            Icon = APIBase.DefaultButtonSprite;

        transform = (gameObject = Object.Instantiate(APIBase.MMMCarouselButtonTemplate, transform)).transform;
        gameObject.name = buttonText;

        TMProCompnt = gameObject.transform.Find("Mask/Text_Name").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = buttonText;
        TMProCompnt.richText = true;

        if (onClick != null)
            OnClick += onClick;

        ((Button = gameObject.GetComponent<Button>()).onClick = new()).AddListener(new Action(()=>OnClick?.Invoke())); // New action so it can change during runtime

        ImageComp = gameObject.transform.Find("Icon").GetComponent<Image>();
        if (Icon != null)
            ImageComp.sprite = Icon;
        else ImageComp.gameObject.SetActive(false);

        (ToolTip = gameObject.GetComponent<VRC.UI.Elements.Controls.ToolTip>())._localizableString = toolTip.Localize();
    }

    public CMenu(MMCarousel ph, string buttonText, string toolTip = "", Sprite Icon = null) : this(ph.BarContents, null, buttonText, toolTip, Icon) {
        OnClick += new Action(() => { // Once more, theres Prlly a better way to do this
            ph.MenuContents.GetChildren().ForEach(a => a.SetActive(false));
            ChlidrenObjects.ForEach(a => a.SetActive(true));
        });
        RootMenu = ph;
    }
}