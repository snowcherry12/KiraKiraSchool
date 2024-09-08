using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Equip
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_IsEquippable;
        [SerializeField] private GameObject m_Prefab;

        [SerializeField] private RunConditionsList m_ConditionsEquip = new RunConditionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnEquip = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_InstructionsOnUnequip = new RunInstructionsList();
        
        [SerializeField] private bool m_ExecuteFromParent;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsEquippable => this.m_IsEquippable;
        public GameObject Prefab => this.m_Prefab;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------ 
        
        public static bool RunCanEquip(Item item, Args args)
        {
            if (item.Equip.m_ExecuteFromParent && item.Parent != null)
            {
                bool parentCanEquip = RunCanEquip(item.Parent, args);
                if (!parentCanEquip) return false;
            }

            RunnerConfig config = new RunnerConfig
            {
                Name = $"Can Equip {TextUtils.Humanize(item.name)}",
                Location = new RunnerLocationParent(args.Self.transform)
            };
            
            return item.Equip.m_ConditionsEquip.Check(args, config);
        }
        
        public static async Task RunOnEquip(Item item, Args args)
        {
            try
            {
                if (item.Equip.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnEquip(item.Parent, args);
                }

                RunnerConfig config = new RunnerConfig
                {
                    Name = $"On Equip {TextUtils.Humanize(item.name)}",
                    Location = new RunnerLocationParent(args.Self.transform)
                };

                await item.Equip.m_InstructionsOnEquip.Run(args, config);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
        
        public static async Task RunOnUnequip(Item item, Args args)
        {
            try
            {
                if (item.Equip.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnUnequip(item.Parent, args);
                }

                RunnerConfig config = new RunnerConfig
                {
                    Name = $"Can Unequip {TextUtils.Humanize(item.name)}",
                    Location = new RunnerLocationParent(args.Self.transform)
                };

                await item.Equip.m_InstructionsOnUnequip.Run(args, config);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
    }
}