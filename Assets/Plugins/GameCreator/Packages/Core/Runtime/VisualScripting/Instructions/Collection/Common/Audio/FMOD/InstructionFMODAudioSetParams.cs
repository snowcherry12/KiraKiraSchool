using System;
using System.Linq;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.FMOD;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Set FMOD Params")]
    [Description("Sets the Params of an FMOD Audio")]

    [Category("Audio/FMOD/Set FMOD Params")]

    [Parameter("Parameter Name", "The Animator parameter name to be modified")]
    [Parameter("Value", "The value of the parameter that is set")]

    [Keywords("Change", "FMOD", "Audio", "Params")]
    [Image(typeof(IconToggleOn), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionFMODAudiSetParams : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private PropertySetFMODAudio m_FMODAudio = SetFMODAudioNone.Create;
        
        [SerializeField]
        private Parameter[] m_Params = Array.Empty<Parameter>();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Set {this.m_FMODAudio}: {this.m_Params.Length} Parameters";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            
            if (fmodAudio == null || this.m_Params.Length == 0) return DefaultResult;
            foreach (Parameter param in this.m_Params)
            {
                bool isExist = false;
                for (int i = 0; i < fmodAudio.Params.Length; i++)
                {
                    if (fmodAudio.Params[i].Name == param.Name)
                    {
                        fmodAudio.Params[i] = param;
                        isExist = true;
                    }
                }
                if (!isExist) 
                {
                    Debug.Log($"Param Name {param.Name} is not exist");
                }
                // Debug.Log("");
                this.m_FMODAudio.Set(fmodAudio, args);
            }

            return DefaultResult;
        }
    }
}