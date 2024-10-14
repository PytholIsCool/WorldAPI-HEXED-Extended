using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace WorldAPI.ButtonAPI.Popups;

internal class QM {
    public static void Msg(string text) {
        var popup = APIBase.QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Modal_Alert").GetComponent<ModalAlert>();
        popup.field_Public_TextMeshProUGUIEx_0.richText = true;
        //popup.Method_Public_Void_LocalizableString_PDM_0(text.Localize());
        popup.Method_Public_Void_LocalizableString_PDM_1(text.Localize());
    }
}
