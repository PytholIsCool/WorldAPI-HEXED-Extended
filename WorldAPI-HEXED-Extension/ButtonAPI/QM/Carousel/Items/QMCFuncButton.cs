using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.Groups;
using Object = UnityEngine.Object;
using Console = Serpentine.PyLog.Console;
using UnityEngine.Playables;
using WorldAPI.ButtonAPI.Buttons;
using VRC.UI.Element;
using WorldAPI.ButtonAPI.QM.Controls;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items;
public class QMCFuncButton : QMCControl {
    public Action<bool> Listener { get; set; }
    public bool isToggled { get; private set; }
    public Image ToggleSprite;
    public Transform ButtonParent { get; private set; }
    public static Transform leftPar { get; private set; }
    public static Transform rightPar { get; private set; }
    public Sprite OnSprite { get; private set; }
    public Sprite OffSprite { get; private set; }
    private bool shouldInvoke = true;
    private Image Tsprite;
    public QMCFuncButton(Transform parent, string text, string tooltip, Action listener, bool rightContainer = false, bool separator = false, Sprite sprite = null) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (transform = (gameObject = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate, parent)).transform).name = $"{text}_ControlContainer";

        (leftPar = transform.Find("LeftItemContainer")).gameObject.DestroyChildren();
        (rightPar = transform.Find("RightItemContainer")).gameObject.DestroyChildren();

        ButtonParent = rightContainer ? rightPar : leftPar;

        transform.Find("TitleMainContainer").gameObject.SetActive(false);

        Transform button = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        button.name = text;

        (TMProCompnt = button.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;

        if (sprite != null) {
            (Tsprite = button.Find("Icon").GetComponent<Image>()).overrideSprite = sprite;
            Tsprite.gameObject.SetActive(true);
        }

        if (separator != false)
            AddSeparator(parent);

        button.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        (ButtonCompnt = button.GetComponent<Button>()).onClick = new();
        ButtonCompnt.onClick.AddListener(listener);

        button.gameObject.SetActive(true);
    }
    public QMCFuncButton AddButton(string text, string tooltip, Action listener, bool rightContainer = false, Sprite sprite = null) {
        ButtonParent = rightContainer ? rightPar : leftPar;

        Transform newButton = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        newButton.name = text;

        if (sprite != null) {
            var Tsprite = newButton.Find("Icon").GetComponent<Image>();
            Tsprite.overrideSprite = sprite;
            Tsprite.gameObject.SetActive(true);
        }

        (TMProCompnt = newButton.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;

        newButton.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        (ButtonCompnt = newButton.GetComponent<Button>()).onClick = new();
        ButtonCompnt.onClick.AddListener(listener);

        newButton.gameObject.SetActive(true);

        return this;
    }
    public QMCFuncButton AddToggle(string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, Sprite onSprite = null, Sprite offSprite = null) {
        ButtonParent = rightContainer ? rightPar : leftPar;

        Transform newToggle = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        newToggle.name = $"{text}_FunctionToggle";

        (OnImageObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle)).name = "OnIcon";
        OnImageObj.GetComponent<Image>().sprite = onSprite ?? APIBase.OnSprite;

        (OffImageObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle)).name = "OffIcon";
        OffImageObj.GetComponent<Image>().sprite = offSprite ?? APIBase.OffSprite;

        newToggle.Find("Icon").gameObject.SetActive(false);

        (TMProCompnt = newToggle.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;

        newToggle.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        bool isToggledLocal = defaultState;
        OnImageObj.SetActive(isToggledLocal);
        OffImageObj.SetActive(!isToggledLocal);

        Button buttonComponent = newToggle.GetComponent<Button>();
        buttonComponent.onClick = new();
        buttonComponent.onClick.AddListener(new Action(() => {
            isToggledLocal = !isToggledLocal;

            OnImageObj.SetActive(isToggledLocal);
            OffImageObj.SetActive(!isToggledLocal);

            if (shouldInvoke) 
                APIBase.SafelyInvolk(isToggledLocal, listener, text);
        }));

        newToggle.gameObject.SetActive(true);

        return this;
    }

    public void SoftSetState(bool state) {
        if (isToggled != state) {
            isToggled = state;
            ToggleSprite.overrideSprite = isToggled ? OnSprite : OffSprite;

            if (shouldInvoke) 
                APIBase.SafelyInvolk(isToggled, Listener, "SoftSet");
        }
    }

    public QMCFuncButton(QMCGroup group, string text, string tooltip, Action listener, bool rightContainer = false, bool separator = false, Sprite sprite = null)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, tooltip, listener, rightContainer, separator, sprite) { }
    public QMCFuncButton(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action listener, bool rightContainer = false, bool separator = false, Sprite sprite = null)
        : this(buttonGroup.QMCParent, text, tooltip, listener, rightContainer, separator, sprite) { }
}

