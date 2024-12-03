using WorldAPI.ButtonAPI.QM.Controls;
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

namespace WorldAPI.ButtonAPI.QM.Carousel.Items;

public class QMCFuncToggle : QMCControl {
    public Action<bool> Listener { get; set; }
    public bool isToggled { get; private set; }
    public Image ToggleSprite;
    public Transform ButtonParent { get; private set; }
    public static Transform leftPar { get; private set; }
    public static Transform rightPar { get; private set; }
    public Sprite OnSprite { get; private set; }
    public Sprite OffSprite { get; private set; }
    private bool shouldInvoke = true;
    public QMCFuncToggle(Transform parent, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (base.transform = (gameObject = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate, parent)).transform).name = $"{text}_ToggleControlContainer";

        (leftPar = base.transform.Find("LeftItemContainer")).gameObject.DestroyChildren();
        (rightPar = base.transform.Find("RightItemContainer")).gameObject.DestroyChildren();

        ButtonParent = rightContainer ? rightPar : leftPar;

        base.transform.Find("TitleMainContainer").gameObject.SetActive(false);

        Transform newToggle = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        newToggle.name = text + "_FunctionToggle";

        (OnImageObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle)).name = "OnIcon";
        OnImageObj.GetComponent<Image>().sprite = onSprite ?? APIBase.OnSprite;

        (OffImageObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle)).name = "OffIcon";
        OffImageObj.GetComponent<Image>().sprite = offSprite ?? APIBase.OffSprite;

        newToggle.Find("Icon").gameObject.SetActive(false);

        TMProCompnt = newToggle.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        if (separator != false)
            AddSeparator(parent);

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

            if (shouldInvoke) {
                APIBase.SafelyInvolk(isToggledLocal, listener, text);
            }
        }));

        newToggle.gameObject.SetActive(true);
    }
    public QMCFuncToggle AddButton(string text, string tooltip, Action listener, bool rightContainer = false, Sprite sprite = null) {
        ButtonParent = rightContainer ? rightPar : leftPar;

        transform = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        transform.name = text;

        if (sprite != null) {
            var Tsprite = transform.Find("Icon").GetComponent<Image>();
            Tsprite.overrideSprite = sprite;
            Tsprite.gameObject.SetActive(true);
        }

        TMProCompnt = transform.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        transform.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        ButtonCompnt = transform.GetComponent<Button>();
        ButtonCompnt.onClick = new();
        ButtonCompnt.onClick.AddListener(listener);

        transform.gameObject.SetActive(true);

        return this;
    }
    public QMCFuncToggle AddToggle(string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, Sprite onSprite = null, Sprite offSprite = null) {
        ButtonParent = rightContainer ? rightPar : leftPar;

        transform = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
        transform.name = text + "_FunctionToggle";

        GameObject OnIconObj = Object.Instantiate(transform.Find("Icon").gameObject, transform);
        OnIconObj.name = "OnIcon";
        OnIconObj.GetComponent<Image>().sprite = onSprite ?? APIBase.OnSprite;

        GameObject OffIconObj = Object.Instantiate(transform.Find("Icon").gameObject, transform);
        OffIconObj.name = "OffIcon";
        OffIconObj.GetComponent<Image>().sprite = offSprite ?? APIBase.OffSprite;

        transform.Find("Icon").gameObject.SetActive(false);

        TMProCompnt = transform.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
        TMProCompnt.text = text;
        TMProCompnt.richText = true;

        transform.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        bool isToggledLocal = defaultState;
        OnIconObj.SetActive(isToggledLocal);
        OffIconObj.SetActive(!isToggledLocal);

        Button buttonComponent = transform.GetComponent<Button>();
        buttonComponent.onClick = new();
        buttonComponent.onClick.AddListener(new Action(() => {
            isToggledLocal = !isToggledLocal;
            OnIconObj.SetActive(isToggledLocal);
            OffIconObj.SetActive(!isToggledLocal);

            if (shouldInvoke)
                APIBase.SafelyInvolk(isToggledLocal, listener, text);
        }));

        transform.gameObject.SetActive(true);

        return this;
    }

    public void SoftSetState(bool state)
    {
        if (isToggled != state) {
            isToggled = state;
            ToggleSprite.overrideSprite = isToggled ? OnSprite : OffSprite;

            if (shouldInvoke)
                APIBase.SafelyInvolk(isToggled, Listener, "SoftSet");
        }
    }
    public QMCFuncToggle(QMCGroup group, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, listener, tooltip, rightContainer, defaultState, separator, onSprite, offSprite) { }
    public QMCFuncToggle(CollapsibleButtonGroup buttonGroup, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
        : this(buttonGroup.QMCParent, text, listener, tooltip, rightContainer, defaultState, separator, onSprite, offSprite) { }
}