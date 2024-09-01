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

namespace WorldAPI.ButtonAPI.QM.Carousel.Items
{
    public class QMCTitle : Root
    {
        public QMCTitle(Transform parent, string text, bool separator = false)
        {
            if (!APIBase.IsReady())
                throw new NullReferenceException("Object Search had FAILED!");

            transform = Object.Instantiate(APIBase.QMCarouselTitleTemplate, parent).transform;
            gameObject = transform.gameObject;
            gameObject.name = text;

            TMProCompnt = transform.Find("LeftItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUI>();
            TMProCompnt.text = text;
            TMProCompnt.richText = true;

            if (separator != false)
            {
                GameObject seB = Object.Instantiate(APIBase.QMCarouselSeparator, parent);
                seB.name = "Separator";
            }
        }
        public QMCTitle(QMCGroup group, string text, bool separator = false)
            : this(group.GetTransform().Find("QM_Settings_Panel/VerticalLayoutGroup"), text, separator) 
        { }
    }
}
