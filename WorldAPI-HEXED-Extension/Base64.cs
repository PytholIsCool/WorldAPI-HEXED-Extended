using UnityEngine;

namespace WorldAPI;

public class Base64 {
    public static Dictionary<string, Sprite> AlreadyLoaded = new();
    public static Sprite GetAndSetSprite(string base64String) {
        if (AlreadyLoaded.ContainsKey(base64String))
            return AlreadyLoaded[base64String];

        byte[] imageBytes = Il2CppSystem.Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(4, 4);
        texture.LoadImage(imageBytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(1f, 1f));
        AlreadyLoaded.Add(base64String, sprite);

        return sprite;
    }
}
