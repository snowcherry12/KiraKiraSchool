using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Behavior
{
    [Title("Random Selector")]
    [Category("Random Selector")]
    
    [Image(typeof(IconCompositeRandomSelector), ColorTheme.Type.TextLight)]
    [Description("Executes the children as a Selector in a random order")]
    
    [Serializable]
    public class CompositeRandomSelector : CompositeSelector
    {
        protected override int GetIndex(int index, ValueBehaviorTreeShuffle value)
        {
            return value?.Get(index) ?? index;
        }
    }
}