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
using VRC.UI.Core.Styles;
using VRC.UI.Element;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using WorldAPI;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Carousel;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items;

public class QMCSlider : SliderControl {
    private bool shouldInvoke = true;
    private static Vector3 onPos = new(93, 0, 0), offPos = new(30, 0, 0);
    public QMCSlider(Transform parent, string text, string tooltip, Action<float, QMCSlider> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false, string ending = "%", bool separator = false) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        DefaultValue = defaultValue;

        var figures = "0";

        (gameObject = (transform = Object.Instantiate(APIBase.Slider, parent)).gameObject).name = text;
        (body = transform.Find("LeftItemContainer")).GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        (TextMeshPro = body.Find("Title").GetComponent<TextMeshProUGUI>()).text = text;
        TextMeshPro.richText = true;

        (slider = gameObject.transform.Find("RightItemContainer/Slider")).GetComponent<ToolTip>()._localizableString = tooltip.Localize();

        (snapSlider = slider.GetComponent<SnapSliderExtendedCallbacks>()).field_Private_UnityEvent_0 = null;
        snapSlider.onValueChanged = new();
        snapSlider.minValue = minValue;
        snapSlider.maxValue = maxValue;
        snapSlider.value = defaultValue;

        snapSlider.onValueChanged.AddListener(new Action<float>((va) => listener.Invoke(va, this)));

        if (separator != false)
            AddSeparator(parent);
        if (isDecimal != false)
            figures = "0.0";

        var perst = (valDisplay = slider.parent.Find("Text_MM_H3")).GetComponent<TextMeshProUGUI>();
        perst.gameObject.active = true;
        snapSlider.onValueChanged.AddListener(new Action<float>((va) => perst.text = va.ToString(figures) + ending));
        perst.text = defaultValue + ending;

        ResetButton = this.transform.Find("RightItemContainer/Button");

        gameObject.GetComponent<SettingComponent>().enabled = false;
    }
    public QMCSlider AddResetButton(float value = -0) {
        ResetButton.gameObject.SetActive(true);
        ResetButton.GetComponent<ToolTip>()._localizableString = "Reset to the default value".Localize();

        var button = ResetButton.GetComponent<Button>();
        button.onClick.AddListener(new Action(() => ResetValue(value)));

        return this;
    }

    //button code goes here

    private void ResetValue(float value) {
        Transform toggleButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleSwitch");
        Transform muteButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleButton");

        bool isToggleSwitchActive = toggleButton != null && toggleButton.gameObject.activeSelf;
        bool isToggleButtonActive = muteButton != null && muteButton.gameObject.activeSelf;

        if (isToggleSwitchActive || isToggleButtonActive) {
            if (ToggleValue == true)
                snapSlider.value = (value != -0) ? value : DefaultValue;
        }
        else {
            snapSlider.value = (value != -0) ? value : DefaultValue;
        }
    }

    public QMCSlider AddMuteButton(Action<bool> stateChange, bool defaultState = true) {
        Transform toggleButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleSwitch");
        bool isToggleSwitchActive = toggleButton != null && toggleButton.gameObject.activeSelf;

        if (!isToggleSwitchActive) {
            var muteButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleButton");
            muteButton.gameObject.SetActive(true);
            muteButton.SetSiblingIndex(4);

            ToggleValue = defaultState;

            (ToggleCompnt = muteButton.GetComponent<Toggle>()).onValueChanged = new();
            ToggleCompnt.isOn = defaultState;

            ToggleListener = stateChange;

            SetVisualComponents(defaultState);

            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) => {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ToggleListener, "SliderMuteToggle");
                APIBase.Events.onQMCSliderToggleValChange?.Invoke(this, val);
                ToggleValue = val;
                SetVisualComponents(val);
            }));
        }
        else {
            throw new Exception("MuteButton could not be added to the following object: \n" + this.ToString() + "\n Don't add a MuteButton and a Toggle to the same slider.");
        }

        return this;
    }
    public QMCSlider AddToggle(Action<bool> stateChange, bool defaultState = true) {
        Transform muteButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleButton");
        bool isToggleButtonActive = muteButton != null && muteButton.gameObject.activeSelf;

        if (!isToggleButtonActive) {
            var toggleButton = this.transform.Find("RightItemContainer/Cell_MM_ToggleSwitch");
            toggleButton.gameObject.SetActive(true);
            toggleButton.SetSiblingIndex(4);

            ToggleValue = defaultState;

            var button = toggleButton.Find("Cell_MM_OnOffSwitch").GetComponent<RadioButton>();
            button.Method_Public_Void_Boolean_0(defaultState);

            (Handle = button._handle)
                .transform.localPosition = defaultState ? onPos : offPos;

            (ToggleCompnt = toggleButton.transform.GetComponent<Toggle>()).onValueChanged = new Toggle.ToggleEvent();
            ToggleCompnt.isOn = defaultState;

            ToggleListener = stateChange;

            SetVisualComponents(defaultState);

            ToggleCompnt.onValueChanged.AddListener(new Action<bool>((val) => {
                if (shouldInvoke)
                    APIBase.SafelyInvolk(val, ToggleListener, "SliderToggle");
                APIBase.Events.onQMCSliderToggleValChange?.Invoke(this, val);
                button.Method_Public_Void_Boolean_0(val);
                Handle.localPosition = val ? onPos : offPos;
                ToggleValue = val;
                SetVisualComponents(val);
            }));

            toggleButton.gameObject.GetComponent<UiToggleTooltip>()._localizableString = "Enable this setting".Localize();
            toggleButton.gameObject.GetComponent<UiToggleTooltip>()._alternateLocalizableString = "Disable this setting".Localize();
        }
        else {
            throw new Exception("Toggle could not be added to the following object: \n" + this.ToString() + "\n Don't add a Toggle and a MuteButton to the same slider.");
        }

        return this;
    }
    public QMCSlider SoftSetState(bool value) {
        shouldInvoke = false;
        ToggleCompnt.isOn = value;
        shouldInvoke = true;

        return this;
    }
    private void SetVisualComponents(bool defaultState) {
        if (defaultState == false) {
            slider.GetComponent<Selectable>().m_GroupsAllowInteraction = false;
            slider.GetComponent<CanvasGroup>().alpha = 0.25f;
            slider.parent.Find("Text_MM_H3").GetComponent<CanvasGroup>().alpha = 0.25f;
        }
        else {
            slider.GetComponent<Selectable>().m_GroupsAllowInteraction = true;
            slider.GetComponent<CanvasGroup>().alpha = 1f;
            slider.parent.Find("Text_MM_H3").GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public QMCSlider(QMCGroup group, string text, string tooltip, Action<float, QMCSlider> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false, string ending = "%", bool separator = false)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup"), text, tooltip, listener, defaultValue, minValue, maxValue, isDecimal, ending, separator) { }
}