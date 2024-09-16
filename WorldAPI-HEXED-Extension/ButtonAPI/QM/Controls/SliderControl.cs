using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;
using WorldAPI.ButtonAPI.Extras;
using WorldAPI.ButtonAPI.QM.Carousel.Items;

namespace Serpentine.ButtonAPI.QM.Controls
{
    public class SliderControl : QMCControl
    {
        public TextMeshProUGUI TextMeshPro { get; set; }
        public Transform body { get; set; }
        public Transform valDisplay { get; set; }
        public Transform slider { get; set; }
        public float DefaultValue { get; set; }
        public SnapSliderExtendedCallbacks snapSlider { get; set; }

        public Action<bool> ToggleListener { get; set; }
        public bool ToggleValue { get; set; }

        public Transform ResetButton { get; set; }
    }
}
