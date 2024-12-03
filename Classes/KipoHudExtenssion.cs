using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.UI;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

            initialized = true;
        }

        private void Update()
        {
            if (timeText != null && Singleton<BaseGameManager>.Instance != null)
                UpdateTimeText(Singleton<BaseGameManager>.Instance.realTime);

            if (staminaText != null && Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum) != null)
                UpdateStaminaPorcentage(Singleton<CoreGameManager>.Instance.GetPlayer(hud.hudNum).plm.stamina);
        }

        public void CreateClock()
        {
            var clock = UIHelpers.CreateImage(Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "AlarmClockIcon_Large"), canvas.transform, new(-288, 114, 1), false);
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
                clockRenderer.color = Random.ColorHSV();
                lastMinute = ((int)Mathf.Floor(gameTime / 60f));
            }
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

        public void UpdateStaminaPorcentage(float staminaPorcentage)
        {
            staminaText.text = $"{(int)staminaPorcentage}%";

            if (staminaPorcentage <= 5)
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

        public HudManager hud;
        public Canvas canvas;
        public bool initialized;

        private Image clockRenderer;
        private TextMeshProUGUI timeText, staminaText, lowStaminaText;
        private int lastMinute;
    }
}
