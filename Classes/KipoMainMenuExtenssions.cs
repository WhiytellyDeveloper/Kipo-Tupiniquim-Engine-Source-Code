using MTM101BaldAPI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace KipoTupiniquimEngine.Classes
{
    public class KipoMainMenuExtenssions : MonoBehaviour
    {
        public Transform exitChalkboard;

        public void Initialize()
        {
            var text = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "TestButton", GameObject.Find("Menu").transform, new(-100, 0));
            text.raycastTarget = true;
            text.color = Color.black;
            var placeholderButton = text.gameObject.ConvertToButton<StandardMenuButton>(true);
            placeholderButton.underlineOnHigh = true;
            placeholderButton.InitializeAllEvents();

            CreateExitChalkboard();

            placeholderButton.OnPress.AddListener(delegate () {
                SwitchExitChalkboard(true);
            });
        }

        private void CreateExitChalkboard()
        {
            var chalkboard = UIHelpers.CreateImage(Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "ChalkBoardStandard"), GameObject.Find("Menu").transform, new(215, -125, 0), false, 1.2f);
            chalkboard.name = "Chalkboard";
            exitChalkboard = chalkboard.transform;

            var text1 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "Are you leaving already?<color=red>\nYou'll miss out on the rest of the fun!</color>", chalkboard.transform, Vector3.up * -110, false);
            text1.rectTransform.sizeDelta = new(500, 500);
            text1.alignment = TextAlignmentOptions.Top;

            var text2 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "Sorry baldi, I'll be back soon...", chalkboard.transform, Vector3.up * -109, false);
            text2.color = Color.red;
            text2.rectTransform.sizeDelta = new(250, 50);
            text2.alignment = TextAlignmentOptions.Center;
            text2.raycastTarget = true;

            var exitButton = text2.gameObject.ConvertToButton<StandardMenuButton>(true);
            exitButton.underlineOnHigh = true;
            exitButton.InitializeAllEvents();

            exitButton.OnPress.AddListener(delegate () {
                CursorController.Instance.SetColor(new(0, 0, 0, 0));
                CursorController.Instance.DisableClick(true);
                GameObject.Find("Menu").GetComponent<MainMenu>().Quit();
                SwitchExitChalkboard(false);
            });

            var text3 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, "You're right, Our fun isn't over yet!", chalkboard.transform, Vector3.up * -81, false);
            text3.color = Color.green;
            text3.rectTransform.sizeDelta = new(400, 50);
            text3.alignment = TextAlignmentOptions.Center;
            text3.raycastTarget = true;

            var cancelButton = text3.gameObject.ConvertToButton<StandardMenuButton>(true);
            cancelButton.underlineOnHigh = true;
            cancelButton.InitializeAllEvents();

            cancelButton.OnPress.AddListener(delegate () {  
                SwitchExitChalkboard(false);
            });

            exitChalkboard.gameObject.SetActive(false);
        }

        public void SwitchExitChalkboard(bool value)
        {
            Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.00000334561f);
            exitChalkboard.gameObject.SetActive(value);

            if (GameObject.Find("New Game Object") != null)
                GameObject.Find("New Game Object").SetActive(!value);
        }
    }
}
