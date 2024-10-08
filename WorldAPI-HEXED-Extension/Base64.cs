﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WorldAPI;

public class Base64 //this is a useless class, dont worry about this
{
    public static Dictionary<string, Sprite> AlreadyLoaded = new();

    public static Sprite FromBase(string data)
    {
        AlreadyLoaded.TryGetValue(data, out var sprite);
        if (sprite != null) return AlreadyLoaded[data];
        Texture2D t = new(2, 2);
        ImageConversion.LoadImage(t, Convert.FromBase64String(data));
        Rect rect = new(0.0f, 0.0f, t.width, t.height);
        Vector2 pivot = new(0.5f, 0.5f);
        Vector4 border = Vector4.zero;

        Sprite s = Sprite.CreateSprite_Injected(t, ref rect, ref pivot, 100.0f, 0, SpriteMeshType.Tight, ref border, false, null); 
        AlreadyLoaded.Add(data, s);
        return s;
    }
}
