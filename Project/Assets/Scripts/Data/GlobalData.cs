using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data
{
    class GlobalData
    {
        public static int allHives;
        public static int flyingBees;
        public static int aliveBees;
        public static int aliveFlowers;

        #region settingsData
        public static int maxBeeInFlower;
        public static float beeSpeedLimit;
        public static int flowerHoneyLimit;

        public static int beeQuantity;
        public static int beeFlyQuantity;
        public static int flowerQuantity;

        public static bool settingCompleted = true;
        #endregion
    }
}
