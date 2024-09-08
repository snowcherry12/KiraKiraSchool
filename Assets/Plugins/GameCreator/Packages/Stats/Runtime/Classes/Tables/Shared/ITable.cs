namespace GameCreator.Runtime.Stats
{
    public interface ITable
    {
        int GetLevelForCumulativeExperience(int cumulative);
        
        int GetCumulativeExperienceForLevel(int level);
        int GetLevelExperienceForLevel(int cumulative);
        
        int GetLevelExperienceAtCurrentLevel(int cumulative);
        int GetLevelExperienceToNextLevel(int cumulative);

        float GetRatioAtCurrentLevel(int cumulative);
        float GetRatioForNextLevel(int cumulative);
    }
}