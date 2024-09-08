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
namespace WorldAPI.ButtonAPI.QM.Carousel.Items
{
    public class QMCFuncToggle : ExtentedControl //i just realized hacker mispelled this shit
    {
        public Action<bool> Listener { get; set; }
        public bool isToggled { get; private set; }
        public Image ToggleSprite;
        public Transform ButtonParent { get; private set; }
        public static Transform leftPar { get; private set; }
        public static Transform rightPar { get; private set; }
        public Sprite OnSprite { get; private set; }
        public Sprite OffSprite { get; private set; }
        private bool shouldInvoke = true;
        public QMCFuncToggle(Transform parent, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");
            transform = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate, parent).transform;
            gameObject = transform.gameObject;
            gameObject.name = text + "_ToggleControlContainer";

            leftPar = transform.Find("LeftItemContainer");
            leftPar.gameObject.DestroyChildren();
            rightPar = transform.Find("RightItemContainer");
            rightPar.gameObject.DestroyChildren();

            ButtonParent = rightContainer ? rightPar : leftPar;

            transform.Find("TitleMainContainer").gameObject.SetActive(false);

            Transform newToggle = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
            newToggle.name = text + "_FunctionToggle";

            GameObject OnIconObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle);
            OnIconObj.name = "OnIcon";
            OnIconObj.GetComponent<Image>().sprite = onSprite ?? APIBase.OnSprite;

            GameObject OffIconObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle);
            OffIconObj.name = "OffIcon";
            OffIconObj.GetComponent<Image>().sprite = offSprite ?? APIBase.OffSprite;

            newToggle.Find("Icon").gameObject.SetActive(false);

            TMProCompnt = newToggle.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, parent);
                seB.name = "Separator";
            }

            newToggle.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            bool isToggledLocal = defaultState;
            OnIconObj.SetActive(isToggledLocal);
            OffIconObj.SetActive(!isToggledLocal);

            Button buttonComponent = newToggle.GetComponent<Button>();
            buttonComponent.onClick = new();
            buttonComponent.onClick.AddListener(new Action(() =>
            {
                isToggledLocal = !isToggledLocal;

                OnIconObj.SetActive(isToggledLocal);
                OffIconObj.SetActive(!isToggledLocal);

                if (shouldInvoke)
                {
                    APIBase.SafelyInvolk(isToggledLocal, listener, text);
                }
            }));

            newToggle.gameObject.SetActive(true);
        }
        public QMCFuncToggle AddButton(string text, string tooltip, Action listener, bool rightContainer = false, Sprite sprite = null)
        {
            ButtonParent = rightContainer ? rightPar : leftPar;

            Transform newButton = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
            newButton.name = text;

            if (sprite != null)
            {
                var Tsprite = newButton.Find("Icon").GetComponent<Image>();
                Tsprite.overrideSprite = sprite;
                Tsprite.gameObject.SetActive(true);
            }

            TMProCompnt = newButton.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            newButton.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            ButtonCompnt = newButton.GetComponent<Button>();
            ButtonCompnt.onClick = new();
            ButtonCompnt.onClick.AddListener(listener);

            newButton.gameObject.SetActive(true);

            return this;
        }
        public QMCFuncToggle AddToggle(string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, Sprite onSprite = null, Sprite offSprite = null)
        {
            ButtonParent = rightContainer ? rightPar : leftPar;

            Transform newToggle = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), ButtonParent);
            newToggle.name = text + "_FunctionToggle";

            GameObject OnIconObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle);
            OnIconObj.name = "OnIcon";
            OnIconObj.GetComponent<Image>().sprite = onSprite ?? APIBase.OnSprite;

            GameObject OffIconObj = Object.Instantiate(newToggle.Find("Icon").gameObject, newToggle);
            OffIconObj.name = "OffIcon";
            OffIconObj.GetComponent<Image>().sprite = offSprite ?? APIBase.OffSprite;

            newToggle.Find("Icon").gameObject.SetActive(false);

            TMProCompnt = newToggle.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            newToggle.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            bool isToggledLocal = defaultState;
            OnIconObj.SetActive(isToggledLocal);
            OffIconObj.SetActive(!isToggledLocal);

            Button buttonComponent = newToggle.GetComponent<Button>();
            buttonComponent.onClick = new();
            buttonComponent.onClick.AddListener(new Action(() =>
            {
                isToggledLocal = !isToggledLocal;

                OnIconObj.SetActive(isToggledLocal);
                OffIconObj.SetActive(!isToggledLocal);

                if (shouldInvoke)
                {
                    APIBase.SafelyInvolk(isToggledLocal, listener, text);
                }
            }));

            newToggle.gameObject.SetActive(true);

            return this;
        }

        public void SoftSetState(bool state)
        {
            if (isToggled != state)
            {
                isToggled = state;
                ToggleSprite.overrideSprite = isToggled ? OnSprite : OffSprite;

                if (shouldInvoke)
                    APIBase.SafelyInvolk(isToggled, Listener, "SoftSet");
            }
        }
        public QMCFuncToggle(QMCGroup group, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
            : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, listener, tooltip, rightContainer, defaultState, separator, onSprite, offSprite) 
        { }
        public QMCFuncToggle(CollapsibleButtonGroup buttonGroup, string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
            : this(buttonGroup.QMCParent, text, listener, tooltip, rightContainer, defaultState, separator, onSprite, offSprite) 
        { }
    }
}
