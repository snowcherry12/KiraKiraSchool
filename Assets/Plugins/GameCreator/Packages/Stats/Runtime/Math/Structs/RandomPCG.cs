using System;

namespace GameCreator.Runtime.Stats
{
    // +------------------------------------------------------------------------------------------+
    // | Implementation of Permuted Congruential random number Generator (PCG) slightly modified  |
    // | to avoid unnecessary sequences/streams and make the algorithm run faster.                |
    // |                                                                                          |
    // | For more information see https://www.pcg-random.org/                                     |
    // +------------------------------------------------------------------------------------------+
    
    internal static class RandomPCG
    {
        private const ulong MULTIPLIER_64 = 6364136223846793005ul;

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private static ulong State;
        
        // CREATOR: -------------------------------------------------------------------------------

        static RandomPCG()
        {
            State = (ulong) UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static double Range(double a, double b)
        {
            uint r = GetUInt() & 0xFFFFFF;
            double f = r / (double) 0xFFFFFF;
            return a + f * (b - a);
        }
        
        public static double Dice(double rolls, double sides)
        {
            double total = 0;
            for (int i = 0; i < rolls; ++i)
            {
                double value = Math.Floor(Range(1, sides + 1));
                total += Math.Min(value, sides);
            }
            
            return Math.Floor(total);
        }

        public static double Chance(double chance)
        {
            double percent = Range(0, 1);
            return chance > percent ? 1 : 0;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static uint GetUInt()
        {
            ulong previousState = State;
            
            Step();
            return XOR_Shift(previousState);
        }
        
        private static void Step()
        {
            State = unchecked(State * MULTIPLIER_64);
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private static uint RotateRight(uint value, int rotation)
        {
            return (value >> rotation) | (value << (-rotation & 31));
        }

        private static uint XOR_Shift(ulong state)
        {
            uint value = (uint) (((state >> 18) ^ state) >> 27);
            int rotation = (int) (state >> 59);
            
            return RotateRight(value, rotation);
        }
    }
}
