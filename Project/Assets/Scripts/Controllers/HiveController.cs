using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Data;
using Random = System.Random;

namespace Assets.Scripts
{
    public class HiveController : MonoBehaviour
    {
        public GameObject beePrefab;
        public int beeHoneyMax = 100;
        public int hiveHoneyMax = 1000;
        public int hiveBeeMax = 20;
        public float beeSpeed = 2f;

        private Vector3 spawnPoint;
        private Vector3 flowerTargetPoint;

        private List<BeesData> bees = new List<BeesData>();
        private HivesData hive = new HivesData();

        private int hiveAliveBees;
        private int hiveAllBees;
        private int hiveFlyingBees;
        private int hiveAliveDrone;
        void Start()
        {
            GlobalData.beeSpeedLimit = beeSpeed;

            GlobalData.flyingBees = 0;
            hiveFlyingBees = 0;
            hiveAliveBees = 0;
            hiveAllBees = 0;
            spawnPoint = gameObject.transform.position;
            spawnPoint.y = gameObject.transform.position.y + 1;

            int id = 0;
            string nameIgnore = gameObject.name.Replace("Hive", "");
            if (int.TryParse(nameIgnore, out _))
            {
               id = Convert.ToInt32(nameIgnore);
            }
            GlobalData.allHives++;

            Random rnd = MultiRandom.GetRandom();
            float beeRespawn = rnd.Next(4, 10);
            float beeDie = rnd.Next(15, 25);

            hive.hiveId = id;
            hive.beeMax = hiveBeeMax;
            hive.beeRespawn = beeRespawn;
            hive.beeDie = beeDie;
            hive.honey = 0;
            hive.honeyMax = hiveHoneyMax;

            InvokeRepeating("BeeCreator", 0, hive.beeRespawn);
            InvokeRepeating("BeeKiller", 0, hive.beeDie);
            InvokeRepeating("BeeCheck", 0, 0.5f);
        }
        private void BeeCreator()
        {
            if (hiveAliveBees < hive.beeMax)
            {
                Random rnd = MultiRandom.GetRandom();
                int droneTest = rnd.Next(-10, 2);
                if (droneTest < 0)
                {
                    bees.Add(new BeesData { beeId = hiveAllBees, hiveId = hive.hiveId, flowerId = 0, honey = 0, honeyMax = beeHoneyMax, beeStatus = BeeStatus.InHive });
                    GlobalData.aliveBees++;
                    hiveAliveBees++;
                    hiveAllBees++;
                }
                else
                {
                    bees.Add(new BeesData { beeId = hiveAllBees, hiveId = hive.hiveId, flowerId = 0, honey = 0, honeyMax = 0, beeStatus = BeeStatus.Drone });
                    GlobalData.aliveBees++;
                    hiveAliveBees++;
                    hiveAliveDrone++;
                    hiveAllBees++;
                }
            }
        }
        private void BeeCheck()
        {
            if (hive.honey < hive.honeyMax && hiveAliveBees > hiveFlyingBees && GlobalData.aliveBees > GlobalData.flyingBees && bees[hiveFlyingBees].beeStatus == BeeStatus.InHive)
            {
                BeeSpawner();
            }
        }
        private void BeeSpawner()
        {
            foreach (var bee in bees.ToList())
            {
                if (bee.beeStatus == BeeStatus.InHive)
                {
                    GameObject newBee = Instantiate(beePrefab, spawnPoint, Quaternion.Euler(0, 90, 0));
                    newBee.name = ("bee" + hive.hiveId + bee.beeId);
                    hiveFlyingBees++;
                    GlobalData.flyingBees++;
                    bee.beeStatus = BeeStatus.FindFlower;
                    break;
                }
            }
        }
        private void BeeKiller()
        {
            if (hiveAliveBees > 1 && hiveAliveDrone > 0)
            {
                foreach (var bee in bees.ToList())
                {
                    if (bee.beeStatus == BeeStatus.Drone)
                    {
                        hiveAliveDrone--;
                        bee.beeStatus = BeeStatus.Dying;
                        break;
                    }
                }
            }
            else if (hiveAliveBees > 1 && hiveAliveDrone <= 0)
            {
                if (bees.Any(_ => _.beeStatus == BeeStatus.InHive))
                {
                    foreach (var bee in bees.ToList())
                    {
                        if (bee.beeStatus == BeeStatus.InHive)
                        {
                            bee.beeStatus = BeeStatus.Dying;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var bee in bees.ToList())
                    {
                        bee.beeStatus = BeeStatus.FlyingToDie;
                        break;
                    }
                }
            }
        }
        private FlowersData GetFlower(BeesData bee)
        {
            if  (bee.beeStatus == BeeStatus.FindFlower)
            {
                foreach (var flower in FlowerController.flowers.ToList())
                {
                    if (flower.bees < GlobalData.maxBeeInFlower)
                    {
                        return flower;
                    }
                }
                return null;
            }
            if (bee.beeStatus == BeeStatus.OnFlower || bee.beeStatus == BeeStatus.FlyingToFlower)
            {
                foreach (var flower in FlowerController.flowers.ToList())
                {
                    if (bee.flowerId == flower.flowerId)
                    {
                        return flower;
                    }
                }
                return null;
            }
            else return null; 
        }
        private void BeeFlyManager()
        {
            foreach (var bee in bees.ToList())
            {
                if (bee.beeStatus == BeeStatus.FindFlower)
                {
                    var flower = GetFlower(bee);
                    BeesFindFlower(bee, flower);
                }
                if (bee.beeStatus == BeeStatus.FlyingToFlower)
                { 
                    var flower = GetFlower(bee);
                    BeesFlyToFlower(bee, flower);
                }
                if (bee.beeStatus == BeeStatus.FlyingToHive)
                {
                    BeesFlyToHive(bee);
                }
                if (bee.beeStatus == BeeStatus.OnFlower)
                {
                    var flower = GetFlower(bee);
                    DoBeesEatFlowers(bee, flower);
                }
                if (bee.beeStatus == BeeStatus.PlacesHoneyInHive)
                {
                    HoneyGoHome(bee);
                }
                if (bee.beeStatus == BeeStatus.FlyingToDie)
                {
                    BeeFliesToDie(bee);
                }
                if (bee.beeStatus == BeeStatus.Dying)
                {
                    BeeDying(bee);
                }
            }
        }
        private void BeesFindFlower(BeesData bee, FlowersData flower)
        {
            if (flower != null && bee != null)
            {
                if (hive.honey >= hive.honeyMax) bee.beeStatus = BeeStatus.FlyingToHive;
                if (bee.flowerId == 0)
                {
                    flower.bees++;
                    bee.flowerId = flower.flowerId;
                }
                if (bee.flowerId != 0)
                {
                    bee.beeStatus = BeeStatus.FlyingToFlower;
                }
            }
            if (bee != null && flower == null)
            {
                if(bee.honey < bee.honeyMax)
                {
                    bee.flowerId = 0;
                    bee.beeStatus = BeeStatus.FindFlower;
                }
                else
                {
                    bee.flowerId = 0;
                    bee.beeStatus = BeeStatus.FlyingToHive;
                }
            }
        }
        private void BeesFlyToFlower(BeesData bee, FlowersData flower)
        {
            if (flower != null && bee != null)
            {
                bee.flowerId = flower.flowerId;
                var beeName = "bee" + bee.hiveId + bee.beeId;
                var beeIndividual = GameObject.Find(beeName);
                flowerTargetPoint = GameObject.Find("flower" + flower?.flowerId).transform.position;

                Vector3 direction = Vector3.zero;

                if (flowerTargetPoint != Vector3.zero && beeIndividual != null)
                {
                    direction = flowerTargetPoint - beeIndividual.transform.position;
                }
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    beeIndividual.transform.rotation = rotation;
                    beeIndividual.transform.position = Vector3.MoveTowards(beeIndividual.transform.position, flowerTargetPoint, GlobalData.beeSpeedLimit * Time.deltaTime);
                }
                if (direction == Vector3.zero)
                {
                    bee.beeStatus = BeeStatus.FindFlower;
                }
                if (beeIndividual?.transform.position == flowerTargetPoint)
                {
                    bee.beeStatus = BeeStatus.OnFlower;
                }
            }
            if (bee != null && flower == null)
            {
                bee.flowerId = 0;
                bee.beeStatus = BeeStatus.FindFlower;
            }
        }
        private void BeesFlyToHive(BeesData bee)
        {
            if (bee != null)
            {
                var beeName = "bee" + bee.hiveId + bee.beeId;
                var beeIndividual = GameObject.Find(beeName);
                Vector3 hivePoint = gameObject.transform.position;
                hivePoint.z = gameObject.transform.position.z - 0.5f;

                Vector3 direction = Vector3.zero;
                if (flowerTargetPoint != Vector3.zero && beeIndividual != null)
                {
                    direction = hivePoint - beeIndividual.transform.position;
                }
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    beeIndividual.transform.rotation = rotation;
                    beeIndividual.transform.position = Vector3.MoveTowards(beeIndividual.transform.position, hivePoint, GlobalData.beeSpeedLimit * Time.deltaTime);
                }
                if (direction == Vector3.zero)
                {
                    bee.beeStatus = BeeStatus.FindFlower;
                }
                if (beeIndividual.transform.position == hivePoint)
                {
                    bee.beeStatus = BeeStatus.PlacesHoneyInHive;
                }
            }
        }
        private void HoneyGoHome(BeesData bee)
        {
            if (bee != null)
            {
                if (bee.honey > 0 && hive.honey < hive.honeyMax && Time.time % 1 == 0)
                {
                    bee.honey = bee.honey - 5;
                    hive.honey = hive.honey + 5;
                }
                if (hive.honey >= hive.honeyMax)
                {
                    bee.beeStatus = BeeStatus.InHive;
                    bee.honey = 0;
                    Destroy(GameObject.Find("bee" + bee.hiveId + bee.beeId));
                    hiveFlyingBees--;
                    GlobalData.flyingBees--;
                }
                if (bee.honey <= 0 && hive.honey != hive.honeyMax)
                {
                    bee.honey = 0;
                    bee.beeStatus = BeeStatus.FindFlower;
                }
            }
        }
        private void DoBeesEatFlowers(BeesData bee, FlowersData flower)
        {
            if (flower != null && bee != null)
            {
                if (bee.honey < bee.honeyMax && flower.honey > 0 && Time.time % 1 == 0)
                {
                    bee.honey = bee.honey + 5;
                    flower.honey = flower.honey - 5;
                }
                else if (bee.honey >= bee.honeyMax)
                {
                    if (bee.flowerId != 0)
                    {
                        flower.bees--;
                        bee.flowerId = 0;
                    }
                    bee.beeStatus = BeeStatus.FlyingToHive;
                }
                else if (flower.honey <= 0)
                {
                    bee.flowerId = 0;
                    bee.beeStatus = BeeStatus.FindFlower;
                }
            }
            if (flower == null && bee != null)
            {
                bee.flowerId = 0;
                bee.beeStatus = BeeStatus.FindFlower;
            }
        }
        private void BeeFliesToDie(BeesData bee)
        {
            if (bee != null)
            {
                var beeName = "bee" + bee.hiveId + bee.beeId;
                var beeIndividual = GameObject.Find(beeName);

                Vector3 diePoint = Vector3.zero;
                Vector3 direction = Vector3.zero;
                if (flowerTargetPoint != Vector3.zero && beeIndividual != null)
                {
                    diePoint = beeIndividual.transform.position;
                    diePoint.y = gameObject.transform.position.y + 3f;
                    direction = diePoint - beeIndividual.transform.position;
                }
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    beeIndividual.transform.rotation = rotation;
                    beeIndividual.transform.position = Vector3.MoveTowards(beeIndividual.transform.position, diePoint, GlobalData.beeSpeedLimit * Time.deltaTime);
                }
                if (direction == Vector3.zero)
                {
                    bee.beeStatus = BeeStatus.FlyingToDie; 
                }
                if (beeIndividual.transform.position == diePoint)
                {
                    bee.honey = 0;
                    bee.flowerId = 0;
                    Destroy(beeIndividual);
                    hiveFlyingBees--;
                    GlobalData.flyingBees--;
                    bee.beeStatus = BeeStatus.Dying;
                }
            }
        }
        private void BeeDying(BeesData bee)
        {
            bees.Remove(bee); 
            GlobalData.aliveBees--;
            hiveAliveBees--;
        }
        private void HiveUi()
        {
            foreach (Transform HiveText in gameObject.transform)
            {
                if (HiveText.name == "HiveText")
                {
                    foreach (Transform text in HiveText)
                    {
                        if (text.name == "beeText")
                        {
                            text.GetComponent<TextMesh>().text = $"{hiveAliveBees}({hiveAliveDrone})/{hive.beeMax}";
                        }
                        if (text.name == "honeyText")
                        {
                            text.GetComponent<TextMesh>().text = $"{hive.honey}/{hive.honeyMax}";
                        }
                    }
                }
                    
            }
        }
        private void SettingSpawn()
        {
            if (GlobalData.settingCompleted == false)
            {
                for (int i = 0; i < GlobalData.beeQuantity; i++)
                {
                    BeeCreator();
                }
                for (int i = 0; i < GlobalData.beeFlyQuantity; i++)
                {
                    BeeSpawner();
                }
                if(Time.time%2 == 0)GlobalData.settingCompleted = true;
            }
        }
        void FixedUpdate()
        {
            HiveUi();
            BeeFlyManager();
            SettingSpawn();
        }
    }
}
