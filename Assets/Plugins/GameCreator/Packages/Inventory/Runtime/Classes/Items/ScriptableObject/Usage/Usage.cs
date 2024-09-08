using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Usage
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_CanUse = false;
        [SerializeField] private bool m_ConsumeWhenUse = true;
        [SerializeField] private PropertyGetDecimal m_Cooldown = GetDecimalDecimal.Create(0f);
        
        [SerializeField] private RunConditionsList m_ConditionsCanUse = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnUse = new RunInstructionsList();

        [SerializeField] private bool m_ExecuteFromParent = false;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool AllowUse => this.m_CanUse;
        public bool ConsumeWhenUse => this.m_ConsumeWhenUse;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public float GetCooldownDuration(Args args)
        {
            return (float) this.m_Cooldown.Get(args);
        }

        // CONDITION METHODS: ---------------------------------------------------------------------

        public static bool RunCanUse(Item item, Args args)
        {
            return RunCanUse(item, args, true);
        }
        
        private static bool RunCanUse(Item item, Args args, bool isLeaf)
        {
            if (isLeaf)
            {
                if (args?.Self == null) return false;
                if (!item.Usage.m_CanUse) return false;
            }
            
            if (item.Usage.m_ExecuteFromParent && item.Parent != null)
            {
                bool parentCanUse = RunCanUse(item.Parent, args, false);
                if (!parentCanUse) return false;
            }

            RunnerConfig config = new RunnerConfig
            {
                Name = $"Can Use {TextUtils.Humanize(item.name)}",
                Location = new RunnerLocationParent(args.Self.transform)
            };

            return item.Usage.m_ConditionsCanUse.Check(args, config);
        }
        
        // USAGE METHODS: -------------------------------------------------------------------------
        
        public static async Task RunOnUse(Item item, Args args)
        {
            try
            {
                if (item.Usage.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnUse(item.Parent, args);
                }

                RunnerConfig config = new RunnerConfig
                {
                    Name = $"On Use {TextUtils.Humanize(item.name)}",
                    Location = new RunnerLocationParent(args.Self.transform)
                };
                
                await item.Usage.m_InstructionsOnUse.Run(args, config);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
    }
}