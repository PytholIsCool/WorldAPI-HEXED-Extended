using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Localization;

namespace WorldAPI.ButtonAPI.Popups;

internal class MM {
    public static void Msg(string text) {
        var menu = APIBase.MMM.transform.Find("Container/MMParent/Modal_MM_Alert").GetComponent<VRC.UI.Elements.Controls.ModalAlert>();
        menu.field_Public_TextMeshProUGUIEx_0.richText = true;
        //menu.Method_Public_Void_LocalizableString_PDM_1(text.Localize());
        menu.Method_Public_Void_LocalizableString_PDM_0(text.Localize());
    }
}
