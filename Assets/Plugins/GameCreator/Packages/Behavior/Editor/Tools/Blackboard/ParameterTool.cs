using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ParameterTool : VisualElement
    {
        private const string NAME_CONTENT = "GC-Overlays-Blackboard-Body-Parameter-Content";
        private const string NAME_DROP_1 = "GC-Overlays-Blackboard-Body-ParameterDrop-Above";
        private const string NAME_DROP_2 = "GC-Overlays-Blackboard-Body-ParameterDrop-Below";
        
        private const string NAME_DRAG = "GC-Overlays-Blackboard-Body-ParameterDrag";
        private const string NAME_TYPE = "GC-Overlays-Blackboard-Body-ParameterType";
        private const string NAME_NAME = "GC-Overlays-Blackboard-Body-ParameterName";
        private const string NAME_DELETE = "GC-Overlays-Blackboard-Body-ParameterDelete";

        private static readonly IIcon ICON_DELETE = new IconMinus(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_DRAG = new IconDrag(ColorTheme.Type.TextLight);

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly ParametersTool m_Parent;
        private readonly SerializedProperty m_Property;
        private readonly int m_Index;

        private readonly VisualElement m_Content;
        private readonly VisualElement m_DropAbove;
        private readonly VisualElement m_DropBelow;
        
        private readonly TextField m_InputRename;

        private readonly Image m_IconType;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private ITypeSelector TypeSelector { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ParameterTool(ParametersTool parent, SerializedProperty property, int index)
        {
            this.m_Parent = parent;
            this.m_Property = property;
            this.m_Index = index;

            this.m_Content = new VisualElement { name = NAME_CONTENT };
            this.m_DropAbove = new VisualElement { name = NAME_DROP_1 };
            this.m_DropBelow = new VisualElement { name = NAME_DROP_2 };
            
            this.Add(this.m_Content);
            this.Add(this.m_DropAbove);
            this.Add(this.m_DropBelow);
            
            VisualElement handleDrag = new VisualElement { name = NAME_DRAG };
            Button buttonType = new Button { name = NAME_TYPE };
            Button buttonDelete = new Button(this.Delete) { name = NAME_DELETE };
            this.m_InputRename = new TextField { name = NAME_NAME };
            
            this.m_Content.Add(handleDrag);
            this.m_Content.Add(buttonType);
            this.m_Content.Add(this.m_InputRename);
            this.m_Content.Add(new FlexibleSpace());
            this.m_Content.Add(buttonDelete);

            this.m_IconType = new Image { pickingMode = PickingMode.Ignore };
            
            Image iconDrag = new Image
            {
                image = ICON_DRAG.Texture,
                pickingMode = PickingMode.Ignore
            };
            
            Image iconDelete = new Image
            {
                image = ICON_DELETE.Texture,
                pickingMode = PickingMode.Ignore
            };
            
            handleDrag.Add(iconDrag);
            buttonType.Add(this.m_IconType);
            buttonDelete.Add(iconDelete);

            this.Refresh();

            handleDrag.AddManipulator(this.m_Parent.ManipulatorSort);
            
            this.m_InputRename.RegisterValueChangedCallback(this.Rename);

            SerializedProperty propertyValue = this.m_Property
                .GetArrayElementAtIndex(this.m_Index)
                .FindPropertyRelative("m_Value");
            
            this.TypeSelector = new TypeSelectorFancyProperty(propertyValue, buttonType);
            this.TypeSelector.EventChange += this.ChangeType;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void DisplayAsNormal()
        {
            this.m_Content.style.opacity = 1f;

            this.m_DropAbove.style.display = DisplayStyle.None;
            this.m_DropBelow.style.display = DisplayStyle.None;
        }
        
        public void DisplayAsDrag()
        {
            this.m_Content.style.opacity = 0.25f;
        }
        
        public void DisplayAsTargetAbove()
        {
            this.m_DropAbove.style.display = DisplayStyle.Flex;
        }
        
        public void DisplayAsTargetBelow()
        {
            this.m_DropBelow.style.display = DisplayStyle.Flex;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Refresh()
        {
            Parameter item = this.m_Property
                .GetArrayElementAtIndex(this.m_Index)
                .GetValue<Parameter>();

            this.m_IconType.image = item?.Icon;
            this.m_InputRename.value = item?.Name;
        }
        
        private void ChangeType(Type prevType, Type newType)
        {
            this.Refresh();

            this.m_Parent.SendChangeEvent();
        }

        private void Rename(ChangeEvent<string> changeEvent)
        {
            string newValue = TextUtils.ProcessID(changeEvent.newValue);
            this.m_Property.serializedObject.Update();
            this.m_Property
                .GetArrayElementAtIndex(this.m_Index)
                .FindPropertyRelative("m_Name")
                .stringValue = newValue;
            
            this.m_Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.m_InputRename.SetValueWithoutNotify(newValue);
        }

        private void Delete()
        {
            this.m_Property.serializedObject.Update();
            this.m_Property.DeleteArrayElementAtIndex(this.m_Index);

            this.m_Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            this.m_Parent.Refresh();
            
            this.m_Parent.SendChangeEvent();
        }
    }
}