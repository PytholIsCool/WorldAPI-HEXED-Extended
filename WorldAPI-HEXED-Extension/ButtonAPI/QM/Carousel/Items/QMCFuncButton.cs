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
    public class QMCFuncButton : ExtentedControl //this control was extra difficult for no good fucking reason
    {
        public static Button AdditionalButton { get; private set; }
        public QMCFuncButton(Transform parent, string text, string tooltip, Action listener)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");
            transform = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate, parent).transform;
            gameObject = transform.gameObject;
            gameObject.name = text + "_ControlContainer";
            transform.Find("LeftItemContainer").gameObject.DestroyChildren();
            transform.Find("TitleMainContainer").gameObject.SetActive(false);

            Transform button = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), transform.Find("LeftItemContainer"));
            button.name = text;

            TMProCompnt = button.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            button.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            ButtonCompnt = button.GetComponent<Button>();
            ButtonCompnt.onClick = new();
            ButtonCompnt.onClick.AddListener(listener);

            button.gameObject.SetActive(true);
        }
        public void AddButton(string text, string tooltip, Action listener)
        {
            Transform newButton = Object.Instantiate(APIBase.QMCarouselFuncButtonTemplate.transform.Find("LeftItemContainer/Button (1)"), this.transform.Find("LeftItemContainer"));
            newButton.name = text;

            TMProCompnt = newButton.Find("Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            newButton.GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            AdditionalButton = newButton.GetComponent<Button>();
            AdditionalButton.onClick = new();
            AdditionalButton.onClick.AddListener(listener);

            newButton.gameObject.SetActive(true);
        }
        public QMCFuncButton(QMCGroup group, string text, string tooltip, Action listener)
            : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform, text, tooltip, listener)
        { }
        public QMCFuncButton(CollapsibleButtonGroup buttonGroup, string text, string tooltip, Action listener)
            : this(buttonGroup.QMCParent, text, tooltip, listener)
        { }
    }
}
