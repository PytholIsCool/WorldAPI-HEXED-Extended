using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VRC.UI.Elements.Controls;

namespace WorldAPI.ButtonAPI.QM.Wings.Controls;

public class WingRoot {
    public GameObject gameObject { get; internal set; }
    public Transform transform { get; internal set; }
    public TextMeshProUGUI TMProCompnt { get; internal set; }
}