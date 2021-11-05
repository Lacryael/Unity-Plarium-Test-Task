using System;
using UnityEngine;
using Assets.Data;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {
        public GameObject buttonApply;

        public GameObject flowerBeeLimitText;
        public GameObject beeSpeedText;
        public GameObject flowerHoneyText;

        public GameObject flowerQuantityText;
        public GameObject beeQuantityText;
        public GameObject beeFlyQuantityText;

        private void SettingApply ()
        {
            GlobalData.maxBeeInFlower = Convert.ToInt32(flowerBeeLimitText.GetComponent<Text>()?.text);
            GlobalData.beeSpeedLimit = Convert.ToInt32(beeSpeedText.GetComponent<Text>()?.text);
            GlobalData.flowerHoneyLimit = Convert.ToInt32(flowerHoneyText.GetComponent<Text>().text);

            GlobalData.flowerQuantity = Convert.ToInt32(flowerQuantityText.GetComponent<Text>().text);
            GlobalData.beeQuantity = Convert.ToInt32(beeQuantityText.GetComponent<Text>().text);
            GlobalData.beeFlyQuantity = Convert.ToInt32(beeFlyQuantityText.GetComponent<Text>().text);

            GlobalData.settingCompleted = false;
        }

        void Update()
        {
            if (buttonApply.GetComponent<ButtonCheck>().isClicked == true)
            {
                SettingApply();
            }
        }
    }
}
