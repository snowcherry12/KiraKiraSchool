using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Set Quest")]
    [Description("Sets a Quest value equal to another one")]

    [Category("Quests/Set Quest")]

    [Parameter("Set", "Where the value is set")]
    [Parameter("From", "The value that is set")]

    [Keywords("Change", "Task", "Variable", "Asset")]
    [Image(typeof(IconQuestSolid), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionGameObjectSetGameObject : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private PropertySetQuest m_Set = SetQuestNone.Create;
        
        [SerializeField]
        private PropertyGetQuest m_From = new PropertyGetQuest();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Set {this.m_Set} = {this.m_From}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override System.Threading.Tasks.Task Run(Args args)
        {
            Quest value = this.m_From.Get(args);
            this.m_Set.Set(value, args);

            return DefaultResult;
        }
    }
}