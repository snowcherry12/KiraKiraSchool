using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using GameCreator.Runtime.Common;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor.UIElements;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public sealed class TBagElement : TypeSelectorValueElement
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/TBag";

        private const string NAME_HEAD_TOOLBAR = "GC-Inventory-TBag-Head-Toolbar";
        private const string NAME_HEAD_BUTTON = "GC-Inventory-TBag-Head-Toolbar-Btn";
        private const string NAME_HEAD_BUTTON_ICON = "GC-Inventory-TBag-Head-Toolbar-BtnIcon";
        private const string NAME_HEAD_BUTTON_LABEL = "GC-Inventory-TBag-Head-Toolbar-BtnLabel";
        private const string NAME_HEAD_BUTTON_ARROW = "GC-Inventory-TBag-Head-Toolbar-BtnArrow";
        
        private const string PROP_SHAPE = "m_Shape";
        private const string PROP_CONTENT = "m_Content";
        private const string PROP_EQUIPMENT = "m_Equipment";
        private const string PROP_WEALTH = "m_Wealth";

        private static readonly IIcon ICON_CHEVRON_DOWN = new IconChevronDown(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Toolbar;
        private Button m_Button;
        
        private Image m_ButtonIcon;
        private Label m_ButtonLabel;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string ElementNameRoot => "GC-Inventory-TBag-Root";
        protected override string ElementNameHead => "GC-Inventory-TBag-Head";
        protected override string ElementNameBody => "GC-Inventory-TBag-Body";

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TBagElement(SerializedProperty property) : base(property, false)
        {
            this.TypeSelector = new TypeSelectorFancyProperty(this.m_Property, this.m_Button);
            this.TypeSelector.EventChange += this.OnChange;
            
            this.m_Button.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);

            this.LoadHeadStyleSheet();
            this.RefreshHead();
        }

        protected override void CreateHead()
        {
            base.CreateHead();

            this.m_Toolbar = new VisualElement { name = NAME_HEAD_TOOLBAR };
            
            this.m_Button = new Button { name = NAME_HEAD_BUTTON };
            this.m_ButtonIcon = new Image { name = NAME_HEAD_BUTTON_ICON };
            this.m_ButtonLabel = new Label { name = NAME_HEAD_BUTTON_LABEL };
            
            this.m_Toolbar.Add(this.m_ButtonIcon);
            this.m_Toolbar.Add(this.m_ButtonLabel);
            
            this.m_Button.Add(new Image
            {
                image = ICON_CHEVRON_DOWN.Texture,
                name = NAME_HEAD_BUTTON_ARROW
            });

            this.m_Toolbar.Add(this.m_Button);
            this.m_Head.Add(this.m_Toolbar);
        }

        protected override void CreateBody()
        {
            this.m_Property.serializedObject.Update();
            
            SerializedProperty shape = this.m_Property.FindPropertyRelative(PROP_SHAPE);
            SerializedProperty content = this.m_Property.FindPropertyRelative(PROP_CONTENT);
            SerializedProperty equipment = this.m_Property.FindPropertyRelative(PROP_EQUIPMENT);
            SerializedProperty wealth = this.m_Property.FindPropertyRelative(PROP_WEALTH);

            PropertyField fieldShape = new PropertyField(shape);
            PropertyField fieldContent = new PropertyField(content);
            PropertyField fieldEquipment = new PropertyField(equipment);
            PropertyField fieldWealth = new PropertyField(wealth);

            fieldShape.Bind(this.m_Property.serializedObject);
            fieldContent.Bind(this.m_Property.serializedObject);
            fieldEquipment.Bind(this.m_Property.serializedObject);
            fieldWealth.Bind(this.m_Property.serializedObject);
            
            this.m_Body.Add(fieldShape);
            this.m_Body.Add(fieldContent);
            this.m_Body.Add(fieldEquipment);
            this.m_Body.Add(fieldWealth);
        }

        protected override void OnChange(Type prevType, Type newType)
        {
            base.OnChange(prevType, newType);
            this.RefreshHead();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void LoadHeadStyleSheet()
        {
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in sheets)
            {
                this.styleSheets.Add(styleSheet);
            }
        }
        
        private void RefreshHead()
        {
            this.m_Property.serializedObject.Update();
            
            Type fieldType = TypeUtils.GetTypeFromProperty(this.m_Property, true);
            ImageAttribute iconAttribute = fieldType
                .GetCustomAttributes<ImageAttribute>()
                .FirstOrDefault();
            
            this.m_ButtonIcon.image = iconAttribute?.Image;
            this.m_ButtonLabel.text = TypeUtils.GetTitleFromType(fieldType);
        }
    }
}
