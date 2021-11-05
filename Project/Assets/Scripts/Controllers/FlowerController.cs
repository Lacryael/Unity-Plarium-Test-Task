using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Data;
using Random = System.Random;

namespace Assets.Scripts
{
    public class FlowerController : MonoBehaviour
    {
        public int flowerQuantity = 2;
        public float flowerSpeed = 1;
        public int flowerHoney = 1000;
        public int flowerMaxBee = 10;

        public static List<FlowersData> flowers = new List<FlowersData>();
        private double requiredFlowers;

        void Start()
        {
            GlobalData.aliveFlowers = 0;
            GlobalData.maxBeeInFlower = flowerMaxBee;
            GlobalData.flowerHoneyLimit = flowerHoney;

            InvokeRepeating("CheckFlower", 0, flowerSpeed);
            InvokeRepeating("FlowerPerdition", 0, 1f);
        }

        private void CheckFlower()
        {
            requiredFlowers = GlobalData.flyingBees / 10 + 2;
            if (GlobalData.aliveFlowers < 110 && flowerQuantity > GlobalData.aliveFlowers)
            {
                flowerQuantity = (flowerQuantity > 110) ? 110 : flowerQuantity;
                SpawnFlower(flowerQuantity);
            }
            else if (GlobalData.aliveFlowers < 110 && requiredFlowers > GlobalData.aliveFlowers && flowerQuantity != 0 && flowerQuantity != 1)
            {
                int flowersToAdd = (int)Math.Ceiling(requiredFlowers) - GlobalData.aliveFlowers;
                SpawnFlower(flowersToAdd);
            }
        }
        private void SpawnFlower(int flowersToAdd)
        {
            Random rnd = new Random();

            for (int i = 0; i < flowersToAdd; i++)
            {
                int flowerId = rnd.Next(101, 211);
                if (flowers.Any(_ => _.flowerId == flowerId) == false)
                {
                    flowers.Add(new FlowersData { flowerId = flowerId, bees = 0, maxBee = GlobalData.maxBeeInFlower, honey = GlobalData.flowerHoneyLimit, honeyMax = GlobalData.flowerHoneyLimit });
                    transform.Find("flower" + flowerId).gameObject.SetActive(true);
                    GetText(flowerId);
                    GlobalData.aliveFlowers++;
                }
            }
        }
        private void FlowerPerdition()
        {
            if (flowers.Any(_ => _.honey <= 0))
            {
                foreach (var flower in flowers.ToList())
                {
                    if (flower.honey <= 0)
                    {
                        transform.Find("flower" + flower.flowerId).gameObject.SetActive(false);
                        flowers.Remove(flower);
                        GlobalData.aliveFlowers--;
                    }
                }
            }
        }
        private void HoneyGeneration()
        {
            foreach (var flower in flowers.ToList())
            {
                if (flower.honey < flower.honeyMax && Time.time % 1 == 0)
                {
                    flower.honey = flower.honey + 5;
                }
            }
        }
        private void GetText(int flowerId)
        {
            var parentFlower = transform.Find("flower" + flowerId);
            if (parentFlower.transform.childCount == 0)
            {
                GameObject flowerStat = new GameObject("FlowerText");
                flowerStat.transform.SetParent(transform.Find("flower" + flowerId).transform);
                flowerStat.layer = 5;
                flowerStat.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                flowerStat.transform.rotation = Quaternion.Euler(20, 0, 0);
                var flowerText = flowerStat.AddComponent<TextMesh>();
                flowerStat.transform.position = parentFlower.position - new Vector3(0.35f,0,0);
                flowerText.fontSize = 15;
                flowerText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                flowerText.text = "Пчелы: 0/0\nМёд: 0/0";
            }
        }
        private void FlowerUi()
        {
            foreach (var flower in flowers.ToList())
            {
                if (flower.flowerId != 0)
                {
                    GetFlowerUi(flower);
                }
            }
        }
        private void GetFlowerUi(FlowersData flower)
        {
            var parentFlower = transform.Find("flower" + flower.flowerId);
            if (parentFlower.transform.childCount != 0)
            {
                foreach (Transform text in parentFlower)
                {
                    if (text.name == "FlowerText")
                    {
                        text.GetComponent<TextMesh>().text = $"Пчёлы: {flower.bees}/{GlobalData.maxBeeInFlower}\nМёд: {flower.honey}/{flower.honeyMax}";
                    }
                }
            }

        }
        private void SettingSpawn()
        {
            if (GlobalData.settingCompleted == false)
            {
                SpawnFlower(GlobalData.flowerQuantity);
                if (Time.time % 2 == 0) GlobalData.settingCompleted = true;
            }
        }
        void FixedUpdate()
        {
            FlowerUi();
            FlowerPerdition();
            HoneyGeneration();
            SettingSpawn();
        }
    }
}
