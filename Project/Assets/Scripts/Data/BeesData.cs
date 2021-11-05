using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data
{
    public class BeesData
    {
        public int beeId { get; set; }
        public int hiveId { get; set; }
        public int flowerId { get; set; }
        public int honey { get; set; }
        public int honeyMax { get; set; }
        public BeeStatus beeStatus { get; set; }

    }
}
