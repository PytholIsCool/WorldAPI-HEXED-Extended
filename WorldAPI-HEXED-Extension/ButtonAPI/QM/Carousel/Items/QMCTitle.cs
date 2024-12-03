using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using WorldAPI.ButtonAPI.QM.Carousel;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;
using WorldAPI.ButtonAPI.QM.Controls;

namespace WorldAPI.ButtonAPI.QM.Carousel.Items;

public class QMCTitle : QMCControl {
    private float _fontSize;
    public float FontSize { get => _fontSize; set {
            _fontSize = value;
            if (TMProCompnt != null)
                TMProCompnt.fontSize = _fontSize;
        } 
    }
    public QMCTitle(Transform parent, string text, bool separator = false, float fontSize = 28f) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        if (fontSize > 112f)
            fontSize = 112f;
        if (fontSize < 0f)
            fontSize = 0f;

        (transform = (gameObject = Object.Instantiate(APIBase.QMCarouselTitleTemplate, parent)).transform).name = text;

        Text = (TMProCompnt = transform.Find("LeftItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;
        TMProCompnt.fontSize = fontSize;

        if (separator != false)
            AddSeparator(parent);
    }
    public QMCTitle(QMCGroup group, string text, bool separator = false)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup"), text, separator) { }
}
