using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Tooltips;
using WorldAPI.ButtonAPI.Controls;

namespace Serpentine.ButtonAPI.QM.Controls
{
    public class QMCControl : ExtendedControl
    {
        public Image OnImage { get; internal set; }
        public Image OffImage { get; internal set; }
        public Toggle ToggleCompnt { get; internal set; }
        public Transform Handle { get; internal set; }
        public UiToggleTooltip ToggleToolTip { get; internal set; }
    }
}
