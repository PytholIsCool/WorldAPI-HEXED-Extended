using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using WorldAPI.ButtonAPI.Extras;
using Object = UnityEngine.Object;

namespace WorldAPI.NamePlateAPI;

public static class BasePlate {
    public static Plate AddPlate(this Player player) => new(player);

    #region Plates
    public class Plate {
        private static int PlateCount { get; set; }
        public Transform _basePlate { get; internal set; }
        public Transform transform { get; internal set; }
        public GameObject gameObject { get; internal set; }
        public Dictionary<string, Tag> TagList { get; internal set; }
        public Dictionary<Sprite, Icon> IconList { get; internal set; }
        public Plate(Player player) {
            CheckCountContainer(player);

            (transform = (gameObject = Object.Instantiate((_basePlate = player._vrcplayer.field_Public_PlayerNameplate_0.transform).Find("Contents/Quick Stats").gameObject, player._vrcplayer.field_Public_PlayerNameplate_0.transform.Find("Contents"))).transform).name = $"PyPlateBase_{Guid.NewGuid()}";
            transform.DestroyChildren();

            if (PlateCount == 0) {
                transform.parent.Find("Main/Group Banner").localScale = new(0f, 0f, 0f);
                transform.parent.Find("Main/Group Banner_Expanded").localScale = new(0f, 0f, 0f);
                transform.parent.Find("Group Info").localScale = new(0f, 0f, 0f);
            }
            float y = PlateCount * 30f + 60f;
            transform.localPosition = new(0, y, 0);

            gameObject.SetActive(true);
        }
        public Tag AddTag(string text) {
            var temp = new Tag(this, text);
            TagList.Add(temp.Identifier, temp);
            return temp;
        }
        public Tag FindTag(string indentifier) => TagList[indentifier];
        public Icon AddIcon(Sprite sprite, Sprite subSprite = null) {
            var temp = new Icon(this, sprite, subSprite);
            IconList.Add(sprite, temp);
            return temp;
        }
        public Icon FindIcon(Sprite identifier) => IconList[identifier];

        private static void CheckCountContainer(Player player) { //I've thought of about a million different ways to do this and they'd all be more efficient but ive stopped caring
            if (player._vrcplayer.field_Public_PlayerNameplate_0.transform.Find("Count") == null) {
                GameObject cContainer = new() { name = "Count" };
                cContainer.transform.SetParent(player._vrcplayer.field_Public_PlayerNameplate_0.transform);
                GameObject Count = new();
                PlateCount = int.Parse(Count.name = "1");
                Count.transform.SetParent(cContainer.transform);
            } else {
                Transform count = player._vrcplayer.field_Public_PlayerNameplate_0.transform.Find("Count").GetChild(0);
                PlateCount = int.Parse(count.name);
                PlateCount++;
                count.name = PlateCount.ToString();
            }
        }
        #endregion

        #region Tags
        public class Tag {
            public string Identifier {  get; private set; }
            public Transform transform { get; internal set; }
            public GameObject gameObject { get; internal set; }
            public TextMeshProUGUI TMProCompt { get; internal set; }
            private string _text { get; set; }
            private static int TempID = 0;
            public string Text {
                get => _text; set {
                    _text = value;
                    if (TMProCompt != null)
                        TMProCompt.text = _text;
                }
            }
            public Tag(Plate plate, string text) {
                (TMProCompt = (transform = (gameObject = Object.Instantiate(plate._basePlate.Find("Contents/Quick Stats/Trust Text"), plate.transform).gameObject).transform).GetComponent<TextMeshProUGUI>()).text = text;
                TMProCompt.richText = true;
                TMProCompt.color = new(1f, 1f, 1f, 1f);

                if (text == null)
                    Identifier = TempID++.ToString();
                else
                    Identifier = text;
                plate.TagList.Add(Identifier, this);
            }
        }
        #endregion

        #region Icons
        public class Icon {
            private Sprite _sprite { get; set; }
            private Sprite _subsprite { get; set; }
            public Transform transform { get; internal set; }
            public GameObject gameObject { get; internal set; }
            public Sprite MainIcon {
                get => _sprite; set {
                    _sprite = value;
                    if (Image != null)
                        Image.overrideSprite = _sprite;
                }
            }
            public Sprite SubIcon {
                get => _subsprite; set {
                    _subsprite = value;
                    if (SubImage != null)
                        SubImage.overrideSprite = _subsprite;
                }
            }
            private Image Image { get; set; }
            private Image SubImage { get; set; }
            public Icon(Plate plate, Sprite icon, Sprite subIcon = null) {
                (Image = (transform = (gameObject = Object.Instantiate(plate._basePlate.Find("Contents/Quick Stats/Performance Icon"), plate.transform).gameObject).transform).GetComponent<Image>()).overrideSprite = icon;
                if (subIcon != null)
                    (SubImage = Image.transform.Find("Reason").GetComponent<Image>()).overrideSprite = subIcon;
                else
                    (SubImage = Image.transform.Find("Reason").GetComponent<Image>()).gameObject.SetActive(false);
                plate.IconList.Add(icon, this);
            }
            #endregion
        }
    }
}
