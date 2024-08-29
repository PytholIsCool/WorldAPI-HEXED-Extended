using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;
using VRC.UI.Element;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using WorldAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

namespace WCv2.ButtonAPI.QM.Carousel.Items
{
    public class QMCSlider : Root
    {
        //slider
        public Action<float, QMCSlider> Listener { get; set; }
        public TextMeshProUGUI TextMeshPro { get; private set; }
        private Transform body { get; set; }
        public Transform valDisplay { get; private set; }
        public Transform slider { get; private set; }
        public float DefaultValue { get; private set; }
        public SnapSliderExtendedCallbacks snapSlider { get; private set; }
        //toggle
        public Action<bool> ToggleListener { get; set; }
        public Transform Handle { get; private set; }
        public Toggle ToggleCompnt { get; private set; }
        public bool ToggleValue { get; private set; }
        private bool shouldInvoke = true;
        private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
        //reset button
        public Transform ResetButton { get; private set; }
        public QMCSlider(QMCGroup group, string text, string tooltip, Action<float, QMCSlider> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false, string ending = "%", bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            var figures = "0";

            transform = Object.Instantiate(APIBase.Slider, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
            gameObject = transform.gameObject;
            gameObject.name = text;

            (body = transform.Find("LeftItemContainer"))
                .GetComponent<ToolTip>()._localizableString = tooltip.Localize();

            (TextMeshPro = body.Find("Title").GetComponent<TextMeshProUGUI>()).text = text;
            TextMeshPro.richText = true;

            (slider = gameObject.transform.Find("RightItemContainer/Slider"))
                .GetComponent<VRC.UI.Elements.Controls.ToolTip>()._localizableString = tooltip.Localize();

            valDisplay = slider.parent.Find("Text_MM_H3");

            snapSlider = slider.GetComponent<SnapSliderExtendedCallbacks>();
            snapSlider.field_Private_UnityEvent_0 = null;
            snapSlider.onValueChanged = new();
            snapSlider.minValue = minValue;
            snapSlider.maxValue = maxValue;
            DefaultValue = defaultValue = snapSlider.value = defaultValue;

            Listener = (value, slider) =>
            {
                if (ToggleValue == true)
                {
                    listener.Invoke(value, this);
                }
            };

            snapSlider.onValueChanged.AddListener(new Action<float>((value) => Listener?.Invoke(value, this)));

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
                seB.name = "Separator";
            }
            if (isDecimal != false)
            {
                figures = "0.0";
            }

            var perst = valDisplay.GetComponent<TextMeshProUGUI>();
            perst.gameObject.active = true;
            snapSlider.onValueChanged.AddListener(new Action<float>((va) => perst.text = va.ToString(figures) + ending));
            perst.text = defaultValue + ending;

            gameObject.GetComponent<SettingComponent>().enabled = false;
        }
        public void AddResetButton()
        {
            ResetButton = this.transform.Find("RightItemContainer/Button");
            ResetButton.gameObject.SetActive(true);
            ResetButton.GetComponent<ToolTip>()._localizableString = "Reset to the default value".Localize();
            ResetButton.GetOrAddComponent<CanvasGroup>().alpha = 1f;

            var button = ResetButton.GetComponent<Button>();
            button.onClick.AddListener(new Action(() => ResetValue()));
        }

        private void ResetValue()
        {
            if (ToggleValue == true)
            {
                snapSlider.value = DefaultValue;
            }
        }

        public void AddMuteButton(Action<bool> stateChange, bool defaultState = true)
        {
            var muteButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleButton");
            muteButton.gameObject.SetActive(true);
            muteButton.SetSiblingIndex(4);

            ToggleValue = defaultState;

            ToggleCompnt = muteButton.GetComponent<Toggle>();
            ToggleListener = stateChange;
            ToggleCompnt.onValueChanged = new();
            ToggleCompnt.isOn = defaultState;

            SetVisualComponents(defaultState);

            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ToggleListener, "SliderMuteToggle");
                APIBase.Events.onQMCSliderToggleValChange?.Invoke(this, val);
                ToggleValue = val;
                SetVisualComponents(val);
            }));
        }

        public void AddToggle(Action<bool> stateChange, bool defaultState = true)
        {
            var toggleButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleSwitch");
            toggleButton.gameObject.SetActive(true);
            toggleButton.SetSiblingIndex(4);

            ToggleValue = defaultState;

            var button = toggleButton.Find("Cell_MM_OnOffSwitch").GetComponent<RadioButton>();
            button.Method_Public_Void_Boolean_0(defaultState);

            (Handle = button._handle)
                .transform.localPosition = defaultState ? onPos : offPos;

            ToggleCompnt = toggleButton.transform.GetComponent<Toggle>();
            ToggleListener = stateChange;

            ToggleCompnt.onValueChanged = new Toggle.ToggleEvent();
            ToggleCompnt.isOn = defaultState;

            SetVisualComponents(defaultState);

            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ToggleListener, "SliderToggle");
                APIBase.Events.onQMCSliderToggleValChange?.Invoke(this, val);
                button.Method_Public_Void_Boolean_0(val);
                Handle.localPosition = val ? onPos : offPos;
                ToggleValue = val;
                SetVisualComponents(val);
            }));

            toggleButton.gameObject.GetComponent<UiToggleTooltip>()._localizableString = "Enable / disable this setting".Localize();
        }

        private void SetVisualComponents(bool defaultState)
        {
            if (defaultState == false)
            {
                slider.GetComponent<Selectable>().m_GroupsAllowInteraction = false;
                slider.GetComponent<CanvasGroup>().alpha = 0.25f;
                slider.parent.Find("Text_MM_H3").GetComponent<CanvasGroup>().alpha = 0.25f;
                if (ResetButton.gameObject.active == true)
                {
                    var RBStyle = ResetButton.GetComponent<StyleElement>();
                    ResetButton.GetComponent<CanvasGroup>().alpha = 0.25f;
                    RBStyle.field_Private_Boolean_0 = false;
                    RBStyle.field_Private_Boolean_1 = true;
                }
            }
            else
            {
                slider.GetComponent<Selectable>().m_GroupsAllowInteraction = true;
                slider.GetComponent<CanvasGroup>().alpha = 1f;
                slider.parent.Find("Text_MM_H3").GetComponent<CanvasGroup>().alpha = 1f;
                if (ResetButton.gameObject.active == true)
                {
                    var RBStyle = ResetButton.GetComponent<StyleElement>();
                    ResetButton.GetComponent<CanvasGroup>().alpha = 1f;
                    RBStyle.field_Private_Boolean_0 = true;
                    RBStyle.field_Private_Boolean_1 = false;
                }
            }
        }
    }
}
