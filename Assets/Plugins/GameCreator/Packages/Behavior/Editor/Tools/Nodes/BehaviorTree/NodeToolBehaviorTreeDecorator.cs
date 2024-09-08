using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Behavior
{
    internal class NodeToolBehaviorTreeDecorator : TNodeToolBehaviorTree
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override float Width => 150f;

        public override bool CanMove => true;
        public override bool CanDelete => true;
        
        protected override bool ShowHead => true;
        protected override bool ShowBody => false;
        protected override bool ShowFoot => false;
        
        protected override bool DrawConditions => false;
        protected override bool DrawInstructions => false;

        public override string Title
        {
            get
            {
                TDecorator decorator = this.Property
                    .FindPropertyRelative("m_Decorator")
                    .managedReferenceValue as TDecorator;

                IEnumerable<TitleAttribute> titleAttributes = decorator?.GetType()
                    .GetCustomAttributes<TitleAttribute>();
                
                return titleAttributes?.FirstOrDefault()?.Title ?? "(none)";
            }
        }

        public override Texture Icon
        {
            get
            {
                TDecorator decorator = this.Property
                    .FindPropertyRelative("m_Decorator")
                    .managedReferenceValue as TDecorator;

                IEnumerable<ImageAttribute> imageAttributes = decorator?.GetType()
                    .GetCustomAttributes<ImageAttribute>();

                Texture2D image = imageAttributes?.FirstOrDefault()?.Image;
                return image != null ? image : Texture2D.whiteTexture;
            }
        }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeToolBehaviorTreeDecorator(TGraphTool graphTool, SerializedProperty property)
            : base(graphTool, property)
        { }
    }
}