using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    public class RunnerInstructionsList : TRunner<InstructionList>
    {
        public void Cancel() => this.m_Value?.Cancel();
    }
}