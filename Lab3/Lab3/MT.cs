using System.Collections.Generic;

namespace Lab3
{
    class MT
    {
        int w = 32;
        int n = 624;
        int f = 1812433253;
        int m = 397;
        int r = 31;

        uint a = 0x9908b0df;
        uint d = 0xffffffff;
        uint b = 0x9d2c5680;
        uint c = 0xefc60000;

        int u = 11;
        int s = 7;
        int t = 15;
        int l = 18;

        long lowMask;
        long upMask;

        int recurIndex = 0;
        List<uint> X = new List<uint>();

        public MT(uint seed)
        {
            SeedMT(seed);
        }

        public uint TemperMT()
        {
            uint result;
            if (recurIndex == n)
            {
                TwistMT();
            }
            uint y = X[recurIndex];
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
                uint num = (uint)((X[i] & upMask) + (X[(i + 1) % n] & lowMask));
                uint numSlided = num >> 1;
                if (num % 2 == 1)
                {
                    numSlided ^= a;
                }
                X[i] = X[(i + m) % n] ^ numSlided;
            }

            recurIndex = 0;
        }

        void SeedMT(uint seed)
        {
            X.Clear();
            X.Add(seed);
            recurIndex = 0;
            for (int i = 1; i < n; i++)
            {
                uint value = ((uint)(f * (X[i - 1] ^ (X[i - 1] >> (w - 2))) + i));
                X.Add(value);
            }
            TwistMT();
        }
    }
}
