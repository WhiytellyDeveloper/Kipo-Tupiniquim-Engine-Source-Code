using KipoTupiniquimEngine.Extenssions;
using KipoTupiniquimEngine.Patches;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.UI;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KipoTupiniquimEngine.Classes
{
    public class KipoHudExtenssion : MonoBehaviour
    {
        public void Initialize(int player)
        {
            hud = Singleton<CoreGameManager>.Instance.GetHud(player);
            hud.SetItemSelect(0, Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.items[0].nameKey);
            canvas = hud.Canvas();
            CreateClock();
            CreateStaminaPorcentage();
            CreateLowStaminaText();
            //CreateUselessHand();
            //CreateQuatersText();

            initialized = true;
        }

        private void Update()
        {
            if (timeText != null && Singleton<BaseGameManager>.Instance != null)
                UpdateTimeText(Singleton<BaseGameManager>.Instance.realTime);

            if (staminaText != null && Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum) != null)
                UpdateStaminaPorcentage(Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).plm.stamina, Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).plm.staminaMax);

            ColerfulInventory();
            AdaptativeColorHud();
            UpdateStaminaBackground();
        }

        public void CreateClock()
        {
            clockAnimation = AssetLoader.SpritesFromSpritesheet(12, 1, 1, Vector2.zero, AssetLoader.TextureFromMod(Plugin._instance, "clockSpriteSheet.png"));

            var clock = UIHelpers.CreateImage(clockAnimation[0], canvas.transform, new(-288, 114, 1), false);
            clock.name = "ClockIcon";
            clock.transform.localScale = new(0.8f, 0.8f, 1);
            clock.transform.SetSiblingIndex(7);
            clockRenderer = clock;

            var darkerSprites = hud.ReflectionGetVariable("spritesToDarken") as Image[];
            Image[] updatedArray = new Image[darkerSprites.Length + 1];
            darkerSprites.CopyTo(updatedArray, 0);
            updatedArray[darkerSprites.Length] = clock;
            hud.ReflectionSetVariable("spritesToDarken", updatedArray);

            var clockText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "00:00", canvas.transform, new(-166, 105, 1));
            clockText.name = "TimeText";
            clockText.color = Color.black;
            clockText.alignment = TextAlignmentOptions.TopLeft;
            clockText.transform.SetSiblingIndex(8);
            timeText = clockText;
        }

        public void UpdateTimeText(float gameTime)
        {
            string str = Mathf.Floor(gameTime % 60f).ToString();
            if (Mathf.Floor(gameTime % 60f) < 10f)
                str = "0" + Mathf.Floor(gameTime % 60f).ToString();
            timeText.text = Mathf.Floor(gameTime / 60f).ToString() + ":" + str;

            if (lastMinute != ((int)Mathf.Floor(gameTime / 60f)))
            {
                _SpinClock();
                lastMinute = ((int)Mathf.Floor(gameTime / 60f));
            }
        }

        //Debug 
        public void _SpinClock() =>
            StartCoroutine(SpinClock());

        private IEnumerator SpinClock()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(Plugin.assetManager.Get<SoundObject>("ClockSpin"));
            foreach(Sprite clockSprite in clockAnimation)
            {
                clockRenderer.sprite = clockSprite;
                yield return new WaitForSeconds(0.1f);
            }
            clockRenderer.sprite = clockAnimation[0];
        }

        public void CreateStaminaPorcentage()
        {
            var staminaBarText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "100%", canvas.GetComponentsInChildren<Transform>().First(x => x.name == "Staminometer"), new(97, 21, 1));
            staminaBarText.name = "StaminaText";
            staminaBarText.color = Color.black;
            staminaBarText.transform.SetSiblingIndex(1);
            staminaBarText.alignment = TextAlignmentOptions.Top;
            staminaText = staminaBarText;
        }

        public void CreateLowStaminaText()
        {
            var lowStaminaText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "YOU NEED REST!!!", canvas.GetComponentsInChildren<Transform>().First(x => x.name == "Staminometer"), new(119, 86, 1));
            lowStaminaText.name = "LowStaminaText";
            lowStaminaText.color = Color.red;
            lowStaminaText.transform.SetSiblingIndex(5);
            lowStaminaText.alignment = TextAlignmentOptions.BottomLeft;
            this.lowStaminaText = lowStaminaText;
        }

        public void UpdateStaminaPorcentage(float staminaPorcentage, float staminaMax)
        {
            staminaText.text = $"{((int)Mathf.MoveTowards(staminaPorcentage, staminaPorcentage / staminaMax * 100, Time.deltaTime * 600))}%";

            if (staminaPorcentage <= 0)
                lowStaminaText.text = "YOU NEED REST!!!";
            else
                lowStaminaText.text = "";
        }


        public void CreateUselessHand()
        {
            var icon = UIHelpers.CreateImage(Plugin.assetManager.Get<Sprite>("Hand"), canvas.transform, new(0, 0, 1), false);
            icon.name = $"UselessHand";
            icon.transform.localScale = new(1.4f, 1.4f, 1);
            icon.transform.localRotation = Quaternion.Euler(0, 0, 12);
        }

        public void CollectPickup(Sprite sprite, Vector3 pos, float scaleMultiplier = 0.3f)
        {
            var icon = UIHelpers.CreateImage(sprite, canvas.transform, new(0, 0, 1), false);
            icon.name = $"{sprite.name}Icon";
            icon.transform.localScale = new(scaleMultiplier, scaleMultiplier, 1);
            icon.transform.SetSiblingIndex(0);

            StartCoroutine(MoveToPos(icon.transform, pos, 1300));
        }

        private IEnumerator MoveToPos(Transform targetObject, Vector3 targetPosition, float speed)
        {
            while (targetObject.localPosition != targetPosition)
            {
                targetObject.localPosition = Vector3.MoveTowards(targetObject.localPosition, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            Destroy(targetObject.gameObject);
        }

        public void ColerfulInventory()
        {
            try
            {
                if (Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).itm != null)
                {
                    ItemManager itm = Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).itm;
                    var itemBackgrounds = hud.ReflectionGetVariable("itemBackgrounds") as RawImage[];

                    for (int i = 0; i < itm.maxItem + 1; i++)
                    {
                        if (i == itm.selectedItem)
                        {
                            Color color = itm.items[itm.selectedItem].itemSpriteSmall.GetMostGenericFromSprite();

                            if (itm.items[itm.selectedItem].itemType == Items.None)
                                color = Color.red;
                            else
                                color -= new Color(0.17f, 0.17f, 0.17f, 0f);

                            itemBackgrounds[itm.selectedItem].color = color;
                        }
                        else
                        {
                            Color color = itm.items[i].itemSpriteSmall.GetMostGenericFromSprite();

                            if (itm.items[i].itemType == Items.None)
                                color = Color.white;
                            else
                                color += new Color(0.25f, 0.25f, 0.25f, 0f);

                            itemBackgrounds[i].color = color;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void AdaptativeColorHud()
        {
            try
            {
                var pos = Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).ec.CellFromPosition(Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).transform.position).position;
                var currentColor = Singleton<CoreGameManager>.Instance.lightMapTexture.GetPixel(pos.x, pos.z);
                var transparentColor = new Color(0, 0, 0, 0);

                if (Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).Invisible)
                    transparentColor = new(0, 0, 0, 0.3f);


                Image[] array = hud.ReflectionGetVariable("spritesToDarken") as Image[];

                if (array == null) return;

                for (int i = 0; i < array.Length; i++)
                    array[i].color = Color.Lerp(array[i].color, currentColor - transparentColor, Time.deltaTime * 5f);

            }
            catch {
            }
        }    

        //CHANGE THIS LATER URGENTLY
        public void UpdateStaminaBackground()
        {
            try
            {
                var player = Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum);
                int stamina = (int)player.plm.stamina;
                var staminatorBG = hud.transform.Find("Staminometer").Find("Background").GetComponent<Image>();

                if (stamina > 0 && stamina < 100)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("1StaminometerBG"); // 0% - 100%
                    StaminaMultiplierManager.Multiplier = 1;
                }
                else if (stamina > 100 && stamina < 200)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("2StaminometerBG"); // 200%
                    StaminaMultiplierManager.Multiplier = 2;
                }
                else if (stamina > 200 && stamina < 300)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("3StaminometerBG"); // 300%
                    StaminaMultiplierManager.Multiplier = 3;
                }
                else if (stamina > 300 && stamina < 400)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("4StaminometerBG"); // 400%
                    StaminaMultiplierManager.Multiplier = 4;
                }
                else if (stamina > 400 && stamina < 500)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("5StaminometerBG"); // 500%
                    StaminaMultiplierManager.Multiplier = 5;
                }
                else if (stamina > 500 && stamina < 600)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("6StaminometerBG"); // 600%
                    StaminaMultiplierManager.Multiplier = 6;
                }
                else if (stamina > 600 && stamina < 700)
                {
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("7StaminometerBG"); // 700%
                    StaminaMultiplierManager.Multiplier = 7;
                }
                /*
                else
                    staminatorBG.sprite = Plugin.assetManager.Get<Sprite>("MaxStaminometerBG"); // 800% e acima
                */

            }

            catch {
            }
        }

        public void CreateQuatersText()
        {
            var quartersText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "0/5", canvas.transform, new(299, -166, 1));
            quartersText.name = "QuartersText";
            quartersText.alignment = TextAlignmentOptions.Center;
            quartersText.color = Color.black;
            this.quartersText = quartersText;
        }

        public void UpdateQuartersText(int amount, int amountMax) =>
            quartersText.text = $"{amount}/{amountMax}";

        public HudManager hud;
        public Canvas canvas;
        public bool initialized;

        private Image clockRenderer;
        public Sprite[] clockAnimation, elevatorAnimation;
        private TextMeshProUGUI timeText, staminaText, lowStaminaText, quartersText;
        private int lastMinute;
    }
}
