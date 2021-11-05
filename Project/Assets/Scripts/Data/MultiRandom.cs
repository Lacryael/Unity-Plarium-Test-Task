using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Data
{
    public static class MultiRandom
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>  new Random(Interlocked.Increment(ref seed)));

        public static Random GetRandom()
        {
            return randomWrapper.Value;
        }
    }
}
