using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class MT
    {
        int w = 32;
        int n = 624;
        int f = 1812433253;
        int m = 397;
        int r = 31;
        long a = 0x9908b0df;
        long d = 0xffffffff;
        long b = 0x9d2c5680;
        long c = 0xefc60000;
        int u = 11;
        int s = 7;
        int t = 15;
        int l = 18;
        long lowMask;
        long upMask;
        int recurIndex = 0;
        List<long> X = new List<long>();

        public MT(long seed)
        {
            SeedMT(seed);
        }

        public long TemperMT()
        {
            long result;
            if (recurIndex == n)
            {
                TwistMT();
            }
            long y = X[recurIndex];
            y = y ^ ((y >> u) & d);
            y = y ^ ((y << s) & b);
            y = y ^ ((y << t) & c);
            result = (y ^ (y >> l));
            recurIndex++;

            return result;
        }

        public void TwistMT()
        {
            for (int i = 0; i < n; i++)
            {
                lowMask = (1 << r) - 1L;
                upMask = (~lowMask) & ((1 << w) - 1);
                long num = (X[i] & upMask) + (X[(i + 1) % n] & lowMask);
                long numSlided = num >> 1;
                if (num % 2 == 1)
                {
                    numSlided ^= a;
                }
                X[i] = X[(i + m) % n] ^ numSlided;
            }

            recurIndex = 0;
        }

        void SeedMT(long seed)
        {
            X.Clear();
            X.Add(seed);
            recurIndex = 0;
            for (int i = 1; i < n; i++)
            {
                long value = (f * (X[i - 1] ^ (X[i - 1] >> (w - 2))) + i);
                X.Add(value);
            }
            TwistMT();
        }
    }
}
