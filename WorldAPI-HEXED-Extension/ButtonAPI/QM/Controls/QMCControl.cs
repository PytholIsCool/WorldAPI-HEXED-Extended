using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Tooltips;
using WorldAPI;
using WorldAPI.ButtonAPI.Controls;
using Object = UnityEngine.Object;

namespace WorldAPI.ButtonAPI.QM.Controls;

public class QMCControl : ExtendedControl {
    public GameObject OnImageObj { get; internal set; }
    public GameObject OffImageObj { get; internal set; }
    public Toggle ToggleCompnt { get; internal set; }
    public Transform Handle { get; internal set; }
    public UiToggleTooltip ToggleToolTip { get; internal set; }

    private GameObject seB;
    public void AddSeparator(Transform p) => (seB = Object.Instantiate(APIBase.QMCarouselSeparator, p)).name = "Separator";
}
