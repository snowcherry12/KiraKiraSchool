using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Random Sequence")]
    [Category("Random Sequence")]
    
    [Image(typeof(IconCompositeRandomSequence), ColorTheme.Type.TextLight)]
    [Description("Executes its children as a Sequence but in a random a order")]
    
    [Serializable]
    public class CompositeRandomSequence : CompositeSequence
    {
        protected override int GetIndex(int index, ValueBehaviorTreeShuffle value)
        {
            return value?.Get(index) ?? index;
        }
    }
}