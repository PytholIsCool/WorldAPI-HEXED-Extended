using System;
using UnityEngine.UI;
using UnityEngine;
using WorldAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.QM.Controls;
using TMPro;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;
using WorldAPI.ButtonAPI.QM.Carousel.Items;

namespace WorldAPI.ButtonAPI.QM.Carousel
{
    public class QMCGroup : Root
    {
        public RectMask2D parentMenuMask { get; internal set; }
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

        public QMCToggle AddQMCToggle(string text, Action<bool> stateChange, string tooltip = "", bool defaultState = false, bool separator = false)
        {
            return new QMCToggle(this, text, stateChange, tooltip, defaultState, separator);
        }
        public QMCSlider AddQMCSlider(string text, string tooltip, Action<float, QMCSlider> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false, string ending = "%", bool separator = false)
        {
            return new QMCSlider(this, text, tooltip, listener, defaultValue, minValue, maxValue, isDecimal, ending, separator);
        }
        public QMCSelector AddQMCSelector(string text, string containerTooltip)
        {
            return new QMCSelector(this, text, containerTooltip);
        }
        public QMCFuncButton AddQMCFuncButton(string text, string tooltip, Action listener, bool rightContainer = false, bool separator = false, Sprite sprite = null)
        {
            return new QMCFuncButton(this, text, tooltip, listener, rightContainer, separator, sprite);
        }
        public QMCFuncToggle AddQMCFuncToggle(string text, Action<bool> listener, string tooltip = "", bool rightContainer = false, bool defaultState = false, bool separator = false, Sprite onSprite = null, Sprite offSprite = null)
        {
            return new QMCFuncToggle(this, text, listener, tooltip, rightContainer, defaultState, separator, onSprite, offSprite);
        }
        public QMCTitle AddQMCTitle(string text, bool separator = false)
        {
            return new QMCTitle(this, text, separator);
        }
    }
}
