﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VRC.Localization;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Controls;
using WorldAPI;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace WCv2.ButtonAPI.QM.Carousel.Items
{
    public class QMCSelector : ExtentedControl
    {
        public ToolTip ContainerTooltip { get; set; }
        public ToolTip SelectionBoxTextTooltip { get; set; }
        public TextMeshProUGUI TMProSelectionBoxText { get; set; }

        private List<Setting> settings = new List<Setting>();
        private int currentIndex = 0;
        public QMCSelector(QMCGroup group, string text, string containerTooltip, bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            gameObject = Object.Instantiate(APIBase.QMCarouselSelectorTemplate, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
            transform = gameObject.transform;
            gameObject.name = text;

            TMProCompnt = transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;
            Text = text;

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup").transform);
                seB.name = "Separator";
            }

            (ContainerTooltip = transform.Find("LeftItemContainer").GetComponent<ToolTip>())._localizableString = containerTooltip.Localize();

            Button buttonLeft = transform.Find("RightItemContainer/ButtonLeft").transform.GetComponent<Button>();
            buttonLeft.onClick.AddListener(new Action(() => ScrollLeft()));
            Button buttonRight = transform.Find("RightItemContainer/ButtonRight").transform.GetComponent<Button>();
            buttonRight.onClick.AddListener(new Action(() => ScrollRight()));
        }
        public void AddSetting(string name, string tooltip, Action listener)
        {
            settings.Add(new Setting { Name = name, Tooltip = tooltip, Listener = listener });

            if (settings.Count == 1)
            {
                UpdateDisplayedSetting(0);
            }
        }

        private void ScrollLeft()
        {
            if (settings.Count == 0) return;

            currentIndex = (currentIndex - 1 + settings.Count) % settings.Count;
            UpdateDisplayedSetting(currentIndex);
        }

        private void ScrollRight()
        {
            if (settings.Count == 0) return;

            currentIndex = (currentIndex + 1) % settings.Count;
            UpdateDisplayedSetting(currentIndex);
        }

        private void UpdateDisplayedSetting(int index)
        {
            var setting = settings[index];
            TMProSelectionBoxText = this.transform.Find("RightItemContainer/OptionSelectionBox/Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProSelectionBoxText.text = setting.Name;

            (SelectionBoxTextTooltip = this.transform.Find("RightItemContainer/OptionSelectionBox").GetComponent<ToolTip>())._localizableString = setting.Tooltip.Localize();

            setting.Listener?.Invoke();
        }
        private class Setting
        {
            public string Name { get; set; }
            public string Tooltip { get; set; }
            public Action Listener { get; set; }
        }
    }
}
