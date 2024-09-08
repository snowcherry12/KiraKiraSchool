using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Random")]
    [Category("Random")]
    
    [Image(typeof(IconNodeRandom), ColorTheme.Type.TextLight)]
    [Description("Picks and runs a random element from its children")]

    [Serializable]
    public class NodeTypeRandom : TNodeType
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override bool IsBranch => true;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private bool m_AllowRepeat = true;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private int m_LastIndex = -1;

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override Task Run(int id, Story story, Args args)
        {
            return Task.CompletedTask;
        }

        public override List<int> GetNext(int id, Story story, Args args)
        {
            List<int> children = story.Content.Children(id);
            DialogueSkin skin = story.Content.DialogueSkin;

            for (int i = children.Count - 1; i >= 0; --i)
            {
                int childId = children[i];
                
                bool canRun = story.Content.Get(childId).CanRun(args);
                if (!canRun) children.RemoveAt(i);
            }

            if (children.Count == 0) return new List<int>();
            bool allowRepeat = this.m_Options switch
            {
                NodeTypeData.FromSkin => skin != null && skin.ValuesRandom.AllowRepeat,
                NodeTypeData.FromNode => this.m_AllowRepeat,
                _ => throw new ArgumentOutOfRangeException()
            };

            int randomLength = allowRepeat || this.m_LastIndex < 0
                ? children.Count - 0
                : children.Count - 1;

            int randomIndex = UnityEngine.Random.Range(0, randomLength);
            if (!allowRepeat && randomIndex == this.m_LastIndex) randomIndex += 1;

            this.m_LastIndex = randomIndex;
            int randomId = children[randomIndex];
            
            return new List<int> { randomId };
        }
    }
}