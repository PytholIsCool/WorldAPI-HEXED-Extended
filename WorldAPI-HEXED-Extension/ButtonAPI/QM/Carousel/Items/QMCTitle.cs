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
    public QMCTitle(Transform parent, string text, bool separator = false) {
        if (!APIBase.IsReady())
            throw new NullReferenceException("Object Search has FAILED!");

        (transform = (gameObject = Object.Instantiate(APIBase.QMCarouselTitleTemplate, parent)).transform).name = text;

        (TMProCompnt = transform.Find("LeftItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUI>()).text = text;
        TMProCompnt.richText = true;

        if (separator != false)
            AddSeparator(parent);
    }
    public QMCTitle(QMCGroup group, string text, bool separator = false)
        : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup"), text, separator) { }
}
