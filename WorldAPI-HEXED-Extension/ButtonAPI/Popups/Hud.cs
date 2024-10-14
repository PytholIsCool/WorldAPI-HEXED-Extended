using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Localization;

namespace WorldAPI.ButtonAPI.Popups;

internal class Hud {
    public static void Msg(string content, string description = null, Sprite icon = null, float duration = 5f) => VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicTYBoTYUnique_1_Boolean_0(icon, LocalizableStringExtensions.Localize(content, null, null, null), LocalizableStringExtensions.Localize(description, null, null, null), duration, null);
}
