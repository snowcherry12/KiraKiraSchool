using System;

namespace GameCreator.Runtime.Stats
{
    internal static class Functions
    {
        public static double TableLevel(Domain data, double value)
        {
            int input = (int) Math.Floor(value);
            return data.Table.CurrentLevel(input);
        }

        public static double TableValue(Domain data, double value)
        {
            int level = (int) Math.Floor(value);
            return data.Table.CumulativeExperienceForLevel(level);
        }

        public static double TableIncrement(Domain data, double value)
        {
            int level = (int) Math.Floor(value);
            return data.Table.ExperienceForLevel(level);
        }

        public static double TableRatioForCurrentLevel(Domain data, double value)
        {
            int input = (int) Math.Floor(value);
            return data.Table.RatioFromCurrentLevel(input);
        }

        public static double TableExpForCurrentLevel(Domain data, double value)
        {
            int input = (int) Math.Floor(value);
            return data.Table.ExperienceForCurrentLevel(input);
        }

        public static double TableExpToNextLevel(Domain data, double value)
        {
            int input = (int) Math.Floor(value);
            return data.Table.ExperienceToNextLevel(input);
        }
    }
}