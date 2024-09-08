using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Text")]
    [Category("Text")]
    
    [Image(typeof(IconNodeText), ColorTheme.Type.TextLight)]
    [Description("Displays the element as a regular conversation text")]

    [Serializable]
    public class NodeTypeText : TNodeType
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override bool IsBranch => false;
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override Task Run(int id, Story story, Args args)
        {
            return Task.CompletedTask;
        }

        public override List<int> GetNext(int id, Story story, Args args)
        {
            return story.Content.Children(id);
        }
    }
}