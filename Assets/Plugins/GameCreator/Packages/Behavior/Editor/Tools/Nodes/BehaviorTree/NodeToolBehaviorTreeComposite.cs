using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolBehaviorTreeComposite : TNodeToolBehaviorTree
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 250f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => true;
        protected override bool ShowFoot => true;
        
        protected override bool DrawConditions => true;
        protected override bool DrawInstructions => false;

        public override string Title
        {
            get
            {
                TComposite composite = this.Property
                    .FindPropertyRelative("m_Composite")
                    .managedReferenceValue as TComposite;

                IEnumerable<TitleAttribute> titleAttributes = composite?.GetType()
                    .GetCustomAttributes<TitleAttribute>();
                
                return titleAttributes?.FirstOrDefault()?.Title ?? "(none)";
            }
        }

        public override Texture Icon
        {
            get
            {
                TComposite composite = this.Property
                    .FindPropertyRelative("m_Composite")
                    .managedReferenceValue as TComposite;

                IEnumerable<ImageAttribute> imageAttributes = composite?.GetType()
                    .GetCustomAttributes<ImageAttribute>();

                Texture2D image = imageAttributes?.FirstOrDefault()?.Image;
                return image != null ? image : Texture2D.whiteTexture;
            }
        }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolBehaviorTreeComposite(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}