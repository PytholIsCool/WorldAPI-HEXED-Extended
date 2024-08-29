using System;
using UnityEngine;
using VRC.UI.Elements;
using WorldAPI.ButtonAPI.Controls;

namespace WorldAPI.ButtonAPI.QM.Controls;

public class WorldPage : Root {
    public string MenuName { get; internal set; }
    public Action OnMenuOpen { get; set; }

    public UIPage Page { get; internal set; }
    public Transform MenuContents { get; internal set; }
}
