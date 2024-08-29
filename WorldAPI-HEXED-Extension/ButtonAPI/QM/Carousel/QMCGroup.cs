using System;
using UnityEngine.UI;
using UnityEngine;
using WorldAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.QM.Controls;
using TMPro;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;
using WCv2.ButtonAPI.QM.Carousel.Items;

namespace WCv2.ButtonAPI.QM.Carousel
{
    public class QMCGroup : ButtonGroupControl
    {
        public Transform MenuContents { get; internal set; }

        private readonly VerticalLayoutGroup Layout;

        public QMCGroup(Transform parent, string text, TextAnchor ButtonAlignment = TextAnchor.UpperLeft)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            gameObject = Object.Instantiate(APIBase.QMCarouselPageTemplate, parent);
            gameObject.name = text;
            GameObject bg = APIBase.QMCarouselPageTemplate.transform.Find("QM_Settings_Panel/VerticalLayoutGroup/Background_Info").gameObject;
            Transform label = gameObject.transform.Find("QM_Foldout/Label");
            label.GetComponent<TextMeshProUGUI>().text = text;
            Transform settingsPanel = gameObject.transform.Find("QM_Settings_Panel/VerticalLayoutGroup");
            settingsPanel.DestroyChildren();
            GameObject newBG = Object.Instantiate(bg, settingsPanel);
            newBG.name = "Background_Info";

            Layout = gameObject.GetComponent<VerticalLayoutGroup>();
            Layout.childAlignment = ButtonAlignment;

            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }

        public void ChangeChildAlignment(TextAnchor ButtonAlignment = TextAnchor.UpperLeft) => Layout.childAlignment = ButtonAlignment;

        public QMCGroup(WorldPage page, string text, TextAnchor ButtonAlignment = TextAnchor.UpperLeft) : this(page.MenuContents, text, ButtonAlignment)
        { }

        public void AddToggle(string text, Action<bool> stateChange, bool defaultState = false, string toolTip = "", bool separator = false)
        {
            new QMCToggle(this, text, stateChange, defaultState, toolTip, separator);
        }
        public void AddSlider(string text, string tooltip, Action<float, QMCSlider> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false, string ending = "%", bool separator = false)
        {
            new QMCSlider(this, text, tooltip, listener, defaultValue, minValue, maxValue, isDecimal, ending, separator);
        }
        public void AddSelector(string text, string containerTooltip)
        {
            new QMCSelector(this, text, containerTooltip);
        }
    }
}
