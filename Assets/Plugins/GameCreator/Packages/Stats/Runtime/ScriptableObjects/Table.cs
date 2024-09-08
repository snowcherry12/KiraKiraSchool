using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [CreateAssetMenu(
        fileName = "Table", 
        menuName = "Game Creator/Stats/Table",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoTable.png")]
    
    public class Table : ScriptableObject
    {
        //   +-----------------+
        //   |   TABLE CHART   |
        //   +-----------------+
        // 
        //   +--------------+--+
        //   |              |  |
        //   |           +--+  |
        //   |           |  |  | <- Each bar represents the Level Experience to go from that Level
        //   |        +--+  |  |    to the next one.
        //   |        |  |  |  |
        //   |     +--+  |  |  |    Level Experience: Amount of experience needed to go from a
        //   |     |  |  |  |  |    to the next one.
        //   |  +--+  |  |  |  |    
        //   |  |  |  |  |  |  |    Cumulative Experience: The sum of Level Experience values
        //   |--+  |  |  |  |  |    from all previous levels.
        //   |  |  |  |  |  |  |
        //   +--+--+--+--+--+--+
        //   |1 |2 |3 |4 |5 |6 | <- Each number represents a Level
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeReference] private TTable m_Table = new TableLinearProgression();

        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Returns the level from the cumulative experience value provided.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public int CurrentLevel(int experience)
        {
            return this.m_Table?.GetLevelForCumulativeExperience(experience) ?? 0;
        }

        /// <summary>
        /// Returns the step-experience value between the the current and the next level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int ExperienceForLevel(int level)
        {
            return this.m_Table?.GetLevelExperienceForLevel(level) ?? 0;
        }

        /// <summary>
        /// Returns the cumulative experience for the specified level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int CumulativeExperienceForLevel(int level)
        {
            return this.m_Table?.GetCumulativeExperienceForLevel(level) ?? 0;
        }

        /// <summary>
        /// Returns the amount of experience accumulated at the current level from the cumulative
        /// experience value provided.
        /// </summary>
        /// <param name="cumulative"></param>
        /// <returns></returns>
        public int ExperienceForCurrentLevel(int cumulative)
        {
            return this.m_Table?.GetLevelExperienceAtCurrentLevel(cumulative) ?? 0;
        }
        
        /// <summary>
        /// Returns the amount of experience left from the cumulative experience value to reach
        /// the next level.
        /// </summary>
        /// <param name="cumulative"></param>
        /// <returns></returns>
        public int ExperienceToNextLevel(int cumulative)
        {
            return this.m_Table?.GetLevelExperienceToNextLevel(cumulative) ?? 0;
        }
        
        /// <summary>
        /// Returns a value between 0 and 1 indicating the percentage of experience accumulated at
        /// the current level.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public float RatioFromCurrentLevel(int experience)
        {
            return this.m_Table?.GetRatioAtCurrentLevel(experience) ?? 0f;
        }

        /// <summary>
        /// Returns a value between 0 and 1 indicating the percentage of experience left to reach
        /// the next level.
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public float RatioForNextLevel(int experience)
        {
            return this.m_Table?.GetRatioForNextLevel(experience) ?? 0f;
        }
    }
}